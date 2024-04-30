using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadfishDialog : MonoBehaviour
{
    [HideInInspector] private NPCSpawner _npcSpawner;
    [HideInInspector] private GameObject _npc;
    [SerializeField] private MaintenanceManager mm;
    [SerializeField] private DialogueTree dialogueTree;
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

    public void UpdateDialog()
    {
        ConversationController conversationController = _npc.GetComponentInChildren<ConversationController>();
        if (conversationController == null)
        {
            Debug.LogError("The NPC is missing the conversationController");
        }
        else
        {
            conversationController.SetDialogueTreeList(dialogueTree);
            conversationController.startDialog();
        }
    }
}
