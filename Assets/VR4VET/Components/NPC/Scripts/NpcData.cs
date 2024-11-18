using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is used for storing NPC data for the pointing controller
public class NpcData
{
    public GameObject Npc { get; set; }
    public Vector3 InitialPosition { get; set; }
    public Quaternion InitialRotation { get; set; }

    public NpcData(GameObject npc, Vector3 initialPosition, Quaternion initialRotation)
    {
        Npc = npc;
        InitialPosition = initialPosition;
        InitialRotation = initialRotation;
    }
}
