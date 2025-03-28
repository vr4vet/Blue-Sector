using UnityEngine;

public class BreakTask : MonoBehaviour
{
    [HideInInspector] private NPCSpawner _npcSpawner;
    [HideInInspector] private GameObject _npc;
    [SerializeField] private DialogueTree dialogueTree;

    [SerializeField] private Task.Skill skillBadge;
    [SerializeField] private GameObject deadFishPumpVideo;
    [SerializeField] private GameObject deadFishCountVideo;
    [SerializeField] private GameObject breakAnchor;

    [SerializeField] private GameObject deadFishBreakAnchor;

    private WatchManager watchManager;
    private SkillManager skillManager;
    private DialogueBoxController dialogueController;
    private ConversationController conversationController;

    private bool breakDialoguePlayed = false;
    private bool breakTaskDone = false;
    private bool deadIntroDialoguePlayed = false;



    // Start is called before the first frame update
    void Start()
    {
        watchManager = WatchManager.Instance;
        skillManager = watchManager.GetComponent<SkillManager>();
        _npcSpawner = FindObjectOfType<NPCSpawner>();
        if (_npcSpawner == null)
        {
            Debug.LogError("No NPCSpawner found");
            return;
        }
        _npc = _npcSpawner._npcInstances[0];
        dialogueController = _npc.GetComponent<DialogueBoxController>();
        conversationController = _npc.GetComponentInChildren<ConversationController>();

        dialogueController.m_DialogueChanged.AddListener(OnDialogueChanged);
    }

    private void Update()
    {
        if (!conversationController.isDialogueActive())
        {
            if (breakDialoguePlayed && !breakTaskDone) // when the break dialogue has finished
            {
                Task.Step breakStep = watchManager.GetStep("Taking a break", "Talk to Laila");
                breakStep.SetCompleated(true);
                watchManager.UpdateCurrentSubtask(watchManager.taskHolder.GetTask("Maintenance").GetSubtask("Handling of dead fish"));

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
                if (watchManager.GetStep("Handling of dead fish", "Watch video").RepetionsCompleated >= 1 && conversationController.GetDialogueTree().name == "DeadfishSetup") // activeSelf set to false when video is finished
                {
                    // moving on to dead fish counting dialogue
                    conversationController.NextDialogueTree();
                    conversationController.DialogueTrigger();
                    deadFishPumpVideo.GetComponent<VideoObject>().HideVideoPlayer();
                    deadFishCountVideo.SetActive(true);
                    deadFishBreakAnchor.SetActive(true);

                    watchManager.taskHolder.GetTask("Maintenance").GetSubtask("Handling of dead fish").GetStep("Get info from Laila").SetCompleated(true);
                    //watchManager.PlayAudio(watchManager.success);
                    //watchManager.InvokeBadge(skillBadge);
                    skillManager.CompleteSkill(skillBadge);
                }
            }
        }
    }

    private void OnDialogueChanged(string NPCname, string dialogueTreeName, int section, int index)
    {
        if (dialogueTreeName != "PauseBoss")
            return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!breakDialoguePlayed && other.CompareTag("Player"))
        {
            dialogueController.StartDialogue(dialogueTree, 0, "Boss", 0);
            breakAnchor.SetActive(false);
            breakDialoguePlayed = true;
        }
    }

    private void OnDestroy()
    {
        dialogueController.m_DialogueChanged.RemoveListener(OnDialogueChanged);
    }
}
