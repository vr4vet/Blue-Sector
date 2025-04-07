using UnityEngine;
using Meta.WitAi.TTS.Utilities;
using System;
using System.Collections.Generic;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private NPC[] _nPCs;
    [HideInInspector] public List<GameObject> _npcInstances = new List<GameObject>(); // Initialize list

    [Header("Global AI Settings")]
    [TextArea(3, 10)]
    public string globalContextPrompt = "You are interacting with a user in a virtual reality training simulation."; // Added global context

    // No direct reference to spawnedNpc needed here anymore? Keeping list _npcInstances

    private void Awake()
    {
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
    }

    public GameObject SpawnNPC(NPC npcSO)
    {
        // Instantiate the NPC prefab at the defined location
        GameObject newNPC = Instantiate(npcSO.NpcPrefab, npcSO.SpawnPosition, Quaternion.identity);
        newNPC.name = npcSO.NameOfNPC; // Set GameObject name for easier identification

        // Rotate the NPC
        newNPC.transform.rotation = Quaternion.Euler(npcSO.SpawnRotation);

        // --- Standard Setup (Keep as is) ---
        AttachTTSComponents(newNPC, npcSO.SpatialBlend, npcSO.MinDistance); // Attaches Wit TTSSpeaker
        SetAppearanceAnimationAndVoice(newNPC, npcSO.CharacterModel, npcSO.CharacterAvatar, npcSO.runtimeAnimatorController, npcSO.VoicePresetId); // Sets model & standard Wit voice

        // Make sure animator reference is updated after model is loaded
        var dialogueBoxController = newNPC.GetComponent<DialogueBoxController>();
        if (dialogueBoxController != null)
        {
            // Make sure animator reference is updated after model is loaded
            dialogueBoxController.updateAnimator();
        }

        SetFollowingBehavior(newNPC, npcSO.ShouldFollow);
        SetName(newNPC, npcSO.NameOfNPC);
        SetConversation(newNPC, npcSO.DialogueTreesSO, npcSO.DialogueTreeJSON);

        if (npcSO.WithoutDialogue)
        {
            // Find the collider used for dialogue triggering (assuming it's on a child)
            var conversationController = newNPC.GetComponentInChildren<ConversationController>();
            if (conversationController != null)
            {
                Collider triggerCollider = conversationController.GetComponent<Collider>();
                if (triggerCollider != null) triggerCollider.enabled = false;
                // Optionally disable the ConversationController itself
                // conversationController.enabled = false;
            }
            else
            {
                Debug.LogWarning($"NPCSpawner: Could not find ConversationController on {newNPC.name} to disable dialogue trigger.", newNPC);
            }
        }

        // --- AI Setup (Conditional) ---
        AIConversationController aiConvCtrl = null; // Keep track if AI components are added/found
        if (npcSO.isAiNpc)
        {
            Debug.Log($"NPCSpawner: Setting up AI components for {newNPC.name}");

            // Ensure AI components exist on the root GameObject
            aiConvCtrl = newNPC.GetComponent<AIConversationController>();
            if (aiConvCtrl == null) aiConvCtrl = newNPC.AddComponent<AIConversationController>();

            var aiResponse = newNPC.GetComponent<AIResponseToSpeech>();
            if (aiResponse == null) aiResponse = newNPC.AddComponent<AIResponseToSpeech>();

            var audioSrc = newNPC.GetComponent<AudioSource>();
            if (audioSrc == null) audioSrc = newNPC.AddComponent<AudioSource>(); // Needed for OpenAI TTS and fallback

            // Configure AI components
            SetAIBehaviour(newNPC, npcSO.contextPrompt, npcSO.maxTokens, aiConvCtrl); // Pass component ref
            SetTTSProvider(newNPC, npcSO.selectedTTSProvider, npcSO.OpenAiVoiceId, aiResponse); // Pass component ref

            // Link components (DialogueBoxController needs references to AI components)
            var dialogueBoxCtrl = newNPC.GetComponent<DialogueBoxController>();
            if (dialogueBoxCtrl != null)
            {
                dialogueBoxCtrl._AIConversationController = aiConvCtrl;
                dialogueBoxCtrl._AIResponseToSpeech = aiResponse;
                // Note: UI element references (mic icon, restart button) should ideally be assigned in the NPC_AI prefab variant.
            }
            else
            {
                Debug.LogError($"NPCSpawner: DialogueBoxController missing on AI NPC {newNPC.name}. AI features might not work correctly.", newNPC);
            }

            // Link AIController to ConversationController on child for proximity/input handling
            var conversationCtrl = newNPC.GetComponentInChildren<ConversationController>();
            if (conversationCtrl != null)
            {
                conversationCtrl._AIConversationController = aiConvCtrl; // Link for HandleRecordButton
            }
            else
            {
                Debug.LogError($"NPCSpawner: ConversationController missing on child of AI NPC {newNPC.name}. AI input might not work.", newNPC);
            }
        }

        // return the NPC instance
        return newNPC;
    }

    // Method to attach standard Wit TTS components (Unchanged from new repo version)
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
                // Assign the *GameObject* containing the TTSSpeaker
                // DialogueBoxController can then GetComponentInChildren<TTSSpeaker>() if needed
                dialogueController.TTSSpeaker = ttsSpeaker.gameObject;

                AudioSource speakerAudio = ttsSpeaker.GetComponentInChildren<AudioSource>();
                if (speakerAudio != null)
                {
                    // Use provided values or defaults
                    speakerAudio.spatialBlend = (spatialBlend >= 0 && spatialBlend <= 1) ? spatialBlend : 1; // Default to 3D audio
                    speakerAudio.minDistance = (minDistance > 0) ? minDistance : 5; // Default min distance
                                                                                    // Add other audio source configurations if needed (volume, doppler, etc.)
                                                                                    // speakerAudio.volume = 0.8f; // Example
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

    // Method to set appearance and *standard* Wit voice (Unchanged from new repo version)
    public void SetAppearanceAnimationAndVoice(GameObject npc, GameObject characterModelPrefab, Avatar characterAvatar, RuntimeAnimatorController runtimeAnimatorController, int voicePresetId) // voicePresetId is the standard Wit ID
    {
        SetCharacterModel setCharacterModel = npc.GetComponent<SetCharacterModel>();
        if (setCharacterModel == null)
        {
            Debug.LogError($"NPCSpawner: The NPC {npc.name} is missing the script SetCharacterModel", npc);
        }
        else
        {
            // Ensure voicePresetId is valid for standard Wit TTS setup if needed by SetCharacterModel
            setCharacterModel.ChangeCharacter(characterModelPrefab, characterAvatar, runtimeAnimatorController, voicePresetId);
        }
    }

    // Method to set standard following behaviour (Unchanged from new repo version)
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

    // Method to set display name (Unchanged from new repo version)
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

    // Method to set dialogue trees (Unchanged from new repo version)
    public void SetConversation(GameObject npc, DialogueTree[] dialogueTreesSO, TextAsset[] dialogueTreesJSON)
    {
        ConversationController conversationController = npc.GetComponentInChildren<ConversationController>(); // Find in children
        if (conversationController == null)
        {
            Debug.LogError($"NPCSpawner: The NPC {npc.name} is missing the ConversationController in its children", npc);
        }
        else
        {
            conversationController.SetDialogueTreeList(dialogueTreesSO, dialogueTreesJSON);
        }
    }


    // --- New AI Configuration Methods ---

    // Sets context and max tokens for the AI
    public void SetAIBehaviour(GameObject npc, string contextPrompt, int maxTokens, AIConversationController aiConvCtrl)
    {
        if (aiConvCtrl != null)
        {
            Debug.Log($"NPCSpawner: Setting AI behaviour for {npc.name}. MaxTokens: {maxTokens}");
            aiConvCtrl.contextPrompt = contextPrompt; // Set specific context
            aiConvCtrl.maxTokens = maxTokens > 0 ? maxTokens : 150; // Use provided or default

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
        }
        else { Debug.LogWarning($"NPCSpawner: AIConversationController missing on AI NPC: {npc.name}. Cannot set AI behaviour."); }
    }

    // Sets the TTS provider and specific voice ID for AI responses
    // Method to set the TTS provider and specific voice ID for AI responses
    public void SetTTSProvider(GameObject npc, TTSProvider ttsProvider, string openAiVoiceId, AIResponseToSpeech aiResponse)
    {
        var dialogueBoxCtrl = npc.GetComponent<DialogueBoxController>();
        if (dialogueBoxCtrl == null)
        {
            Debug.LogError($"NPCSpawner: DialogueBoxController missing on NPC: {npc.name}. Cannot set TTS provider.");
            return;
        }

        Debug.Log($"NPCSpawner: Setting TTS provider for {npc.name} to {ttsProvider}");

        if (ttsProvider == TTSProvider.OpenAI)
        {
            dialogueBoxCtrl.useOpenAiTTS(); // Tell DialogueBoxController to use OpenAI via AIResponseToSpeech
            if (aiResponse != null)
            {
                // Set the specific OpenAI voice
                aiResponse.OpenAiVoiceId = string.IsNullOrEmpty(openAiVoiceId) ? "alloy" : openAiVoiceId;
                Debug.Log($"NPCSpawner: Set OpenAI Voice ID to: {aiResponse.OpenAiVoiceId}");
            }
            else
            {
                Debug.LogWarning($"NPCSpawner: AIResponseToSpeech missing on AI NPC: {npc.name}. Cannot set OpenAI voice ID.");
            }
        }
        else // TTSProvider.Wit
        {
            dialogueBoxCtrl.useWitTTS(); // Tell DialogueBoxController to use Wit via AIResponseToSpeech (which uses TTSSpeaker)

            // Don't try to get an NPC component - instead use a different approach
            NPC npcData = null;

            // Find the NPC scriptable object in _nPCs array that matches this GameObject's name
            foreach (var npcSO in _nPCs)
            {
                if (npcSO != null && npcSO.NameOfNPC == npc.name)
                {
                    npcData = npcSO;
                    break;
                }
            }

            if (npcData != null)
            {
                Debug.Log($"NPCSpawner: WitAI TTS selected. Voice will be determined by the attached TTSSpeaker's settings (Preset {npcData.VoicePresetId}).");
            }
            else
            {
                Debug.LogWarning($"NPCSpawner: NPC data not found for {npc.name}. Cannot determine VoicePresetId.");
            }
        }
    }

    // Optional: Method to get a specific spawned NPC if needed later
    public GameObject GetNpcInstance(string npcName)
    {
        return _npcInstances.Find(npc => npc != null && npc.name == npcName);
    }
}
