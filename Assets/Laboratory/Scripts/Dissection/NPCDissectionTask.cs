using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDissectionTask : MonoBehaviour
{
    [SerializeField] private FishDissectionGroup dissectionGroup;

    private DialogueBoxController _dialogueBoxController;
    private ConversationController _conversationController;
    private int _questionSection, _questionIndex;

    // Start is called before the first frame update
    void Start()
    {
        if (!(_dialogueBoxController = FindObjectOfType<DialogueBoxController>()))
            Debug.LogError("Could not find DialogueBoxController!");
        else
        {
            dissectionGroup.m_OnSalmonEntered.AddListener(OnSalmonPlacedOnCuttingBoard);
            dissectionGroup.m_OnFirstCutComplete.AddListener(OnFirstCutComplete);
            _dialogueBoxController.m_DialogueChanged.AddListener(OnDialogueChanged);
        }

        if (!(_conversationController = FindObjectOfType<ConversationController>()))
            Debug.LogError("Could not find ConversationController!");
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

    private void OnDialogueChanged(string name, string dialogueTree, int section, int index)
    {
        //Debug.Log("Name: " + name + "\nDialogue tree: " + dialogueTree + "\nSection: " + section + "\nIndex: " + index);

        if (dialogueTree.ToLower().Contains("dissection"))
        {
            if (index == -1)  // index == -1 means question/branch point
            {
                if (_dialogueBoxController._dialogueText.text.Equals("What is this organ called?"))
                {
                    _questionSection = section;
                    _questionIndex = index;
                    //Debug.Log(_questionSection);
                    //Debug.Log(_questionIndex);
                }
            }
            else
            {
                if (_dialogueBoxController._dialogueText.text.Equals("Return to question")) // return to failed question
                {
                    //Debug.Log("Returning to question " + _questionSection + " index " + _questionIndex);
                    _dialogueBoxController.StartDialogue(_dialogueBoxController.dialogueTreeRestart, _questionSection, name, _questionIndex);
                }
                else if (_dialogueBoxController._dialogueText.text.Equals("Practice"))
                {
                    Debug.Log("Practice chosen");
                }
                else if (_dialogueBoxController._dialogueText.text.Equals("Do another task"))
                {
                    _dialogueBoxController.StartDialogue(_conversationController.GetDialogueTrees()[0], 2, name, 0);
                }
            }
        }
    }


    private void OnDestroy()
    {
        if (_dialogueBoxController)
            dissectionGroup.m_OnSalmonEntered.RemoveListener(OnSalmonPlacedOnCuttingBoard);
    }
}
