#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

/// <summary>
/// Editor utility to check and fix NPC tags
/// </summary>
public class NPCTagChecker : EditorWindow
{
    private bool autoFixTags = true;
    private string[] tagOptions = { "NPC" };
    private int selectedTagIndex = 0;

    [MenuItem("VR4VET/NPC Tag Checker")]
    static void Init()
    {
        NPCTagChecker window = (NPCTagChecker)EditorWindow.GetWindow(typeof(NPCTagChecker));
        window.titleContent = new GUIContent("NPC Tag Checker");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("NPC Tag Checker", EditorStyles.boldLabel);
        GUILayout.Space(10);

        GUILayout.Label("This tool helps ensure all NPCs in the scene are properly tagged.");
        GUILayout.Space(5);

        autoFixTags = EditorGUILayout.Toggle("Auto-fix missing tags", autoFixTags);
        
        GUILayout.Space(5);
        GUILayout.Label("Tag to use:");
        selectedTagIndex = EditorGUILayout.Popup(selectedTagIndex, tagOptions);
        
        GUILayout.Space(15);
        if (GUILayout.Button("Check NPCs"))
        {
            CheckNPCs();
        }
    }

    private void CheckNPCs()
    {
        // Find all objects with NpcTriggerDialogue or ConversationController components
        NpcTriggerDialogue[] npcTriggers = Resources.FindObjectsOfTypeAll<NpcTriggerDialogue>();
        ConversationController[] conversationControllers = Resources.FindObjectsOfTypeAll<ConversationController>();

        List<GameObject> npcsWithoutTag = new List<GameObject>();
        int npcCount = 0;
        int fixedCount = 0;

        // Check NPC Trigger objects
        foreach (NpcTriggerDialogue npc in npcTriggers)
        {
            if (npc.gameObject != null && PrefabUtility.GetPrefabInstanceStatus(npc.gameObject) != PrefabInstanceStatus.NotAPrefab)
            {
                npcCount++;
                if (npc.gameObject.tag != tagOptions[selectedTagIndex])
                {
                    npcsWithoutTag.Add(npc.gameObject);
                    
                    if (autoFixTags)
                    {
                        npc.gameObject.tag = tagOptions[selectedTagIndex];
                        fixedCount++;
                        EditorUtility.SetDirty(npc.gameObject);
                    }
                }
            }
        }

        // Check Conversation Controller objects
        foreach (ConversationController controller in conversationControllers)
        {
            if (controller.gameObject != null && controller.gameObject.tag != tagOptions[selectedTagIndex] && 
                PrefabUtility.GetPrefabInstanceStatus(controller.gameObject) != PrefabInstanceStatus.NotAPrefab)
            {
                // Skip if we already processed this GameObject from NpcTriggerDialogue
                bool alreadyChecked = false;
                foreach (GameObject go in npcsWithoutTag)
                {
                    if (go == controller.gameObject)
                    {
                        alreadyChecked = true;
                        break;
                    }
                }
                
                if (!alreadyChecked)
                {
                    npcCount++;
                    npcsWithoutTag.Add(controller.gameObject);
                    
                    if (autoFixTags)
                    {
                        controller.gameObject.tag = tagOptions[selectedTagIndex];
                        fixedCount++;
                        EditorUtility.SetDirty(controller.gameObject);
                    }
                }
            }
        }

        // Save changes
        if (fixedCount > 0)
        {
            EditorSceneManager.MarkAllScenesDirty();
            Debug.Log($"Fixed {fixedCount} NPCs to use the '{tagOptions[selectedTagIndex]}' tag");
        }

        // Report results
        if (npcsWithoutTag.Count == 0)
        {
            EditorUtility.DisplayDialog("NPC Tag Check", $"All NPCs ({npcCount}) are properly tagged with '{tagOptions[selectedTagIndex]}'.", "OK");
        }
        else if (autoFixTags)
        {
            EditorUtility.DisplayDialog("NPC Tag Check", $"Fixed {fixedCount} NPCs to use the '{tagOptions[selectedTagIndex]}' tag.\n\nTotal NPCs found: {npcCount}", "OK");
        }
        else
        {
            string message = $"Found {npcsWithoutTag.Count} NPCs without the '{tagOptions[selectedTagIndex]}' tag.\n\nTotal NPCs found: {npcCount}\n\nEnable auto-fix to update their tags.";
            EditorUtility.DisplayDialog("NPC Tag Check", message, "OK");
        }
    }
}
#endif