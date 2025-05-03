using System;
using System.Collections;
using System.Collections.Generic;
using Task;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class NPCEvents : MonoBehaviour
{
    private GameObject npcIn;
    private DialogueBoxController dialogueBoxController;
    public Step stepToHaveBeenCompleted;
    public UnityEvent OnDialogueEnd;
    public UnityEvent OnCompletedStep;

    void Start() 
    { 
        npcIn = gameObject.GetComponent<NPCSpawner>().NpcInstances[0];
        dialogueBoxController = npcIn.GetComponent<DialogueBoxController>();
        StartCoroutine(CheckForEnd());
    }

    IEnumerator CheckForEnd()
    {
        while (!dialogueBoxController.DialogueEnded)
        {
            yield return new WaitForSeconds(0.1f);
        }
        OnDialogueEnd.Invoke();
        if (stepToHaveBeenCompleted)
        {
            if (stepToHaveBeenCompleted.IsCompeleted())
            {
                OnCompletedStep.Invoke();
            }
        }
        yield return null;
    }
}
