using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakTask : MonoBehaviour
{
    [HideInInspector] private NPCSpawner _npcSpawner;
    [HideInInspector] private GameObject _npc;
    [SerializeField] private MaintenanceManager mm;
    [SerializeField] private DialogueTree dialogueTree;

    // Start is called before the first frame update
    void Start()
    {
        _npcSpawner = GetComponent<NPCSpawner>();
        if (_npcSpawner == null)
        {
            Debug.LogError("No NPCSpawner found");
            return;
        }

        _npc = _npcSpawner._npcInstances[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (mm.stepCount > 0 && !_npc.GetComponentInChildren<ConversationController>().isDialogueActive())
        {
            ConversationController conversationController = _npc.GetComponentInChildren<ConversationController>();
            if (conversationController == null)
            {
                Debug.LogError("The NPC is missing the conversationController");
            }
            else
            {
                conversationController.SetDialogueTreeList(dialogueTree);
            }
        }

    }
}
