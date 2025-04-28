using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConversationController : MonoBehaviour
{
    // --- Existing References & Config ---
    [SerializeField] private DialogueTree _toBeOverwrittenByJSON;
    [SerializeField] private List<TextAsset> _dialogueTreesJSONFormat = new List<TextAsset>();
    [SerializeField] private List<DialogueTree> _dialogueTreesSOFormat = new List<DialogueTree>();
    [HideInInspector] private DialogueTree _dialogueTree;
    [HideInInspector] private DialogueTree _oldDialogueTree;
    [HideInInspector] private int _currentElement = 0; // Tracks current DialogueTree in the list

    // --- Component References ---
    [HideInInspector] private Animator _animator; // Found on parent's child model
    [HideInInspector] private DialogueBoxController _dialogueBoxController; // Found on parent
    [HideInInspector] public AIConversationController _AIConversationController; // Found on parent (Set by NPCSpawner)

    // --- Input System ---
    [Header("Input Settings")]
    [Tooltip("Assign in Inspector to VoiceRecord action")]
    [SerializeField] private InputActionReference voiceRecordAction;

    // --- State ---
    [HideInInspector] public bool playerInsideTrigger = false;
    [HideInInspector] public bool isRecording = false; // Track active recording state

    // --- Animation Hashes ---
    [HideInInspector] private int _hasNewDialogueOptionsHash;

    void OnEnable()
    {
        if (voiceRecordAction != null && voiceRecordAction.action != null)
        {
            voiceRecordAction.action.Enable();
            voiceRecordAction.action.started += OnVoiceRecordPerformed;  // Use started for immediate response
            voiceRecordAction.action.canceled += OnVoiceRecordCanceled;
        }
    }

    void OnDisable()
    {
        if (voiceRecordAction != null && voiceRecordAction.action != null)
        {
            voiceRecordAction.action.Disable();
            voiceRecordAction.action.started -= OnVoiceRecordPerformed;  // Match with started
            voiceRecordAction.action.canceled -= OnVoiceRecordCanceled;
        }

        // Ensure we stop recording if component is disabled while recording
        if (isRecording)
        {
            HandleRecordButton(false);
        }
    }

    // Called when the button is pressed down
    private void OnVoiceRecordPerformed(InputAction.CallbackContext context)
    {
        // Only handle input if this NPC is the one the player is interacting with
        if (playerInsideTrigger && CanStartRecording())
        {
            Debug.Log($"VoiceRecord button pressed down on {transform.parent?.name}");
            HandleRecordButton(true);
        }
    }

    // Called when the button is released
    private void OnVoiceRecordCanceled(InputAction.CallbackContext context)
    {
        // Check if we're the active recorder to avoid stopping recordings that weren't started by us
        if (isRecording)
        {
            Debug.Log($"VoiceRecord button released on {transform.parent?.name}");
            HandleRecordButton(false);
        }
    }

    void Start()
    {
        // Get reference to parent's DialogueBoxController
        _dialogueBoxController = GetComponentInParent<DialogueBoxController>();
        if (_dialogueBoxController == null)
        {
            Debug.LogError("ConversationController: Parent DialogueBoxController script not found!", this);
        }

        // AIConversationController reference is set by NPCSpawner if it's an AI NPC

        // Animator reference setup
        updateAnimator(); // Find animator initially
        if (_animator != null)
        {
            _hasNewDialogueOptionsHash = Animator.StringToHash("hasNewDialogueOptions");
        }

        // Dialogue Tree setup
        JoinWithScriptableObjectList(_dialogueTreesJSONFormat);
        if (_dialogueTreesSOFormat.Count > 0)
        {
            _dialogueTree = _dialogueTreesSOFormat.ElementAt(_currentElement);
        }
        else
        {
            // Check if this is an AI NPC (chatbot) which might not need dialogue trees
            if (_AIConversationController != null)
            {
                // For AI NPCs like chatbot, no dialogue trees is a valid configuration
                Debug.Log($"ConversationController on {transform.parent?.name}: AI NPC with no dialogue trees. Will use AI conversation only.");
            }
            else
            {
                Debug.LogWarning($"ConversationController on {transform.parent?.name}: No dialogue trees assigned.", this);
            }
        }
    }

    public void updateAnimator()
    {
        // Animator is expected on a child of the parent (e.g., the model GameObject)
        Transform parentTransform = this.transform.parent;
        if (parentTransform != null)
        {
            _animator = parentTransform.GetComponentInChildren<Animator>();
            if (_animator == null)
            {
                Debug.LogError($"ConversationController on {parentTransform.name}: Animator not found in parent's children.", this);
            }
        }
        else
        {
            Debug.LogError("ConversationController: Cannot find parent transform!", this);
        }
    }

    // Called by SetCharacterModel when animator changes
    public void updateAnimator(Animator animator)
    {
        this._animator = animator;
        if (this._animator != null)
        {
            _hasNewDialogueOptionsHash = Animator.StringToHash("hasNewDialogueOptions");
        }
        else
        {
            Debug.LogWarning("ConversationController: updateAnimator called with null Animator.", this);
        }
    }

    public bool isDialogueActive()
    {
        return _dialogueBoxController != null && _dialogueBoxController.dialogueIsActive;
    }

    // Method for external systems (like VR Input) to check if interaction is possible
    public bool CanStartRecording()
    {
        return playerInsideTrigger &&
               _dialogueBoxController != null &&
               _dialogueBoxController.isTalkable && // Check talkable state from DialogueBoxController
               !_dialogueBoxController.dialogueEnded;
    }

    // --- Public method for VR Input System to call ---
    public void HandleRecordButton(bool pressed)
    {
        if (_AIConversationController == null)
        {
            Debug.Log("HandleRecordButton called, but this is not an AI NPC.");
            return; // Ignore if not an AI NPC
        }

        if (pressed) // Button Press Down
        {
            if (CanStartRecording() && !isRecording) // Only start if not already recording
            {
                Debug.Log($"ConversationController ({transform.parent.name}): Record button pressed & valid state. Starting recording.");
                _AIConversationController.TriggerRecording(true);
                isRecording = true;
            }
            else
            {
                Debug.Log($"ConversationController ({transform.parent.name}): Record button pressed but cannot start recording (Inside: {playerInsideTrigger}, Talkable: {_dialogueBoxController?.isTalkable}, Ended: {_dialogueBoxController?.dialogueEnded}).");
            }
        }
        else // Button Release
        {
            if (isRecording && _AIConversationController.GetTranscribe() != null)
            {
                Debug.Log($"ConversationController ({transform.parent.name}): Record button released. Stopping recording.");
                _AIConversationController.TriggerRecording(false);
                isRecording = false;
            }
        }
    }

    // --- Trigger Events for Proximity ---

    void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player (using the singleton reference)
        if (NPCToPlayerReferenceManager.Instance != null && other.Equals(NPCToPlayerReferenceManager.Instance.PlayerCollider))
        {
            playerInsideTrigger = true;
            Debug.Log($"ConversationController ({transform.parent.name}): Player entered trigger range.");

            // Show the dialogue UI canvas (DialogueBoxController handles actual content visibility)
            if (_dialogueBoxController != null) _dialogueBoxController.ShowDialogueBox();

            // Start dialogue automatically ONLY if proximity trigger is enabled, not already active, and it's a new tree
            if (_dialogueTree != null && _dialogueTree.shouldTriggerOnProximity && !isDialogueActive() && _oldDialogueTree != _dialogueTree)
            {
                Debug.Log($"ConversationController ({transform.parent.name}): Proximity trigger starting dialogue tree '{_dialogueTree.name}'.");
                _oldDialogueTree = _dialogueTree; // Mark as seen
                if (_dialogueBoxController != null)
                {
                    // Start dialogue using the parent's name
                    _dialogueBoxController.StartDialogue(_dialogueTree, 0, transform.parent.name);
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (NPCToPlayerReferenceManager.Instance != null && other.Equals(NPCToPlayerReferenceManager.Instance.PlayerCollider))
        {
            playerInsideTrigger = false;
            Debug.Log($"ConversationController ({transform.parent.name}): Player exited trigger range.");

            // Hide the dialogue canvas if dialogue is not active
            if (_dialogueBoxController != null) _dialogueBoxController.HideDialogueBox();

            // If the player leaves while recording, stop recording
            if (isRecording)
            {
                HandleRecordButton(false); // Trigger stop
            }
        }
    }

    // --- Dialogue Tree Management Methods ---

    public int GetActivatedCount()
    {
        return _dialogueBoxController != null ? _dialogueBoxController.GetActivatedCount() : 0;
    }

    public void DialogueTrigger()
    {
        if (_dialogueTree != null && _oldDialogueTree != _dialogueTree)
        {
            _oldDialogueTree = _dialogueTree;
            if (_dialogueBoxController != null)
            {
                _dialogueBoxController.StartDialogue(_dialogueTree, 0, transform.parent.name);
            }
            else { Debug.LogError("DialogueBoxController is null in DialogueTrigger.", this); }
        }
        else
        {
            if (_dialogueTree == null) Debug.LogError("Cannot trigger dialogue, _dialogueTree is null.", this);
            // else Debug.Log("DialogueTrigger called, but tree hasn't changed or is null.");
        }
    }

    public void DialogueTriggerAbsolute()
    {
        if (_dialogueTree != null)
        {
            if (_dialogueBoxController != null)
            {
                _dialogueBoxController.StartDialogue(_dialogueTree, 0, transform.parent.name);
            }
            else { Debug.LogError("DialogueBoxController is null in DialogueTriggerAbsolute.", this); }
        }
        else
        {
            Debug.LogError("Cannot trigger dialogue, _dialogueTree is null.", this);
        }
    }

    public void CommentTrigger(int section = 0)
    {
        if (_dialogueTree != null)
        {
            if (_dialogueBoxController != null)
            {
                // Comments likely don't need AI? Use standard StartComment if it exists
                // _dialogueBoxController.StartComment(_dialogueTree, section, transform.parent.name);
                Debug.LogWarning("CommentTrigger called, but StartComment needs verification/implementation in the merged DialogueBoxController.", this);
            }
            else { Debug.LogError("DialogueBoxController is null in CommentTrigger.", this); }
        }
        else
        {
            Debug.LogError("Cannot trigger comment, _dialogueTree is null.", this);
        }
    }

    private void JoinWithScriptableObjectList(List<TextAsset> jsonList)
    {
        if (jsonList == null) return;
        foreach (var dialogueJSON in jsonList)
        {
            if (dialogueJSON != null && _toBeOverwrittenByJSON != null)
            {
                try
                {
                    DialogueTree temp = Instantiate(_toBeOverwrittenByJSON);
                    JsonUtility.FromJsonOverwrite(dialogueJSON.text, temp);
                    temp.name = dialogueJSON.name; // Give it a name based on the TextAsset
                    _dialogueTreesSOFormat.Add(temp);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to load DialogueTree from JSON '{dialogueJSON.name}'. Error: {e.Message}", this);
                }
            }
        }
    }

    private List<DialogueTree> ConvertFromJSONListToDialogueTreeList(List<TextAsset> jsonList)
    {
        List<DialogueTree> treeList = new List<DialogueTree>();
        if (jsonList == null) return treeList;

        foreach (var dialogueJSON in jsonList)
        {
            if (dialogueJSON != null && _toBeOverwrittenByJSON != null)
            {
                try
                {
                    DialogueTree temp = Instantiate(_toBeOverwrittenByJSON);
                    JsonUtility.FromJsonOverwrite(dialogueJSON.text, temp);
                    temp.name = dialogueJSON.name;
                    treeList.Add(temp);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to convert DialogueTree from JSON '{dialogueJSON.name}'. Error: {e.Message}", this);
                }
            }
        }
        return treeList;
    }

    public void SetDialogueTreeList(List<DialogueTree> dialogueTrees)
    {
        if (_dialogueBoxController != null) _dialogueBoxController.ExitConversation(); // End current conversation safely
        this._dialogueTreesSOFormat = dialogueTrees ?? new List<DialogueTree>(); // Ensure list is not null
        _currentElement = 0;
        _dialogueTree = _dialogueTreesSOFormat.Count > 0 ? _dialogueTreesSOFormat[0] : null;
        _oldDialogueTree = null; // Reset seen tree
        SignalNewDialogue();
    }

    public void SetDialogueTreeList(DialogueTree dialogueTree)
    {
        SetDialogueTreeList(new List<DialogueTree> { dialogueTree });
    }

    public void SetDialogueTreeList(List<TextAsset> dialogueTreesJSON)
    {
        List<DialogueTree> convertedTrees = ConvertFromJSONListToDialogueTreeList(dialogueTreesJSON);
        SetDialogueTreeList(convertedTrees);
    }

    public void SetDialogueTreeList(TextAsset dialogueTreeJSON)
    {
        SetDialogueTreeList(new List<TextAsset> { dialogueTreeJSON });
    }

    // Combined setter used by NPCSpawner
    public void SetDialogueTreeList(DialogueTree[] dialogueTreesSO, TextAsset[] dialogueTreesJSON)
    {
        if (_dialogueBoxController != null) _dialogueBoxController.ExitConversation();
        _dialogueTreesSOFormat = new List<DialogueTree>(dialogueTreesSO ?? Array.Empty<DialogueTree>());
        JoinWithScriptableObjectList(new List<TextAsset>(dialogueTreesJSON ?? Array.Empty<TextAsset>()));
        _currentElement = 0;
        _dialogueTree = _dialogueTreesSOFormat.Count > 0 ? _dialogueTreesSOFormat[0] : null;
        _oldDialogueTree = null; // Reset seen tree
        SignalNewDialogue();
    }

    public void InsertDialogueTreeAndChange(DialogueTree dialogueTree)
    {
        if (dialogueTree != null && !_dialogueTreesSOFormat.Contains(dialogueTree))
        {
            if (_dialogueBoxController != null) _dialogueBoxController.ExitConversation();
            _currentElement++; // Insert after current
            _dialogueTreesSOFormat.Insert(Mathf.Clamp(_currentElement, 0, _dialogueTreesSOFormat.Count), dialogueTree);
            this._dialogueTree = dialogueTree; // Switch to the new tree
            _oldDialogueTree = null; // Allow immediate trigger if needed
            SignalNewDialogue();
        }
    }

    public void InsertDialogueTreeAndChange(TextAsset dialogueTreeJSON)
    {
        if (dialogueTreeJSON != null && _toBeOverwrittenByJSON != null)
        {
            try
            {
                DialogueTree temp = Instantiate(_toBeOverwrittenByJSON);
                JsonUtility.FromJsonOverwrite(dialogueTreeJSON.text, temp);
                temp.name = dialogueTreeJSON.name;
                InsertDialogueTreeAndChange(temp);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to insert DialogueTree from JSON '{dialogueTreeJSON.name}'. Error: {e.Message}", this);
            }
        }
    }

    public void NextDialogueTree()
    {
        if (_currentElement + 1 < _dialogueTreesSOFormat.Count)
        {
            _currentElement++;
            _dialogueTree = _dialogueTreesSOFormat[_currentElement];
            _oldDialogueTree = null; // Allow immediate trigger if needed
            SignalNewDialogue();
            if (_dialogueBoxController != null) _dialogueBoxController.StartSpeakCanvas(_dialogueTree); // Show speak button for new tree
        }
        else
        {
            Debug.Log("Already at the last dialogue tree.");
        }
    }

    public void PreviousDialogueTree()
    {
        if (_currentElement - 1 >= 0)
        {
            _currentElement--;
            _dialogueTree = _dialogueTreesSOFormat[_currentElement];
            _oldDialogueTree = null; // Allow immediate trigger if needed
            SignalNewDialogue();
            if (_dialogueBoxController != null) _dialogueBoxController.StartSpeakCanvas(_dialogueTree);
        }
        else
        {
            Debug.Log("Already at the first dialogue tree.");
        }
    }

    // Start a specific dialogue tree by name
    public void StartDialogueTree(string dialogueTreeName)
    {
        DialogueTree foundTree = _dialogueTreesSOFormat.Find(tree => tree != null && tree.name == dialogueTreeName);
        if (foundTree != null)
        {
            int foundIndex = _dialogueTreesSOFormat.IndexOf(foundTree);
            if (foundIndex >= 0)
            {
                _currentElement = foundIndex;
                _dialogueTree = foundTree;
                _oldDialogueTree = null; // Allow immediate trigger if needed
                SignalNewDialogue();
                if (_dialogueBoxController != null) _dialogueBoxController.StartSpeakCanvas(_dialogueTree); // Show speak button
                Debug.Log($"Switched to dialogue tree: {dialogueTreeName}");
            }
        }
        else
        {
            Debug.LogError($"DialogueTree named '{dialogueTreeName}' not found in the list for {transform.parent?.name}.", this);
        }
    }

    public DialogueTree GetDialogueTree()
    {
        return _dialogueTree;
    }

    public List<DialogueTree> GetDialogueTrees()
    {
        return _dialogueTreesSOFormat;
    }

    private void SignalNewDialogue()
    {
        if (_animator != null)
        {
            _animator.SetBool(_hasNewDialogueOptionsHash, true);
        }
    }

#if UNITY_EDITOR
    // Editor-only test function
    public void TestVoiceRecordingConnection()
    {
        Debug.Log("Testing voice recording connection:");
        Debug.Log($"- Input Action Reference: {(voiceRecordAction != null ? "Assigned" : "Missing")}");
        Debug.Log($"- AI Controller: {(_AIConversationController != null ? "Connected" : "Missing")}");
        Debug.Log($"- DialogueBox Controller: {(_dialogueBoxController != null ? "Connected" : "Missing")}");
        Debug.Log($"- Transcribe Component: {(_AIConversationController != null && _AIConversationController.GetTranscribe() != null ? "Available" : "Missing")}");
        Debug.Log($"- Player Inside Trigger: {playerInsideTrigger}");
        Debug.Log($"- Talkable State: {(_dialogueBoxController != null ? _dialogueBoxController.isTalkable.ToString() : "N/A")}");
        Debug.Log($"- Dialogue Ended: {(_dialogueBoxController != null ? _dialogueBoxController.dialogueEnded.ToString() : "N/A")}");
    }
#endif
}
