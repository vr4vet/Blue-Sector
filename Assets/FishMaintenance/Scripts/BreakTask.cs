using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakTask : MonoBehaviour
{
    [HideInInspector] private NPCSpawner _npcSpawner;
    [HideInInspector] private GameObject _npc;
    [SerializeField] private MaintenanceManager mm;
    [SerializeField] private DialogueTree dialogueTree;
    private int _activatedCount = 0;


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
        ConversationController conversationController = _npc.GetComponentInChildren<ConversationController>();
        Task.Step breakStep = mm.GetStep("Pause", "Snakk med Laila");
        if (mm.stepCount > 3 && !conversationController.isDialogueActive())
        {

            if (conversationController == null)
            {
                Debug.LogError("The NPC is missing the conversationController");
            }
            if (breakStep.IsCompeleted())
            {
                conversationController.SetDialogueTreeList(new List<DialogueTree>
                {
                    null
                });
                return;
            }
            else
            {
                conversationController.SetDialogueTreeList(dialogueTree);
            }
        }
        if (_activatedCount == 3)
        {
            mm.BadgeChanged.Invoke(breakStep);
        }
        else
        {
            int updatedCount = conversationController.GetActivatedCount();
            if (_activatedCount < 4) _activatedCount = updatedCount;

        }

    }


}
