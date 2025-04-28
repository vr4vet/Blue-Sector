using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using Meta.WitAi.TTS;
using Meta.WitAi.TTS.Utilities;
using Meta.WitAi.TTS.Integrations;
#endif

/// <summary>
/// ScriptableObject to store settings for the Chatbot
/// Similar to NPC character settings used for other NPCs
/// </summary>
[CreateAssetMenu(fileName = "ChatbotSettings", menuName = "VR4VET/Chatbot Settings", order = 1)]
public class ChatbotSettings : ScriptableObject
{
    [Header("Voice Settings")]
    public bool useWitAI = true; // Use Wit.ai for TTS instead of OpenAI
    
    [Tooltip("Voice preset ID to use with WitAI TTS")]
    #if UNITY_EDITOR
    [VoicePresetDropdown]
    #endif
    public string voicePreset = ""; // Will be populated by dropdown in editor
    
    [Header("Appearance Settings")]
    public Material fishMaterial; // Optional custom material
    public Color fishColor = Color.blue;
    
    [Header("Behavior Settings")]
    [Range(0.5f, 3f)] 
    public float movementSpeed = 1f;
    [Range(0.5f, 3f)]
    public float animationSpeed = 1f;
    
    [Header("Identity")]
    public string chatbotName = "Bubbles";
    [TextArea(3, 10)]
    public string personalityPrompt = "You are Bubbles, a friendly fish assistant. You love to help people and share information about the underwater world. You have a cheerful personality and occasionally make fish-related puns. You always try to guide players who seem lost or idle.";
}

#if UNITY_EDITOR
// Custom property drawer to create a dropdown for voice presets
public class VoicePresetDropdownAttribute : PropertyAttribute { }

[CustomPropertyDrawer(typeof(VoicePresetDropdownAttribute))]
public class VoicePresetDropdownDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Make sure we're drawing a string property
        if (property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.PropertyField(position, property, label);
            return;
        }

        EditorGUI.BeginProperty(position, label, property);
        
        // Get all available presets from TTSService if possible
        string[] presets = GetAvailablePresets();
        
        // Find current index
        int currentIndex = -1;
        string currentValue = property.stringValue;
        for (int i = 0; i < presets.Length; i++)
        {
            if (presets[i] == currentValue)
            {
                currentIndex = i;
                break;
            }
        }
        
        if (currentIndex < 0 && presets.Length > 0)
        {
            currentIndex = 0;
        }

        // Draw popup
        int newIndex = EditorGUI.Popup(position, label.text, currentIndex, presets);
        
        // Update value if changed
        if (newIndex >= 0 && newIndex < presets.Length && newIndex != currentIndex)
        {
            property.stringValue = presets[newIndex];
        }
        
        EditorGUI.EndProperty();
    }
    
    // Get available presets from the scene
    private string[] GetAvailablePresets()
    {
        // Try to find TTSService in the scene
        TTSService[] services = Object.FindObjectsOfType<TTSService>();
        if (services != null && services.Length > 0)
        {
            TTSService service = services[0];
            
            // First check if we can get voice settings
            var voiceSettings = service.GetAllPresetVoiceSettings();
            if (voiceSettings != null && voiceSettings.Length > 0)
            {
                string[] results = new string[voiceSettings.Length];
                for (int i = 0; i < voiceSettings.Length; i++)
                {
                    results[i] = voiceSettings[i].SettingsId;
                }
                return results;
            }
        }
        
        // Fallback to common voice presets
        return new string[] 
        { 
            "Charlie", "Clyde", "Dave", "Dorothy", "Jasmine", "Jelly", 
            "Max", "Melody", "Nova", "Ripley", "Rose", "Stella", "Windows" 
        };
    }
}
#endif