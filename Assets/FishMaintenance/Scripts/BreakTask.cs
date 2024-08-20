using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BreakTask : MonoBehaviour
{
    [HideInInspector] private NPCSpawner _npcSpawner;
    [HideInInspector] private GameObject _npc;
    [SerializeField] private MaintenanceManager mm;
    [SerializeField] private DialogueTree dialogueTree;

    [SerializeField] private Task.Skill skillBadge;
    [SerializeField] private GameObject deadfishTask;
    [SerializeField] private GameObject breakAnchor;
    private int _activatedCount = 0;
    private Task.Step breakStep;
    private DialogueBoxController dialogueController;



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
        dialogueController = _npc.gameObject.GetComponent<DialogueBoxController>();
    }

    // Update is called once per frame
    void Update()
    {
        Task.Task task = mm.MaintenanceTask;
        Task.Step breakStep = mm.GetStep("Pause", "Snakk med Laila");
        ConversationController conversationController = _npc.GetComponentInChildren<ConversationController>();
        if (task.GetSubtask("Håndforing").Compleated() && task.GetSubtask("Runde På Ring").Compleated() && !conversationController.isDialogueActive())
        {
            if (conversationController == null)
            {
                Debug.LogError("The NPC is missing the conversationController");
            }
            if (dialogueController.dialogueEnded && !breakStep.IsCompeleted() && dialogueController.timesEnded == 1)
            {
                mm.CompleteStep(breakStep);
                deadfishTask.SetActive(true);
                breakAnchor.SetActive(false);
                conversationController.SetDialogueTreeList(new List<DialogueTree>
                {
                    null
                });
                this.enabled = false;
                return;
            }
            else
            {
                conversationController.SetDialogueTreeList(dialogueTree);
            }
        }
        if (_activatedCount == 3)
        {
            mm.BadgeChanged.Invoke(skillBadge);
        }
        else
        {
            int updatedCount = conversationController.GetActivatedCount();
            if (_activatedCount < 4) _activatedCount = updatedCount;

        }

    }


}
