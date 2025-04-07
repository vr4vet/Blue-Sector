// Purpose: Custom Inspector for the NPC Scriptable Object to handle TTS provider selection.
// Note: Unchanged from the deprecated version. Requires UnityEditor namespace.
#if UNITY_EDITOR // Important: Keep editor scripts wrapped
using UnityEngine;
using UnityEditor;
using System;

// Custom inspector for TTS provider selection for the NPC scriptable object
[CustomEditor(typeof(NPC))] // Target the NPC script
public class NPCInspector : Editor
{
    public override void OnInspectorGUI()
    {
        NPC npc = (NPC)target; // Cast the target object to NPC

        // Draw the default inspector fields first
        DrawDefaultInspector();

        // --- AI Configuration Section ---
        // Only show AI settings if isAiNpc is true
        if (npc.isAiNpc)
        {
            EditorGUILayout.Space(); // Add some spacing
            EditorGUILayout.LabelField("AI Configuration", EditorStyles.boldLabel);

            // TTS Provider Selection
            TTSProvider[] providers = (TTSProvider[])Enum.GetValues(typeof(TTSProvider));
            // string[] providerNames = Enum.GetNames(typeof(TTSProvider)); // Not needed for EnumPopup

            npc.selectedTTSProvider = (TTSProvider)EditorGUILayout.EnumPopup("TTS Provider", npc.selectedTTSProvider);

            // Voice Preset Selection based on Provider
            switch (npc.selectedTTSProvider)
            {
                case TTSProvider.Wit:
                    // Wit voices are often indexed presets. Assuming 0-21 as before.
                    int witPresetCount = 22; // Or get dynamically from WitSDK if possible
                    string[] WitVoicePresets = new string[witPresetCount];
                    for (int i = 0; i < witPresetCount; i++)
                    {
                        WitVoicePresets[i] = $"Wit Preset {i}"; // Make names clearer
                    }

                    // Ensure index is within bounds
                    int selectedWitPresetIndex = Mathf.Clamp(npc.WitVoiceId, 0, WitVoicePresets.Length - 1);
                    selectedWitPresetIndex = EditorGUILayout.Popup("Wit Voice Preset", selectedWitPresetIndex, WitVoicePresets);
                    npc.WitVoiceId = selectedWitPresetIndex;
                    break;

                case TTSProvider.OpenAI:
                    // OpenAI voices are named strings
                    string[] openAiVoicePresets = { "alloy", "echo", "fable", "onyx", "nova", "shimmer" };

                    int selectedOpenAiPresetIndex = Array.IndexOf(openAiVoicePresets, npc.OpenAiVoiceId);
                    if (selectedOpenAiPresetIndex == -1) // If current ID not found (or empty), default to first
                    {
                        selectedOpenAiPresetIndex = 0;
                        npc.OpenAiVoiceId = openAiVoicePresets[0]; // Update the SO value if defaulted
                    }

                    selectedOpenAiPresetIndex = EditorGUILayout.Popup("OpenAI Voice Preset", selectedOpenAiPresetIndex, openAiVoicePresets);
                    npc.OpenAiVoiceId = openAiVoicePresets[selectedOpenAiPresetIndex];
                    break;
            }

            // Context Prompt
            EditorGUILayout.LabelField("Context Prompt (Instructions for AI)");
            npc.contextPrompt = EditorGUILayout.TextArea(npc.contextPrompt, GUILayout.Height(60)); // Text Area for context

            // Max Tokens
            npc.maxTokens = EditorGUILayout.IntField("Max Response Tokens", npc.maxTokens);


        } // End of AI Configuration Section


        // Apply changes if GUI was modified
        if (GUI.changed)
        {
            EditorUtility.SetDirty(npc); // Mark the Scriptable Object as dirty to save changes
            // AssetDatabase.SaveAssets(); // Optional: force save immediately
        }
    }
}
#endif // UNITY_EDITOR