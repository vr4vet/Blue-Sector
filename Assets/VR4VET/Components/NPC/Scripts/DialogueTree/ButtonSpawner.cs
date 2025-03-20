using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class ButtonSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _answerButtonContainer;
    [SerializeField] private GameObject _dialogueCanvas;
    private Rect _dialogueCanvasRect;
    [SerializeField] private GameObject _buttonPrefab;
    // max 4 buttons
    [HideInInspector] private GameObject[] _buttonsSpawned = new GameObject[4];
    [HideInInspector] private DialogueBoxController _dialogueBoxController;
    [HideInInspector] public static event Action<string> OnAnswer;

    private List<GameObject> _answerButtons = new();

    void Start() {
        _dialogueBoxController = GetComponent<DialogueBoxController>();
        if (_dialogueBoxController == null ) {
            Debug.LogError("The NPC missing the DialogueBoxController script");
        }
        
        _dialogueCanvasRect = _dialogueCanvas.GetComponent<RectTransform>().rect;


        foreach (Button child in _answerButtonContainer.GetComponentsInChildren<Button>())
        {
            if (child != _answerButtonContainer.transform)
                _answerButtons.Add(child.gameObject);
        }
    }

    // max 4 buttons
    private Vector3 getSpawnLocation(int numberOfButtons, int currentNumber) {
        switch (numberOfButtons)
        {
            case 1:
                return new Vector3(0,-38,1);
            case 2:
                return new Vector3(-24 + 48 * currentNumber,-38,0);
            case 3:
                return new Vector3(-46 + 46 * currentNumber,-38,1);
            case 4:
                return new Vector3(-58 + 39 * currentNumber,-38,1);
            default:
                Debug.LogError("You have too many buttons/answer options of the dialogue tree. I do not know how to place them");
                return new Vector3(0,0,0);
        }
    }
 
    // max 4 buttons
    void AddAnswerQuestionNumberListener() {
        foreach (GameObject answerButton in _answerButtons)
        {
            answerButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                OnAnswer?.Invoke(answerButton.GetComponentInChildren<TextMeshProUGUI>().text);
                _dialogueBoxController.AnswerQuestion(_answerButtons.IndexOf(answerButton));
            });
        }
    }

    public void spawnAnswerButtons(Answer[] answers) {
        for (int i = 0; i <  answers.Length; i++)
        {
            _answerButtons[i].SetActive(true);
            GameObject button = _answerButtons[i];

            // fill in the text
            button.GetComponentInChildren<TextMeshProUGUI>().text = answers[i].answerLabel;
        }

        // Add onclick listeners
        AddAnswerQuestionNumberListener();
    }

    /// <summary>
    /// Destroy the button gameobject and remove the button reference in the array
    /// </summary>
    public void removeAllButtons() {
        foreach(GameObject button in _answerButtons)
            button.SetActive(false);
    }

    // Add new "Speak" button to start conversation again
    public void spawnSpeakButton(DialogueTree dialogueTree) {
        removeAllButtons();
        _buttonsSpawned[0] = Instantiate(_buttonPrefab, _dialogueCanvas.transform);
        GameObject button = _buttonsSpawned[0];
        button.transform.localPosition = Vector3.zero;
        button.GetComponentInChildren<TextMeshProUGUI>().text = "Speak";

        //_dialogueCanvasRect.Set(_dialogueCanvasRect.x, _dialogueCanvasRect.y, 50, 30);
        _buttonsSpawned[0].GetComponent<Button>().onClick.AddListener(() => {_dialogueBoxController.StartDialogue(dialogueTree, 0, "NPC", 0);});
    }
}
