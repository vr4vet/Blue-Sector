using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCDissectionTask : MonoBehaviour
{
    [SerializeField] private FishDissectionGroup dissectionGroup;
    [Tooltip("The section in DissectionDialogue that incorrect answers lead to.")][SerializeField] private int incorrectOrganSection;

    private DialogueBoxController _dialogueBoxController;
    private ConversationController _conversationController;
    private int _questionSection, _questionIndex;

    public UnityEvent m_OnQuizLiver;
    public UnityEvent m_OnQuizKidney;
    public UnityEvent m_OnQuizHeart;
    public UnityEvent m_OnQuizSpleen;
    public UnityEvent m_OnQuizStomach;
    public UnityEvent m_OnQuizSwimBladder;
    public UnityEvent m_OnQuizIntestine;
    public UnityEvent m_OnQuizEnd;

    // Start is called before the first frame update
    void Start()
    {
        if (!(_dialogueBoxController = FindObjectOfType<DialogueBoxController>()))
            Debug.LogError("Could not find DialogueBoxController!");
        else
        {
            dissectionGroup.m_OnSalmonEntered.AddListener(OnSalmonPlacedOnCuttingBoard);
            dissectionGroup.m_OnFirstCutComplete.AddListener(OnFirstCutComplete);
            dissectionGroup.m_OnSecondCutComplete.AddListener(OnSecondCutComplete);
            dissectionGroup.m_OnThirdCutComplete.AddListener(OnThirdCutComplete);
            _dialogueBoxController.m_DialogueChanged.AddListener(OnDialogueChanged);
        }

        if (!(_conversationController = FindObjectOfType<ConversationController>()))
            Debug.LogError("Could not find ConversationController!");

        m_OnQuizLiver ??= new();
        m_OnQuizKidney ??= new();
        m_OnQuizHeart ??= new();
        m_OnQuizSpleen ??= new();
        m_OnQuizStomach ??= new();
        m_OnQuizSwimBladder ??= new();
        m_OnQuizIntestine ??= new();
        m_OnQuizEnd ??= new();
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

    private void OnSecondCutComplete()
    {
        if (_dialogueBoxController.dialogueTreeRestart != null && _dialogueBoxController.dialogueTreeRestart.name == "DissectionDialogue")
        {
            if (_dialogueBoxController._dialogueText.text == _dialogueBoxController.dialogueTreeRestart.sections[0].dialogue[2])
                _dialogueBoxController.SkipLine();
        }
    }

    private void OnThirdCutComplete()
    {
        if (_dialogueBoxController.dialogueTreeRestart != null && _dialogueBoxController.dialogueTreeRestart.name == "DissectionDialogue")
        {
            if (_dialogueBoxController._dialogueText.text == _dialogueBoxController.dialogueTreeRestart.sections[0].dialogue[3])
                _dialogueBoxController.SkipLine();
        }
    }

    private void OnDialogueChanged(string name, string dialogueTree, int section, int index)
    {
        if (!dialogueTree.ToLower().Contains("dissection")) // ensure correct dialogue tree before proceeding
            return;

        if (index == -1 && _dialogueBoxController._dialogueText.text.Equals("What is this organ called?"))  // index == -1 means question/branch point
        {                
            _questionSection = section;
            _questionIndex = index;

            foreach (Answer answer in _dialogueBoxController.dialogueTreeRestart.sections[section].branchPoint.answers)
            {
                if (answer.nextElement != incorrectOrganSection)
                {
                    switch (answer.answerLabel)
                    {
                        case "Liver":
                            m_OnQuizLiver.Invoke();
                            break;
                        case "Heart":
                            m_OnQuizHeart.Invoke();
                            break;
                        case "Kidney":
                            m_OnQuizKidney.Invoke();
                            break;
                        case "Spleen":
                            m_OnQuizSpleen.Invoke();
                            break;
                        case "Stomach":
                            m_OnQuizStomach.Invoke();
                            break;
                        case "Swim bladder":
                            m_OnQuizSwimBladder.Invoke();
                            break;
                        case "Intestine":
                            m_OnQuizIntestine.Invoke();
                            break;
                    }
                }
            }
        }
        else
        {
            switch (_dialogueBoxController._dialogueText.text)
            {
                case "Return to question":
                    _dialogueBoxController.StartDialogue(_dialogueBoxController.dialogueTreeRestart, _questionSection, name, _questionIndex);
                    break;
                case "Practice":
                    Debug.Log("Practice chosen");
                    break;
                case "Do another task":
                    _dialogueBoxController.StartDialogue(_conversationController.GetDialogueTrees()[0], 2, name, 0);
                    m_OnQuizEnd.Invoke();
                    break;
            }
        }
    }


    private void OnDestroy()
    {
        if (_dialogueBoxController)
        {
            dissectionGroup.m_OnSalmonEntered.RemoveListener(OnSalmonPlacedOnCuttingBoard);
            dissectionGroup.m_OnFirstCutComplete.RemoveListener(OnFirstCutComplete);
            dissectionGroup.m_OnSecondCutComplete.RemoveListener(OnSecondCutComplete);
            dissectionGroup.m_OnThirdCutComplete.RemoveListener(OnThirdCutComplete);
            _dialogueBoxController.m_DialogueChanged.RemoveListener(OnDialogueChanged);
        }
    }
}
