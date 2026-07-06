using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

/// <summary>
/// This class is responsible for the specialized behavior of Larry.
/// Reduces Larry trigger distance to avoid TTSSpeaker component missing error.
/// </summary>
public class SpecializedBehaviourLarry : MonoBehaviour
{
    [SerializeField]
    public float TriggerDistance = 2.5f; // Distance to trigger the dialogue

    private NPCSpawner _npcSpawner;
    private GameObject _larryNpc;

    // Start is called before the first frame update
    void Start()
    {
        _npcSpawner = GetComponent<NPCSpawner>();
        if (_npcSpawner == null)
        {
            Debug.LogError("No NPCSpawner found");
            return;
        }

        _larryNpc = _npcSpawner._npcInstances[0];

        // Reduce dialogue trigger radius to avoid TTSSpeaker component missing error from spawning inside NPC dialogue trigger distance
        CapsuleCollider capsuleCollider = _larryNpc.GetNamedChild("CollisionTriggerHandler").GetComponent<CapsuleCollider>();
        capsuleCollider.radius = TriggerDistance;
    }
}
