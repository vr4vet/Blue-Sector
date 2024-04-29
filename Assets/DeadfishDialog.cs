using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadfishDialog : MonoBehaviour
{
    [HideInInspector] private NPCSpawner _npcSpawner;
    [HideInInspector] private GameObject _npc;
    [SerializeField] private MaintenanceManager mm;
    [SerializeField] private DialogueTree dialogueTree;
    [SerializeField] private GameObject video2;
    private Task.Step videoStep;

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
        Task.Task task = mm.MaintenanceTask;
        Task.Step videoStep = mm.GetStep("Dødfisk håndtering", "Se Video");
    }

    public void UpdateDialog(int repetitionCount)
    {
        ConversationController conversationController = _npc.GetComponentInChildren<ConversationController>();
        if (conversationController == null)
        {
            Debug.LogError("The NPC is missing the conversationController");
        }
        else
        {
            if (repetitionCount == 1)
            {
                conversationController.SetDialogueTreeList(dialogueTree);
                video2.SetActive(true);
            }
            // else if (repetitionCount == 2)
            // {
                // conversationController.SetDialogueTreeList(dialogueTree2);
                // conversationController.SetDialogueTreeList(new List<DialogueTree>
                // {
                //     null
                // });
            // }
        }
    }
}
