using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scripts for spawning objects. The object to be spawned is decided by a weighting.
/// </summary>
public class ObjectSpawner : MonoBehaviour, DestroyedListener
{
    // Editor fields
    [SerializeField]
    private List<Node> spawnables;
    [SerializeField][Range(0.1f,20f)]
    private float rate = 2;
    [SerializeField]
    [Range(1, 100)]
    private int maxSpawns = 5;
    [SerializeField]
    [Range(0.0f, 50f)]
    private float spawnRange = 3.0f;
    [SerializeField]
    [Tooltip("Amount to increase weight by if bad luck prevention is active.")]
    private int badLuckPreventionWeight;
    [SerializeField]
    private bool spawnImmediatly = false;
    [SerializeField]
    private bool disableSpawn = false;

    private Dictionary<int, GameObject> spawnedObjects; // List of objects spawned by this script.
    private int _totalWeight = 0;

    private void Start()
    {
        spawnedObjects = new Dictionary<int, GameObject>();
        spawnables.Sort((a, b) => a.Weight - b.Weight);
        foreach (Node node in spawnables)
        {
            _totalWeight += node.Weight;
            node.OriginalWeight = node.Weight;
        }
        InvokeRepeating("spawnObject", spawnImmediatly ? 0 : rate, rate);
    }


    /// <summary>
    /// Selects a node from spawnables using their weight as probability. 
    /// </summary>
    /// <returns>A GameObject used for instantiation.</returns>
    private GameObject selectSpawnable()
    {
        if (spawnables.Count > 0)
        {
            int randWeight = UnityEngine.Random.Range(1, _totalWeight);
            int curWeight = 0;
            Node selectedNode = null;
            foreach (Node node in spawnables)
            {
                curWeight += node.Weight;
                if (curWeight >= randWeight && selectedNode == null)
                {
                    selectedNode = node;
                    node.Weight = node.OriginalWeight;
                } 
                else
                {
                    if (node.BadLuckPrevention)
                    {
                        node.Weight += badLuckPreventionWeight;
                    }
                }
            }
            return selectedNode.Obj;
        }
        disableSpawn = true;
        return null;
    }

    /// <summary>
    /// Spawns an object selected from spawnables.
    /// </summary>
    private void spawnObject()
    {
        if (spawnedObjects.Count >= maxSpawns || disableSpawn) { return; }

        GameObject newObject = Instantiate(selectSpawnable(), gameObject.transform);
        newObject.transform.localPosition = UnityEngine.Random.insideUnitSphere * spawnRange;
        SpawnedObject spawnerChild = newObject.AddComponent<SpawnedObject>();
        spawnerChild.AddListener(this);
        spawnedObjects.Add(newObject.GetInstanceID(), newObject);
    }

    /// <summary>
    /// If the object is destroyed, remove it from the spawned list.
    /// </summary>
    /// <param name="obj">The object that was destroyed</param>
    public void Destroyed(GameObject obj)
    {
        if (spawnedObjects.ContainsKey(obj.GetInstanceID()))
        {
            spawnedObjects.Remove(obj.GetInstanceID());
        }
    }

    /// <summary>
    /// Toggles the spawner on and off.
    /// </summary>
    public void ToggleSpawner()
    {
        disableSpawn = !disableSpawn;
    }
}

/// <summary>
/// Class used for editor serialization of what objects to spawn in the objectspawner.
/// </summary>
[Serializable]
public class Node
{
    // Editor fields
    [SerializeField]
    [Tooltip("The object to spawn.")]
    private GameObject obj;
    [SerializeField]
    [Tooltip("Probability of this object spawning. A weight of 0 will make the object impossible to spawn.")]
    private int weight;
    [SerializeField]
    [Tooltip("When this is enabled, the weight of the object will increase if it fails to spawn.")]
    private bool badLuckPrevention;

    // used to reset weight after bad luck prevention
    private int originalWeight;

    // Getters and setters.
    public int Weight
    {
        get { return weight; }
        set { weight = value; }
    }

    public GameObject Obj
    {
        get { return obj; }
        set { obj = value; }
    }

    public bool BadLuckPrevention
    {
        get { return badLuckPrevention; }
    }

    public int OriginalWeight
    {
        get { return originalWeight; }
        set { originalWeight = value; }
    }
}

