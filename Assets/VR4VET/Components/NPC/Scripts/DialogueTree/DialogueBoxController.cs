using System;
using System.Collections;
using TMPro;
using UnityEngine;
// Import of the TTS namespace
using Meta.WitAi.TTS.Utilities;
using UnityEngine.Events;
using UnityEngine.UI;

// This event will be called when the dialogue changes
[System.Serializable]
public class DialogueChanged : UnityEvent<string, string, int, int>
{
}

public class DialogueBoxController : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI _dialogueText;
    [SerializeField] private GameObject _dialogueBox;
    [SerializeField] private GameObject _answerBox;
    [SerializeField] private GameObject _dialogueCanvas;
    [SerializeField] public GameObject TTSSpeaker;

    [HideInInspector] public static event Action<string> OnDialogueStarted;
    [HideInInspector] public static event Action<string> OnDialogueEnded;
    [HideInInspector] private bool _skipLineTriggered;
    [HideInInspector] private bool _answerTriggered;
    [HideInInspector] private int _answerIndex;
    [SerializeField] private GameObject _skipLineButton;
    private Button _skipLineButtonComponent;
    [SerializeField] private GameObject _exitButton;
    [SerializeField] private GameObject _speakButton; 
    [HideInInspector] private Animator _animator;
    [HideInInspector] private int _isTalkingHash;
    [HideInInspector] private int _hasNewDialogueOptionsHash;
    [HideInInspector] private int _isPointingHash;
    [HideInInspector] public ButtonSpawner buttonSpawner;
    private Vector2 _oldDialogueCanvasSizeDelta;
    [HideInInspector] public bool dialogueIsActive;
    private int _activatedCount = 0;
    [HideInInspector]public DialogueTree dialogueTreeRestart;
    public bool dialogueEnded;
    public int timesEnded = 0;
    private PointingScript _pointingScript;
   
    public DialogueChanged m_DialogueChanged;

    private void Awake() 
    {
        buttonSpawner = GetComponent<ButtonSpawner>();
        if (buttonSpawner == null) 
        {
            Debug.LogError("The NPC missing the Button spawner script");
        }
        ResetBox();
        dialogueIsActive = false;

        // Animation stuff
        updateAnimator();
    }

    private void Start()
    {
        if(m_DialogueChanged == null)
        {
            m_DialogueChanged = new DialogueChanged();
        }

        _skipLineButtonComponent = _skipLineButton.GetComponent<Button>();
        _pointingScript = GetComponent<PointingScript>();
        dialogueEnded = false;
        // Assign the event camera
        if (_dialogueCanvas != null)
        {
            GameObject cameraCaster = GameObject.Find("CameraCaster");
            if (cameraCaster != null)
            {
                Camera eventCamera = cameraCaster.GetComponent<Camera>();
                if (eventCamera != null)
                {
                    _dialogueCanvas.GetComponent<Canvas>().worldCamera = eventCamera;
                }
                else
                {
                    Debug.LogError("CameraCaster does not have a Camera component!");
                }
            }
            else
            {
                Debug.LogError("CameraCaster GameObject not found in the scene!");
            }
        }
        else
        {
            Debug.LogError("DialogueCanvas not found or does not have a Canvas component!");
        }

        // Get the background transform for dimension changes
        _oldDialogueCanvasSizeDelta = _dialogueCanvas.GetComponent<RectTransform>().sizeDelta;
    }

    public void updateAnimator() {
        //this.animator = animator;
        this._animator = GetComponentInChildren<Animator>();
        _isTalkingHash = Animator.StringToHash("isTalking");
        _hasNewDialogueOptionsHash = Animator.StringToHash("hasNewDialogueOptions");
        _isPointingHash = Animator.StringToHash("isPointing");
    }

    public void updateAnimator(Animator animator) {
        this._animator = animator;
    }


    public void StartDialogue(DialogueTree dialogueTree, int startSection, string name, int element) 
    {
        dialogueIsActive = true;
        // stop I-have-something-to-tell-you-animation and start talking
        _animator.SetBool(_hasNewDialogueOptionsHash, false);
        //_animator.SetBool(_isTalkingHash, true);
        // Dialogue 
        ResetBox();
        _dialogueBox.SetActive(true);
        OnDialogueStarted?.Invoke(name);
        _activatedCount = 0;
        StartCoroutine(RunDialogue(dialogueTree, startSection, element));
        _exitButton.SetActive(true);

    }

    IEnumerator RunDialogue(DialogueTree dialogueTree, int section, int element)
    {
        // Make the "Speak" restart tree the current tree
        dialogueTreeRestart = dialogueTree;
        // Reset the dialogue box dimensions from "Speak" button dimensionsww
        _dialogueCanvas.GetComponent<RectTransform>().sizeDelta = _oldDialogueCanvasSizeDelta;
        
        //int dialogueSection = 0;
        
        // -1 means that the dialogue was a branchpoint and the script will skip to loading the branchpoint, instead of the standard dialogue when returning to the section
        if (element != -1)
        {
            for (int i = element; i < dialogueTree.sections[section].dialogue.Length; i++)
            {
                _pointingScript.ResetDirection(_animator.transform);
                _animator.SetBool(_isPointingHash, false);
                // Start talking animation
                _animator.SetBool(_isTalkingHash, true);
                StartCoroutine(revertToIdleAnimation());
                _dialogueText.text = dialogueTree.sections[section].dialogue[i];

                // button sometimes won't highlight without setting to false first
                _skipLineButtonComponent.interactable = false;
                _skipLineButtonComponent.interactable = true;

                _skipLineButton.transform.GetChild(0).GetComponent<Image>().color = _skipLineButtonComponent.colors.normalColor; // give arrow icon child same colour
                TTSSpeaker.GetComponent<TTSSpeaker>().Speak(_dialogueText.text);
                // Invoke the dialogue changed event
                m_DialogueChanged.Invoke(transform.name, dialogueTreeRestart.name, section, i);
                _skipLineButton.SetActive(true);


                // Check if the current section should have disabled the skip line button
                if (dialogueTree.sections[section].disabkeSkipLineButton)
                {
                    _skipLineButtonComponent.interactable = false;
                    _skipLineButton.transform.GetChild(0).GetComponent<Image>().color = _skipLineButtonComponent.colors.disabledColor; // give arrow icon child same colour
                }
              
                // Check if the current dialogue section should have the NPC pointing
                string pointAt = dialogueTree.sections[section].pointAt;
                if (pointAt != null && !pointAt.Equals(string.Empty))
                {
                    bool directionChanged = _pointingScript.ChangeDirection(dialogueTree.sections[section].pointAt, _animator.transform);
                    // Make sure the NPC is looking at the object before the pointing animation is started
                    if (directionChanged)
                    {
                        _animator.SetBool(_isTalkingHash, false);
                        _animator.SetBool(_isPointingHash, true);
                    }
                }

                while (!_skipLineTriggered)
                {
                    _exitButton.SetActive(true);
                    yield return null;
                }
                _skipLineTriggered = false;
                //dialogueSection = section;
            }   
        }

        string walkTurnDestination = dialogueTree.sections[section].walkOrTurnTowardsAfterDialogue;
        if (walkTurnDestination != null && !walkTurnDestination.Equals(string.Empty))
        {
            GetComponent<WalkingNpc>().WalkPath(dialogueTree.sections[section].walkOrTurnTowardsAfterDialogue);
        }
        
        if (dialogueTree.sections[section].endAfterDialogue)
        {
            dialogueEnded = true;
            timesEnded++;
            OnDialogueEnded?.Invoke(name);
            ExitConversation();
            yield break;
        }
        _dialogueText.text = dialogueTree.sections[section].branchPoint.question;
        TTSSpeaker.GetComponent<TTSSpeaker>().Speak(_dialogueText.text);
        _animator.SetBool(_isTalkingHash, true);
        StartCoroutine(revertToIdleAnimation());
        // Invoke the dialogue changed event
        m_DialogueChanged.Invoke(transform.name, dialogueTreeRestart.name, section, -1);
        ShowAnswers(dialogueTree.sections[section].branchPoint);
        while (_answerTriggered == false)
        {
            _skipLineButton.transform.GetChild(0).GetComponent<Image>().color = _skipLineButtonComponent.colors.disabledColor; // give arrow icon child same colour
            _skipLineButtonComponent.interactable = false;
            yield return null;
        }

        _skipLineButtonComponent.interactable = true;

        _skipLineButton.transform.GetChild(0).GetComponent<Image>().color = _skipLineButtonComponent.colors.normalColor; // give arrow icon child same colour
        _answerTriggered = false;
        //_exitButton.SetActive(false);
        //_skipLineButton.SetActive(false);

        walkTurnDestination = dialogueTree.sections[section].branchPoint.answers[_answerIndex].walkOrTurnTowardsAfterAnswer;
        if (walkTurnDestination != null && !walkTurnDestination.Equals(string.Empty))
        {
            GetComponent<WalkingNpc>().WalkPath(dialogueTree.sections[section].branchPoint.answers[_answerIndex].walkOrTurnTowardsAfterAnswer);
        }

        if (dialogueTree.sections[section].branchPoint.answers[_answerIndex].endAfterAnswer) {
            // Exit conversation if the answer is set to exit after answer
            dialogueEnded = true;
            timesEnded++;
            OnDialogueEnded?.Invoke(name);
            ExitConversation();
        } else {
            // Continue to section of the dialogue the answer points to
            StartCoroutine(RunDialogue(dialogueTree, dialogueTree.sections[section].branchPoint.answers[_answerIndex].nextElement, 0));
        }
    }

    public void StartComment(DialogueTree dialogueTree, int startSection, string name) {
        // Reset dialogue box if active
        dialogueIsActive = false;
        ResetBox();
        // Similar to startDialogue but don't activate the dialogue box
        dialogueIsActive = true;
        _animator.SetBool(_hasNewDialogueOptionsHash, false);
        OnDialogueStarted?.Invoke(name);
        RunComment(dialogueTree, startSection);
    }

    void RunComment(DialogueTree dialogueTree, int section) {
        // Runs the current section with no dialogue box, then exits
        _animator.SetBool(_isTalkingHash, true);
        StartCoroutine(ExitComment());
        _dialogueText.text = dialogueTree.sections[section].dialogue[0];
        TTSSpeaker.GetComponent<TTSSpeaker>().Speak(_dialogueText.text);
    }

    private IEnumerator ExitComment() {
        // When 9 seconds have passed, stop the animation and exit the comment dialogue
        yield return new WaitForSeconds(9.0f);
        _animator.SetBool(_isTalkingHash, false);
        dialogueIsActive = false;
    }

    public void ResetBox() 
    {
        StopAllCoroutines();
        _dialogueBox.SetActive(false);
        buttonSpawner.removeAllButtons();
        _skipLineTriggered = false;
        _answerTriggered = false;
        _skipLineButton.SetActive(false);
        _exitButton.SetActive(false); 
    }

    void ShowAnswers(BranchPoint branchPoint)
    {
        // Reveals the selectable answers and sets their text values
        buttonSpawner.spawnAnswerButtons(branchPoint.answers);

        _animator.SetBool(_isPointingHash, false);
        if (_pointingScript != null )
        {
            _pointingScript.ResetDirection(_animator.transform);
        }
    }

    public void SkipLine()
    {
        _skipLineTriggered = true;
    }

    public void AnswerQuestion(int answer)
    {
        _answerIndex = answer;
        _answerTriggered = true;
        _activatedCount++;
        // remove the buttons
        buttonSpawner.removeAllButtons();
    }

    // Reverts to idle animation after 10.267 seconds
    // Time is length of talking animation, should be tweaked to not use value
    private IEnumerator revertToIdleAnimation() {
        yield return new WaitForSeconds(9.0f);
        _animator.SetBool(_isTalkingHash, false);
    }

    public int GetActivatedCount()
    {
        return _activatedCount;
    }

    public void ExitConversation()
    {
        // stop talk-animation
        _animator.SetBool(_isTalkingHash, false);
        _animator.SetBool(_isPointingHash, false);
        if (_pointingScript != null)
        {
            _pointingScript.ResetDirection(_animator.transform);
        }
        dialogueIsActive = false;
        ResetBox();
        if (dialogueTreeRestart.speakButtonOnExit) {
            // Only start speak canvas if option is not turned off
            StartSpeakCanvas(dialogueTreeRestart);
        }
    }

    public void StartSpeakCanvas(DialogueTree dialogueTree)
    {
        _dialogueBox.SetActive(true);
        _dialogueText.text = null;
        _dialogueCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 30);
        buttonSpawner.spawnSpeakButton(dialogueTree);
    }
}