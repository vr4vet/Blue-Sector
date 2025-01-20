using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandFeeding : MonoBehaviour
{

    [SerializeField] private GameObject maintenanceManager;
    [SerializeField] private GameObject bucket;
    [SerializeField] private GameObject shovel;
    [SerializeField] private GameObject videoObject;
    [SerializeField] private BNG.Grabber grabberLeft;
    [SerializeField] private BNG.Grabber grabberRight;
    private DropItem dropItem;
    public Task.Subtask subtask;
    private Task.Step step;
    private FeedbackManager feedbackManager;
    private MaintenanceManager manager;



    // Start is called before the first frame update
    void Start()
    {



        dropItem = gameObject.GetComponent<DropItem>();
        step = subtask.GetStep("Throw food to the fish");
    }

    // Update is called once per frame
    void Update()
    {
        if (step.IsCompeleted()) dropItem.DropAll();
        if (subtask.Compleated())
        {
            bucket.SetActive(false);
        }

    }

    void OnEnable()
    {
        manager = maintenanceManager.GetComponent<MaintenanceManager>();
        feedbackManager = maintenanceManager.GetComponent<FeedbackManager>();

        if (manager.GetStep("Get Equipment", "Get bucket and spade").IsCompeleted())
        {
            SetEquipmentActive();

        }
        else
        {
            feedbackManager.equipmentFeedback("Handfeeding");
        }

    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            bucket.SetActive(false);
            videoObject.GetComponent<VideoObject>().HideVideoPlayer();
        }
    }

    public void SetEquipmentActive()
    {
        //feedbackManager.AddFeedback("Handfeeding");
        bucket.SetActive(true);
        shovel.SetActive(true);
        if(! subtask.GetStep("Watch video").IsCompeleted())videoObject.SetActive(true);
        // BNG.Grabbable bucketGrabbable = bucket.GetComponent<BNG.Grabbable>();
        BNG.Grabbable shovelGrabbable = shovel.GetComponent<BNG.Grabbable>();
        // grabberLeft.GrabGrabbable(bucketGrabbable);
        grabberRight.GrabGrabbable(shovelGrabbable);
    }

}