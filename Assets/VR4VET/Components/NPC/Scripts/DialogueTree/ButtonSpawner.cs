using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Button = UnityEngine.UI.Button;

public class ButtonSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _answerButtonContainer;
    [SerializeField] private GameObject _dialogueCanvas;
    [SerializeField] private GameObject _speakButton;
    [SerializeField] private GameObject _buttonPrefab;
    [HideInInspector] private DialogueBoxController _dialogueBoxController;
    [HideInInspector] public static event Action<string, string, string> OnAnswer;

    private List<GameObject> _answerButtons = new();
    private List<UnityAction> _answerButtonActions = new();
    private UnityAction _speakButtonAction = null;

    void Start() 
    {
        _dialogueBoxController = GetComponent<DialogueBoxController>();
        
        if (_dialogueBoxController == null )
            Debug.LogError("The NPC missing the DialogueBoxController script");

        foreach (Button child in _answerButtonContainer.GetComponentsInChildren<Button>())
        {
            if (child != _answerButtonContainer.transform)
                _answerButtons.Add(child.gameObject);
        }
    } 
    
    /// <summary>
    /// Add listeners to answer buttons
    /// </summary>
    void AddAnswerQuestionNumberListener() 
    {
        foreach (GameObject answerButton in _answerButtons)
        {
            // storing Unity Action in list before adding listener so it can later be removed
            UnityAction action = new(() =>
            {
                OnAnswer?.Invoke(answerButton.GetComponentInChildren<TextMeshProUGUI>().text, _dialogueBoxController._dialogueText.text, transform.name);
                _dialogueBoxController.AnswerQuestion(_answerButtons.IndexOf(answerButton));
            });
            _answerButtonActions.Add(action);

            answerButton.GetComponent<Button>().onClick.AddListener(action);
        }
    }

    /// <summary>
    /// Enabling an answer button for each provided answer from dialogue tree. 
    /// More than 4 buttons will result in bad looking formatting/layout, so an error is triggered instead.
    /// </summary>
    /// <param name="answers"></param>
    public void spawnAnswerButtons(Answer[] answers) 
    {
        if (answers.Length > 4)
        {
            Debug.LogError("There is not room for more than 4 answer buttons in the dialogue canvas, but there are " + answers.Length + " answers in the dialogue tree '" + _dialogueBoxController.dialogueTreeRestart.name + "'!");
            return;
        }

        for (int i = 0; i <  answers.Length; i++)
        {
            _answerButtons[i].SetActive(true);
            GameObject button = _answerButtons[i];

            // button sometimes won't highlight without setting to false first
            button.GetComponent<Button>().interactable = false;
            button.GetComponent<Button>().interactable = true;

            // fill in the text
            button.GetComponentInChildren<TextMeshProUGUI>().text = answers[i].answerLabel;
        }

        // Add onclick listeners
        AddAnswerQuestionNumberListener();
    }

    /// <summary>
    /// Deactivate button objects and remove listeners
    /// </summary>
    public void removeAllButtons() 
    {
        foreach(GameObject button in _answerButtons)
        {
            if (_answerButtons.IndexOf(button) <= _answerButtonActions.Count - 1)
            {
                UnityAction action = _answerButtonActions[_answerButtons.IndexOf(button)];
                button.GetComponent<Button>().onClick.RemoveListener(action);
            }

            button.SetActive(false);
        }
        _answerButtonActions.Clear();

        if (_speakButtonAction != null)
            _speakButton.GetComponent<Button>().onClick.RemoveListener(_speakButtonAction);
        
        _speakButton.SetActive(false);
    }

    // Add new "Speak" button to start conversation again
    public void spawnSpeakButton(DialogueTree dialogueTree) 
    {
        removeAllButtons();
        _speakButton.GetComponentInChildren<TextMeshProUGUI>().text = "Speak";

        UnityAction action = new(() => 
        { 
            _dialogueBoxController.StartDialogue(dialogueTree, 0, "NPC", 0); 
        });

        _speakButtonAction = action;
        _speakButton.GetComponent<Button>().onClick.AddListener(action);
        _speakButton.SetActive(true);
    }
}
