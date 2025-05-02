using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDissectionTask : MonoBehaviour
{
    [SerializeField] private FishDissectionGroup dissectionGroup;

    private DialogueBoxController _dialogueBoxController;

    // Start is called before the first frame update
    void Start()
    {
        if (!(_dialogueBoxController = FindObjectOfType<DialogueBoxController>()))
            Debug.LogError("Could not find DialogueBoxController!");
        else
        {
            dissectionGroup.m_OnSalmonEntered.AddListener(OnSalmonPlacedOnCuttingBoard);
            dissectionGroup.m_OnFirstCutComplete.AddListener(OnFirstCutComplete);
        }
    }

    private void OnSalmonPlacedOnCuttingBoard()
    {
        // skip to next line when salmon is placed on cutting board
        if (_dialogueBoxController.dialogueTreeRestart != null && _dialogueBoxController.dialogueTreeRestart.name == "DissectionDialogue")
        {
            if (_dialogueBoxController._dialogueText.text == _dialogueBoxController.dialogueTreeRestart.sections[0].dialogue[0])
                _dialogueBoxController.SkipLine();
        }
    }

    private void OnFirstCutComplete()
    {
        if (_dialogueBoxController.dialogueTreeRestart != null && _dialogueBoxController.dialogueTreeRestart.name == "DissectionDialogue")
        {
            if (_dialogueBoxController._dialogueText.text == _dialogueBoxController.dialogueTreeRestart.sections[0].dialogue[1])
                _dialogueBoxController.SkipLine();
        }
    }


    private void OnDestroy()
    {
        if (_dialogueBoxController)
            dissectionGroup.m_OnSalmonEntered.RemoveListener(OnSalmonPlacedOnCuttingBoard);
    }
}
