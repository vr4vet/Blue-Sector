#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor tool to create an idle dialogue tree asset
/// </summary>
public class IdleDialogueTreeCreator : EditorWindow
{
    [MenuItem("NPCTools/Create Idle Dialogue Tree")]
    public static void CreateIdleDialogueTree()
    {
        // Create a new dialogue tree asset
        DialogueTree idleTree = ScriptableObject.CreateInstance<DialogueTree>();
        
        // Configure the dialogue tree
        idleTree.shouldTriggerOnProximity = false;
        idleTree.speakButtonOnExit = true;
        
        // Create a section for idle dialogue
        DialogueSection idleSection = new DialogueSection();
        idleSection.dialogue = new string[1];
        idleSection.dialogue[0] = "I notice you haven't taken any action for a while. Is there something I can help with?";
        
        // Add the section to the tree
        idleTree.sections = new DialogueSection[1];
        idleTree.sections[0] = idleSection;
        
        // Initialize the interruptable elements
        idleTree.InitializeInterruptableElements();
        
        // Create the asset
        AssetDatabase.CreateAsset(idleTree, "Assets/VR4VET/Resources/DialogueTrees/IdleDialogueTree.asset");
        AssetDatabase.SaveAssets();
        
        // Select the created asset in the Project window
        Selection.activeObject = idleTree;
        EditorUtility.FocusProjectWindow();
        
        Debug.Log("Created IdleDialogueTree asset at Assets/VR4VET/Resources/DialogueTrees/IdleDialogueTree.asset");
    }
}
#endif