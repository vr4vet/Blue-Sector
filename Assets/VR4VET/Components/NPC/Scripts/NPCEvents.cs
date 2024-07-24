using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class NPCEvents : MonoBehaviour
{
    private GameObject npcIn;
    private DialogueBoxController dialogueBoxController;
    public UnityEvent OnDialogueEnd;

    void Start() 
    { 
        npcIn = gameObject.GetComponent<NPCSpawner>()._npcInstances[0];
        dialogueBoxController = npcIn.GetComponent<DialogueBoxController>();
        StartCoroutine(CheckForEnd());
    }

    IEnumerator CheckForEnd()
    {
        while (!dialogueBoxController.dialogueEnded)
        {
            yield return new WaitForSeconds(0.1f);
        }
        OnDialogueEnd.Invoke();
        yield return null;
    }
}
