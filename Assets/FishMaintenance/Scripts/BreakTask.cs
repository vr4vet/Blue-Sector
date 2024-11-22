using UnityEngine;

public class BreakTask : MonoBehaviour
{
    [HideInInspector] private NPCSpawner _npcSpawner;
    [HideInInspector] private GameObject _npc;
    [SerializeField] private MaintenanceManager mm;
    [SerializeField] private DialogueTree dialogueTree;

    [SerializeField] private Task.Skill skillBadge;
    [SerializeField] private GameObject deadFishPumpVideo;
    [SerializeField] private GameObject deadFishCountVideo;
    [SerializeField] private GameObject breakAnchor;

    [SerializeField] private GameObject deadFishBreakAnchor;

    

    private int _activatedCount = 0;
    private Task.Step breakStep;
    private DialogueBoxController dialogueController;
    private ConversationController conversationController;

    private bool breakDialoguePlayed = false;
    private bool breakTaskDone = false;
    private bool deadIntroDialoguePlayed = false;
    private bool deadIntroDone = false;



    // Start is called before the first frame update
    void Start()
    {
        _npcSpawner = FindObjectOfType<NPCSpawner>();
        if (_npcSpawner == null)
        {
            Debug.LogError("No NPCSpawner found");
            return;
        }
        _npc = _npcSpawner._npcInstances[0];
        dialogueController = _npc.GetComponent<DialogueBoxController>();
        conversationController = _npc.GetComponentInChildren<ConversationController>();
    }

    // Update is called once per frame
    /*    void Update()
        {
            Task.Task task = mm.MaintenanceTask;
            Task.Step breakStep = mm.GetStep("Pause", "Talk to Laila");
            ConversationController conversationController = _npc.GetComponentInChildren<ConversationController>();
            //Debug.Log(conversationController.isDialogueActive());
            //Debug.Log(dialogueController.dialogueEnded);
            if (task.GetSubtask("Hand-feeding").Compleated() && task.GetSubtask("Daily Round").Compleated() && !conversationController.isDialogueActive())
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

        }*/

    private void Update()
    {
        if (!conversationController.isDialogueActive())
        {
            if (breakDialoguePlayed && !breakTaskDone) // when the break dialogue has finished
            {
                Task.Step breakStep = mm.GetStep("Taking a break", "Talk to Laila");
                breakStep.SetCompleated(true);
                mm.UpdateCurrentSubtask(mm.taskHolder.GetTask("Maintenance").GetSubtask("Handling of dead fish"));

                // moving on to dead fish intro dialogue
                if (conversationController.GetDialogueTree().name == "PauseBoss")
                {
                    conversationController.NextDialogueTree();
                    conversationController.DialogueTrigger();
                    deadIntroDialoguePlayed = true;
                    deadFishPumpVideo.SetActive(true);

                    //mm.PlayAudio(mm.success);
                    //mm.InvokeBadge(mm.taskHolder.GetSkill("Observant"));
                }



                //breakAnchor.SetActive(false);
                //gameObject.SetActive(false);
                breakTaskDone = true;
            }

            if (deadIntroDialoguePlayed && !deadIntroDone && breakTaskDone) // when the dead fish dialogue has finished
            {
                Debug.Log(conversationController.GetDialogueTree().name);
                if (!deadFishPumpVideo.activeSelf && conversationController.GetDialogueTree().name == "DeadfishSetup") // activeSelf set to false when video is finished
                {
                    // moving on to dead fish counting dialogue
                    conversationController.NextDialogueTree();
                    conversationController.DialogueTrigger();
                    deadFishCountVideo.SetActive(true);
                    deadFishBreakAnchor.SetActive(true);

                    mm.taskHolder.GetTask("Maintenance").GetSubtask("Handling of dead fish").GetStep("Get info from Laila").SetCompleated(true);
                    mm.PlayAudio(mm.success);
                    mm.InvokeBadge(skillBadge);

                    deadIntroDone = true;
                }
            }
        }

/*        if (mm.taskHolder.GetTask("Maintenance").GetSubtask("Handling of dead fish").GetStep("Push the dead fish into the tub").IsCompeleted())
            Debug.Log("Completed");

        if (mm.taskHolder.GetTask("Maintenance").GetSubtask("Handling of dead fish").Compleated())
        {
            Debug.Log("Completed");
            if (conversationController.GetDialogueTree().name == "DeadfishExplanation")
            {
                conversationController.NextDialogueTree();
                conversationController.DialogueTrigger();
            }
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!breakDialoguePlayed && other.CompareTag("Player"))
        {
            // moving on to break dialogue
            conversationController.NextDialogueTree();
            conversationController.DialogueTrigger();
            breakDialoguePlayed = true;
        }
    }
}
