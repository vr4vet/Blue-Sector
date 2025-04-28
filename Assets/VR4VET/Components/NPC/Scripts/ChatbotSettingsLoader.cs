using UnityEngine;
using Meta.WitAi.TTS.Utilities;

/// <summary>
/// Applies settings from a ChatbotSettings asset to this Chatbot
/// </summary>
public class ChatbotSettingsLoader : MonoBehaviour
{
    [SerializeField] private ChatbotSettings chatbotSettings;
    
    private void Awake()
    {
        if (chatbotSettings == null)
        {
            Debug.LogError("ChatbotSettings asset not assigned!");
            return;
        }
        
        ApplySettings();
    }
    
    private void ApplySettings()
    {
        // Apply voice settings to DialogueBoxController
        DialogueBoxController dialogueController = GetComponent<DialogueBoxController>();
        if (dialogueController != null)
        {
            // Apply WitAI setting
            dialogueController.useWitAI = chatbotSettings.useWitAI;
            
            // Apply voice preset to TTSSpeaker if available
            TTSSpeaker ttsSpeaker = GetComponentInChildren<TTSSpeaker>();
            if (ttsSpeaker != null && !string.IsNullOrEmpty(chatbotSettings.voicePreset))
            {
                // Set the voice preset ID - Wit.ai uses preset IDs for different voices
                ttsSpeaker.presetVoiceID = chatbotSettings.voicePreset;
                Debug.Log($"Set TTS voice preset to {chatbotSettings.voicePreset}");
            }
            else if (ttsSpeaker == null)
            {
                Debug.LogWarning("No TTSSpeaker found on Chatbot. Voice settings not applied.");
            }
        }
        else
        {
            Debug.LogError("No DialogueBoxController found on Chatbot!");
        }
        
        // Apply settings to SimpleFishController
        SimpleFishController fishController = GetComponent<SimpleFishController>();
        if (fishController != null)
        {
            // Set fish color/material if applicable
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                if (chatbotSettings.fishMaterial != null)
                {
                    renderer.material = chatbotSettings.fishMaterial;
                }
                else
                {
                    // Just change the color of existing materials
                    foreach (Material mat in renderer.materials)
                    {
                        if (mat.HasProperty("_Color"))
                        {
                            mat.color = chatbotSettings.fishColor;
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("No SimpleFishController found on Chatbot. Fish appearance settings not applied.");
        }
            
        // Set name for display
        DisplayName displayNameComponent = GetComponent<DisplayName>();
        if (displayNameComponent != null)
        {
            // Using the correct method UpdateDisplayedName instead of SetName
            displayNameComponent.UpdateDisplayedName(chatbotSettings.chatbotName);
            Debug.Log($"Set Chatbot name to {chatbotSettings.chatbotName}");
        }
        
        // Apply personality to AIConversationController if present
        AIConversationController aiController = GetComponent<AIConversationController>();
        if (aiController != null && !string.IsNullOrEmpty(chatbotSettings.personalityPrompt))
        {
            // Since we might not have direct access to add system messages,
            // look for a method that might support adding personality/context
            var methods = aiController.GetType().GetMethods();
            foreach (var method in methods)
            {
                if (method.Name.Contains("AddMessage") || 
                    method.Name.Contains("SetPersonality") || 
                    method.Name.Contains("SetSystemPrompt"))
                {
                    try
                    {
                        // Try to invoke the method with the personality prompt
                        method.Invoke(aiController, new object[] { chatbotSettings.personalityPrompt });
                        Debug.Log("Applied personality prompt to AIConversationController");
                        break;
                    }
                    catch (System.Exception)
                    {
                        // Method signature didn't match, continue trying others
                        continue;
                    }
                }
            }
        }
        else if (aiController == null)
        {
            Debug.LogWarning("No AIConversationController found on Chatbot. Personality settings not applied.");
        }
    }
}