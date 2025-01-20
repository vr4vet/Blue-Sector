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

    private DialogueBoxController dialogueController;
    private ConversationController conversationController;

    private bool breakDialoguePlayed = false;
    private bool breakTaskDone = false;
    private bool deadIntroDialoguePlayed = false;



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
                }
                breakTaskDone = true;
            }

            if (deadIntroDialoguePlayed && breakTaskDone) // when the dead fish dialogue has finished
            {
                if (conversationController.GetDialogueTree().name != "DeadfishSetup")
                    return;

                //Debug.Log(conversationController.GetDialogueTree().name);
                if (mm.GetStep("Handling of dead fish", "Watch video").RepetionsCompleated >= 1 && conversationController.GetDialogueTree().name == "DeadfishSetup") // activeSelf set to false when video is finished
                {
                    // moving on to dead fish counting dialogue
                    conversationController.NextDialogueTree();
                    conversationController.DialogueTrigger();
                    deadFishPumpVideo.GetComponent<VideoObject>().HideVideoPlayer();
                    deadFishCountVideo.SetActive(true);
                    deadFishBreakAnchor.SetActive(true);

                    mm.taskHolder.GetTask("Maintenance").GetSubtask("Handling of dead fish").GetStep("Get info from Laila").SetCompleated(true);
                    mm.PlayAudio(mm.success);
                    mm.InvokeBadge(skillBadge);
                }
            }
        }
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
