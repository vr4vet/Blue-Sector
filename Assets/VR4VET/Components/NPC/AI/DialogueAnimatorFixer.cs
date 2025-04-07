using System.Collections;
using UnityEngine;

/// <summary>
/// Helper component to fix animator reference issues with DialogueBoxController.
/// Add this to the NPCSpawner GameObject in scenes having animator initialization issues.
/// </summary>
public class DialogueAnimatorFixer : MonoBehaviour
{
    private NPCSpawner _npcSpawner;

    void Start()
    {
        // Get the NPCSpawner on the same GameObject
        _npcSpawner = GetComponent<NPCSpawner>();
        if (_npcSpawner == null)
        {
            Debug.LogError("DialogueAnimatorFixer: No NPCSpawner found on this GameObject");
            return;
        }

        // Start the coroutine that will fix the animator references
        StartCoroutine(FixAnimators());
    }

    private IEnumerator FixAnimators()
    {
        // First wait for NPCs to be spawned
        yield return new WaitForSeconds(0.5f);

        // Check if NPCs were spawned
        if (_npcSpawner._npcInstances == null || _npcSpawner._npcInstances.Count == 0)
        {
            Debug.LogWarning("DialogueAnimatorFixer: No NPCs were spawned");
            yield break;
        }

        // Loop through all spawned NPCs
        foreach (var npc in _npcSpawner._npcInstances)
        {
            if (npc == null) continue;

            // Get the DialogueBoxController
            var dialogueBoxController = npc.GetComponent<DialogueBoxController>();
            if (dialogueBoxController == null)
            {
                Debug.LogWarning($"DialogueAnimatorFixer: No DialogueBoxController found on NPC {npc.name}");
                continue;
            }

            // Find the animator in the NPC's children
            var animator = npc.GetComponentInChildren<Animator>();
            if (animator != null)
            {
                Debug.Log($"DialogueAnimatorFixer: Setting animator for {npc.name}");
                dialogueBoxController.updateAnimator(animator);
            }
            else
            {
                Debug.LogWarning($"DialogueAnimatorFixer: No Animator found in children of NPC {npc.name}");
            }

            // Also update any ConversationController components
            var conversationController = npc.GetComponentInChildren<ConversationController>();
            if (conversationController != null && animator != null)
            {
                conversationController.updateAnimator(animator);
            }
        }
    }
}