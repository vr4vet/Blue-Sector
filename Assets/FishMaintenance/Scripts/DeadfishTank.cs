using UnityEngine;

public class DeadfishTank : MonoBehaviour
{
    [SerializeField] private GameObject maintenanceManager;
    [SerializeField] private GameObject hoett;
    [SerializeField] private GameObject fish;
    [SerializeField] private GameObject[] additionalFish;
    [SerializeField] private GameObject openDoor;
    [SerializeField] private GameObject closedDoor;
    [SerializeField] private GameObject water;
    [SerializeField] private BNG.Grabber grabberRight;
    [SerializeField] private Task.Subtask subtask;
    private DropItem dropItem;
    private FeedbackManager feedbackManager;
    private MaintenanceManager manager;
    
    private GameObject _npc;
    private DialogueBoxController dialogueController;
    private ConversationController conversationController;

    // Start is called before the first frame update
    void Start()
    {
        dropItem = gameObject.GetComponent<DropItem>();
        _npc = FindObjectOfType<NPCSpawner>()._npcInstances[0];
        dialogueController = _npc.GetComponent<DialogueBoxController>();
        conversationController = _npc.GetComponentInChildren<ConversationController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (subtask.GetStep("Push the dead fish into the tub").IsCompeleted())
        {
            dropItem.DropAll();

            // moving on to congratulations conversation
            if (conversationController.GetDialogueTree().name == "DeadfishExplanation")
            {
                conversationController.NextDialogueTree();
                conversationController.DialogueTrigger();
            }
        }
    }

    void OnEnable()
    {
        manager = maintenanceManager.GetComponent<MaintenanceManager>();
        feedbackManager = maintenanceManager.GetComponent<FeedbackManager>();

        hoett.SetActive(true);
        fish.SetActive(true);
        water.SetActive(true);
        openDoor.SetActive(true);
        closedDoor.SetActive(false);
        BNG.Grabbable hoettGrabbable = hoett.GetComponent<BNG.Grabbable>();
        grabberRight.GrabGrabbable(hoettGrabbable);

        if (manager.GetTeleportationAnchorCount() > 10)
        {
            //feedbackManager.AddFeedback("Dødfisk håndtering");
        }
        else
        {
            foreach (GameObject deadfish in additionalFish)
            {
                deadfish.SetActive(true);
                subtask.GetStep("Push the dead fish into the tub").RepetionNumber = 4;
            }
        }
    }
}
