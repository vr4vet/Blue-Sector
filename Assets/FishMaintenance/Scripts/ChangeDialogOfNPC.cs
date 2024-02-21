using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChangeDialogOfNPC : MonoBehaviour
{
    [SerializeField] private NPC newNpc;
    public NPCSpawner spawner;
    public void changeDialogue()
    {
        GameObject colleague = spawner.getNpc();
        colleague.SetActive(false);
        spawner.SpawnNPC(newNpc);
    }
}
