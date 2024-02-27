using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class FactoryFishSpawner : MonoBehaviour
{
    // TODO: Not used at this state, but can be implemented down the line
    [HideInInspector]
    enum FishState
    {
        Alive,
        Stunned,
        Dead
    }

    [SerializeField]
    [Tooltip("The gameobject prefab to spawn")]
    private GameObject fishPrefab;

    [SerializeField]
    [Tooltip("The maximum amount of fish that can be spawned")]
    private int maxAmountOfFish;

    // Counts the amount of child gameobjects in the spawner
    private int currentAmountOfFish;

    [SerializeField]
    [Range(0.1f, 10)]
    [Tooltip("The rate at which the fish will spawn in seconds")]
    private float spawnRate;

    // TODO: private int varationInSpawnrate

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnFish());
    }

    void Update()
    {
        // Checks the amount of spawned gameobjects in the simulation
        currentAmountOfFish = gameObject.transform.childCount;
    }

    private IEnumerator SpawnFish()
    {
        // Waits for number of seconds equal to the spawnrate
        yield return new WaitForSeconds(spawnRate);

        if (currentAmountOfFish < maxAmountOfFish)
        {
            // spawn object as a child of the spawner object
            GameObject childGameObject = Instantiate(
                fishPrefab,
                transform.position,
                Quaternion.identity,
                transform
            );
            childGameObject.name = "FactoryFish" + gameObject.transform.childCount.ToString();
        }

        StartCoroutine(SpawnFish());
    }
}
