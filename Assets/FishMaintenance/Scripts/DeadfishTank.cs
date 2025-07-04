using UnityEngine;

public class DeadfishTank : MonoBehaviour
{
    [SerializeField] private GameObject hoett;
    [SerializeField] private GameObject fish;
    [SerializeField] private GameObject[] additionalFish;
    [SerializeField] private GameObject openDoor;
    [SerializeField] private GameObject closedDoor;
    [SerializeField] private GameObject water;
    [SerializeField] private BNG.Grabber grabberRight;
    [SerializeField] private Task.Subtask subtask;
    [SerializeField] private DialogueTree allTasksCompletedDialogue;
    private DropItem dropItem;

    private GameObject _npc;
    private DialogueBoxController dialogueController;

    // Start is called before the first frame update
    void Start()
    {
        dropItem = gameObject.GetComponent<DropItem>();
        _npc = FindObjectOfType<NPCSpawner>()._npcInstances[0];
        dialogueController = _npc.GetComponent<DialogueBoxController>();
    }

    void OnEnable()
    {
        hoett.SetActive(true);
        fish.SetActive(true);
        water.SetActive(true);
        openDoor.SetActive(true);
        closedDoor.SetActive(false);
        BNG.Grabbable hoettGrabbable = hoett.GetComponent<BNG.Grabbable>();
        grabberRight.GrabGrabbable(hoettGrabbable);

        foreach (GameObject deadfish in additionalFish)
        {
            deadfish.SetActive(true);
        }
    }

    public void OnDeadFishTaskComplete()
    {
        dropItem.DropAll();
        dialogueController.StartDialogue(allTasksCompletedDialogue, 0, "Boss", 0);
    }
    
}
