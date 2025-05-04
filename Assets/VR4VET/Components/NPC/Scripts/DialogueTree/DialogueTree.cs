using System;
using UnityEngine;

[CreateAssetMenu(menuName = "NPCScriptableObjects/DialogueTree")]
[Serializable]
public class DialogueTree : ScriptableObject
{
    public bool shouldTriggerOnProximity = true;
    public bool speakButtonOnExit = true;
    public DialogueSection[] sections;

#if UNITY_EDITOR
    // This method is called when something changes in the inspector or the asset is loaded.
    private void OnValidate()
    {
        InitializeInterruptableElements();
    }

    // Method to initialize or resize the interruptableElements array based on the dialogue size in each section.
    public void InitializeInterruptableElements()
    {
        if (sections == null) return;

        bool changed = false;
        for (int i = 0; i < sections.Length; i++)
        {
            int dialogueCount = (sections[i].dialogue != null) ? sections[i].dialogue.Length : 0;

            // Initialize or resize the interruptableElements array
            if (sections[i].interruptableElements == null || sections[i].interruptableElements.Length != dialogueCount)
            {
                // Create new array or resize existing one
                bool[] newArray = new bool[dialogueCount];
                // Optionally copy old values if resizing and old array exists
                if (sections[i].interruptableElements != null)
                {
                    int copyLength = Mathf.Min(sections[i].interruptableElements.Length, dialogueCount);
                    Array.Copy(sections[i].interruptableElements, newArray, copyLength);
                }
                sections[i].interruptableElements = newArray;
                changed = true; // Mark that we made a change
            }
        }
        // If we changed something, mark the asset dirty so Unity saves the change
        if (changed)
        {
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
#endif // UNITY_EDITOR
}