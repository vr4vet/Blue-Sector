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
    private Task.Step step;
    private FeedbackManager feedbackManager;
    private MaintenanceManager manager;

    // Start is called before the first frame update
    void Start()
    {
        dropItem = gameObject.GetComponent<DropItem>();
        step = manager.GetStep("Håndforing", "Kast mat til fisken");
    }

    // Update is called once per frame
    void Update()
    {
        if (step.IsCompeleted()) dropItem.DropAll();
    }

    void OnEnable()
    {
        feedbackManager = maintenanceManager.GetComponent<FeedbackManager>();
        manager = maintenanceManager.GetComponent<MaintenanceManager>();
        if (manager.GetStep("Hent Utstyr", "Hent bøtte og spade").IsCompeleted())
        {
            SetEquipmentActive();
        }
        else
        {
            feedbackManager.equipmentFeedback("Håndforing");
        }

    }

    public void SetEquipmentActive()
    {
        feedbackManager.addFeedback("Håndforing");
        bucket.SetActive(true);
        shovel.SetActive(true);
        videoObject.SetActive(true);
        // BNG.Grabbable bucketGrabbable = bucket.GetComponent<BNG.Grabbable>();
        BNG.Grabbable shovelGrabbable = shovel.GetComponent<BNG.Grabbable>();
        // grabberLeft.GrabGrabbable(bucketGrabbable);
        grabberRight.GrabGrabbable(shovelGrabbable);
    }

}