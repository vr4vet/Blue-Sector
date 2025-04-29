using UnityEngine;
using Meta.WitAi.TTS.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private NPC[] _nPCs;
    [HideInInspector] public List<GameObject> _npcInstances = new List<GameObject>(); // Initialize list

    [Header("Global AI Settings")]
    [TextArea(3, 10)]
    public string globalContextPrompt = "You are interacting with a user in a virtual reality training simulation."; // Added global context
    private ActionManager _actionManager;
    private void Awake()
    {
        _actionManager = ActionManager.Instance;

        if (_nPCs == null || _nPCs.Length == 0)
        {
            Debug.LogWarning("NPCSpawner: No NPC Scriptable Objects assigned.", this);
            return;
        }

        foreach (var npcSO in _nPCs)
        {
            if (npcSO == null)
            {
                Debug.LogWarning("NPCSpawner: Found a null NPC Scriptable Object in the list.", this);
                continue;
            }
            if (npcSO.NpcPrefab == null)
            {
                Debug.LogError($"NPCSpawner: NpcPrefab is null for NPC SO '{npcSO.name}'. Skipping.", this);
                continue;
            }
            _npcInstances.Add(SpawnNPC(npcSO));

        }

        // Start the coroutine to fix animator references after all NPCs are spawned
        StartCoroutine(FixAnimators());
        
    }

    private IEnumerator FixAnimators()
    {
        // Wait for NPCs to be spawned fully
        yield return new WaitForSeconds(0.5f);

        // Check if NPCs were spawned
        if (_npcInstances == null || _npcInstances.Count == 0)
        {
            Debug.LogWarning("NPCSpawner: No NPCs were spawned to fix animators");
            yield break;
        }

        // Loop through all spawned NPCs
        foreach (var npc in _npcInstances)
        {
            if (npc == null) continue;

            // Find the animator in the NPC's children
            var animator = npc.GetComponentInChildren<Animator>();
            if (animator == null)
            {
                Debug.LogWarning($"NPCSpawner: No Animator found in children of NPC {npc.name}");
                continue;
            }

            // Update animator references in controllers
            var dialogueBoxController = npc.GetComponent<DialogueBoxController>();
            if (dialogueBoxController != null)
            {
                dialogueBoxController.updateAnimator(animator);

                // Reset all animation parameters to ensure NPC is in idle state
                try
                {
                    animator.SetBool("isTalking", false);
                    animator.SetBool("hasNewDialogueOptions", false);
                    animator.SetBool("isPointing", false);
                    animator.SetBool("isListening", false);

                    // Force update the animator to apply the changes immediately
                    animator.Update(0);
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"Error resetting animation parameters: {e.Message}");
                }
            }

            // Also update any ConversationController components
            var conversationController = npc.GetComponentInChildren<ConversationController>();
            if (conversationController != null)
            {
                conversationController.updateAnimator(animator);
            }
        }
    }

    public GameObject SpawnNPC(NPC npcSO)
    {
        // Instantiate the NPC prefab at the defined location
        GameObject newNPC = Instantiate(npcSO.NpcPrefab, npcSO.SpawnPosition, Quaternion.identity);
        newNPC.name = npcSO.NameOfNPC; // Set GameObject name for easier identification


        // Rotate the NPC
        newNPC.transform.rotation = Quaternion.Euler(npcSO.SpawnRotation);

        // --- Standard Setup ---
        AttachTTSComponents(newNPC, npcSO.SpatialBlend, npcSO.MinDistance); // Attaches Wit TTSSpeaker
        SetAppearanceAnimationAndVoice(newNPC, npcSO.CharacterModel, npcSO.CharacterAvatar, npcSO.runtimeAnimatorController, npcSO.VoicePresetId); // Sets model & standard Wit voice

        // Make sure animator reference is updated after model is loaded
        var dialogueBoxController = newNPC.GetComponent<DialogueBoxController>();
        if (dialogueBoxController != null)
        {
            dialogueBoxController.updateAnimator();
        }

        SetFollowingBehavior(newNPC, npcSO.ShouldFollow);
        SetName(newNPC, npcSO.NameOfNPC);
        SetConversation(newNPC, npcSO.DialogueTreesSO, npcSO.DialogueTreeJSON);

        if (npcSO.WithoutDialogue)
        {
            // Disable dialogue triggering
            var conversationController = newNPC.GetComponentInChildren<ConversationController>();
            if (conversationController != null)
            {
                Collider triggerCollider = conversationController.GetComponent<Collider>();
                if (triggerCollider != null) triggerCollider.enabled = false;
            }
            else
            {
                Debug.LogWarning($"NPCSpawner: Could not find ConversationController on {newNPC.name} to disable dialogue trigger.", newNPC);
            }
        }

        // --- AI Setup ---
        if (npcSO.isAiNpc && _actionManager._isAiNpcToggled)
        {
            Debug.Log($"NPCSpawner: Setting up AI components for {newNPC.name}");

            // Ensure AI components exist on the root GameObject
            var aiConvCtrl = newNPC.GetComponent<AIConversationController>();
            if (aiConvCtrl == null) aiConvCtrl = newNPC.AddComponent<AIConversationController>();

            var aiResponse = newNPC.GetComponent<AIResponseToSpeech>();
            if (aiResponse == null) aiResponse = newNPC.AddComponent<AIResponseToSpeech>();

            var audioSrc = newNPC.GetComponent<AudioSource>();
            if (audioSrc == null) audioSrc = newNPC.AddComponent<AudioSource>();

            // Configure AI components
            SetAIBehaviour(newNPC, npcSO.contextPrompt, npcSO.maxTokens, aiConvCtrl);

            // Link components (DialogueBoxController needs references to AI components)
            if (dialogueBoxController != null)
            {
                dialogueBoxController._AIConversationController = aiConvCtrl;
                dialogueBoxController._AIResponseToSpeech = aiResponse;
            }
            else
            {
                Debug.LogError($"NPCSpawner: DialogueBoxController missing on AI NPC {newNPC.name}. AI features might not work correctly.", newNPC);
            }

            // Link AIController to ConversationController on child for proximity/input handling
            var conversationCtrl = newNPC.GetComponentInChildren<ConversationController>();
            if (conversationCtrl != null)
            {
                conversationCtrl._AIConversationController = aiConvCtrl;
            }
            else
            {
                Debug.LogError($"NPCSpawner: ConversationController missing on child of AI NPC {newNPC.name}. AI input might not work.", newNPC);
            }
        }

        return newNPC;
    }

    // Method to attach standard Wit TTS components
    public void AttachTTSComponents(GameObject npc, float spatialBlend, float minDistance)
    {
        GameObject ttsPrefab = Resources.Load<GameObject>("TTS"); // Assuming TTS prefab is in Resources
        if (ttsPrefab != null)
        {
            GameObject ttsObject = Instantiate(ttsPrefab, npc.transform);
            ttsObject.name = "TTS_Wit"; // Give it a specific name

            TTSSpeaker ttsSpeaker = ttsObject.GetComponentInChildren<TTSSpeaker>();
            DialogueBoxController dialogueController = npc.GetComponent<DialogueBoxController>();

            if (ttsSpeaker != null && dialogueController != null)
            {
                dialogueController.TTSSpeaker = ttsSpeaker.gameObject;

                AudioSource speakerAudio = ttsSpeaker.GetComponentInChildren<AudioSource>();
                if (speakerAudio != null)
                {
                    speakerAudio.spatialBlend = Mathf.Clamp(spatialBlend, 0, 1); // Default to 3D audio
                    speakerAudio.minDistance = Mathf.Max(minDistance, 1); // Default min distance
                }
                else
                {
                    Debug.LogWarning($"NPCSpawner: AudioSource not found on TTSSpeaker child for {npc.name}.", ttsSpeaker);
                }
            }
            else
            {
                if (ttsSpeaker == null) Debug.LogError("NPCSpawner: TTSSpeaker component not found in TTS prefab children.", ttsPrefab);
                if (dialogueController == null) Debug.LogError($"NPCSpawner: DialogueBoxController not found on NPC {npc.name}.", npc);
            }
        }
        else
        {
            Debug.LogError("NPCSpawner: TTS prefab could not be loaded from Resources folder. Ensure it exists and is named 'TTS'.");
        }
    }

    // Method to set appearance and *standard* Wit voice
    public void SetAppearanceAnimationAndVoice(GameObject npc, GameObject characterModelPrefab, Avatar characterAvatar, RuntimeAnimatorController runtimeAnimatorController, int voicePresetId)
    {
        SetCharacterModel setCharacterModel = npc.GetComponent<SetCharacterModel>();
        if (setCharacterModel == null)
        {
            Debug.LogError($"NPCSpawner: The NPC {npc.name} is missing the script SetCharacterModel", npc);
        }
        else
        {
            setCharacterModel.ChangeCharacter(characterModelPrefab, characterAvatar, runtimeAnimatorController, voicePresetId);
        }
    }

    // Method to set standard following behavior
    public void SetFollowingBehavior(GameObject npc, bool shouldFollow)
    {
        FollowThePlayerController followController = npc.GetComponent<FollowThePlayerController>();
        if (followController != null)
        {
            followController.ShouldFollow = shouldFollow;
        }
        else
        {
            Debug.LogWarning($"NPCSpawner: NPC {npc.name} does not have a FollowThePlayerController component!", npc);
        }
    }

    // Method to set display name
    public void SetName(GameObject npc, String name)
    {
        DisplayName displayName = npc.GetComponent<DisplayName>();
        if (displayName == null)
        {
            Debug.LogError($"NPCSpawner: The NPC {npc.name} is missing the DisplayName component", npc);
        }
        else
        {
            displayName.UpdateDisplayedName(name);
        }
    }

    // Method to set dialogue trees
    public void SetConversation(GameObject npc, DialogueTree[] dialogueTreesSO, TextAsset[] dialogueTreesJSON)
    {
        ConversationController conversationController = npc.GetComponentInChildren<ConversationController>();
        if (conversationController == null)
        {
            Debug.LogError($"NPCSpawner: The NPC {npc.name} is missing the ConversationController in its children", npc);
        }
        else
        {
            conversationController.SetDialogueTreeList(dialogueTreesSO, dialogueTreesJSON);
        }
    }

    // Sets context and max tokens for the AI
    public void SetAIBehaviour(GameObject npc, string contextPrompt, int maxTokens, AIConversationController aiConvCtrl)
    {
        if (aiConvCtrl != null)
        {
            Debug.Log($"NPCSpawner: Setting AI behaviour for {npc.name}. MaxTokens: {maxTokens}");
            aiConvCtrl.contextPrompt = contextPrompt;
            aiConvCtrl.maxTokens = Mathf.Max(maxTokens, 1); // Ensure maxTokens is at least 1

            // Clear any existing messages and add system prompts
            aiConvCtrl.messages.Clear();
            if (!string.IsNullOrWhiteSpace(globalContextPrompt))
            {
                aiConvCtrl.AddMessage(new Message { role = "system", content = globalContextPrompt });
            }
            if (!string.IsNullOrWhiteSpace(contextPrompt))
            {
                aiConvCtrl.AddMessage(new Message { role = "system", content = contextPrompt });
            }

            // Add global chat memory to NPC if enabled
            foreach (var npcSO in _nPCs)
            {
                if (npcSO != null && npcSO.NameOfNPC == npc.name)
                {
                    if (npcSO.GlobalChatMemory)
                        aiConvCtrl.PopulateGlobalMemory();
                    break;
                }
            }
        }
        else
        {
            Debug.LogWarning($"NPCSpawner: AIConversationController missing on AI NPC: {npc.name}. Cannot set AI behaviour.");
        }
    }

    public GameObject GetNpcInstance(string npcName)
    {
        return _npcInstances.Find(npc => npc != null && npc.name == npcName);
    }
}