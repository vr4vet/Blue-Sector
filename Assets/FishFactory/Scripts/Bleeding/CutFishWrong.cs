using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class CutFishWrong : MonoBehaviour
{

    private DialogueBoxController _dialogueBoxController;
    private ConversationController _conversationController;
    [SerializeField] private DialogueTree _errorTree;
    private bool _returnInvoked = false;

    private string _currentDialogue = "", _oldDialogue = "";
    private int _oldSection, _oldIndex, _currentSection, _currentIndex = 0;

    private void Update()
    {
        if (_currentDialogue == "CutFishWrong" && !_returnInvoked)
        {
            _returnInvoked = true;
            Invoke(nameof(ReturnToBleedingInstruction), 5.0f);
            
        }
    }

    void Start()
    {
        _dialogueBoxController = FindObjectOfType<DialogueBoxController>();
        _conversationController = FindObjectOfType<ConversationController>();
        _dialogueBoxController.m_DialogueChanged.AddListener(OnDialogueChanged);
    }

    public void CutNotStunnedFish()
    {
        if (_currentDialogue == "BleedingInstruction" || _currentDialogue == "CutFishWrong")// && _dialogueBoxController._dialogueText.text == _dialogueBoxController.dialogueTreeRestart.sections[4].dialogue[0])
        {
            _dialogueBoxController.StartDialogue(_errorTree, 0, "Bleeding station guide Bernard", 0);
        }
    }

    public void CutBadFish() 
    {
        if (_currentDialogue == "BleedingInstruction" || _currentDialogue == "CutFishWrong")
        {
            _dialogueBoxController.StartDialogue(_errorTree, 1, "Bleeding station guide Bernard", 0); 
        }
    }

    public void ForgotToCutFish()
    {
        Debug.Log(_currentDialogue);
        if (_currentDialogue == "BleedingInstruction" || _currentDialogue == "CutFishWrong")
        {
            _dialogueBoxController.StartDialogue(_errorTree, 2, "Bleeding station guide Bernard", 0);
        }
    } 
    
    public void MetalInFish()
    {
        if (_currentDialogue == "BleedingInstruction" || _currentDialogue == "CutFishWrong")
        {
            _dialogueBoxController.StartDialogue(_errorTree, 3, "Bleeding station guide Bernard", 0);
        }
    }

    private void ReturnToBleedingInstruction()
    {
        _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("BleedingInstruction"), 4, "Bleeding station guide Bernard", -1);
        _returnInvoked = false;
    }

    private void OnDialogueChanged(string npcName, string dialogueTree, int section, int index)
    {
        // The variables for the old dialogue are set before the new dialogue is set, making it represent the previous duologue.
        if (dialogueTree != "CutFishWrong" || _currentDialogue != "CutFishWrong")
        {
           _oldDialogue = _currentDialogue;
           _oldSection = _currentSection;
           _oldIndex = _currentIndex;
           _returnInvoked = false;
        }
        
        // The variables store information about the current dialogue.
        _currentDialogue = dialogueTree;
        _currentSection = section;
        _currentIndex = index;
    }

    private DialogueTree GetDialogueTreeFromName(string name)
    {
        DialogueTree returnTree = null;

        foreach (DialogueTree tree in _conversationController.GetDialogueTrees())
        {
            if (tree.name.Equals(name))
                returnTree = tree;
        }

        if (returnTree != null)
            return returnTree;
        return null;
    }
}
