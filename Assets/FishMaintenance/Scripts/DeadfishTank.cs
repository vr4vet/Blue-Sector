using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        dropItem = gameObject.GetComponent<DropItem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (subtask.GetStep("Skyv dødfisken i karet").IsCompeleted()) dropItem.DropAll();
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

        if (manager.getTeleportationAnchorCount() > 10)
        {
            feedbackManager.addFeedback("Dødfisk håndtering");
        }
        else
        {
            foreach (GameObject deadfish in additionalFish)
            {
                deadfish.SetActive(true);
                subtask.GetStep("Skyv dødfisken i karet").RepetionNumber = 4;
            }
        }
    }
}
