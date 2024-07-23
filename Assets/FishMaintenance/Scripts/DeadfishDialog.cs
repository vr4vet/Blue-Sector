using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadfishDialog : MonoBehaviour
{
    [HideInInspector] private NPCSpawner _npcSpawner;
    [HideInInspector] private GameObject _npc;
    [SerializeField] private GameObject videoObject;
    [SerializeField] private MaintenanceManager mm;
    [SerializeField] private DialogueTree dialogueTree;
    [SerializeField] public Task.Step videoStep;
    private Task.Step infoStep;
    private Task.Task task;
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
        task = mm.MaintenanceTask;
        dialogueController = _npc.gameObject.GetComponent<DialogueBoxController>();
        infoStep = mm.GetStep("Dødfisk håndtering", "Få info fra Laila");
    }
//ye så step må bli riktig for at det skal fungere
//dialogen er slutt men må mere til for å ikke få begge videoer opp samtidig
//unity events i dialogue box kanskje der har de endret mye i hvertfall
    void Update()
    { 
        Debug.Log(dialogueController.timesEnded);
        if (dialogueController.timesEnded == 2)
        {
            mm.CompleteStep(infoStep);
        }
        if (dialogueController.timesEnded == 2 && dialogueController.dialogueEnded)
        {
            ConversationController conversationController = _npc.GetComponentInChildren<ConversationController>();
            if (conversationController == null)
            {
                Debug.LogError("The NPC is missing the conversationController");
            }
            else
            {
                if (!conversationController.isDialogueActive())
                {
                    videoObject.SetActive(true);
                    conversationController.SetDialogueTreeList(new List<DialogueTree>
                    {
                        null
                    });
                    this.enabled = false;
                }
            }
        }
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
