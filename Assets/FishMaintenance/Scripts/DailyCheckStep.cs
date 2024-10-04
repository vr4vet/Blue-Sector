using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyCheckStep : MonoBehaviour
{
    [SerializeField] private GameObject maintenanceManager;
    [SerializeField] private BNG.Grabber grabber;
    [SerializeField] private GameObject grabbableObject;
    [SerializeField] private string equipmentStep;
    [SerializeField] private string step;
    private FeedbackManager feedbackManager;
    private MaintenanceManager manager;

    void OnEnable()
    {
        feedbackManager = maintenanceManager.GetComponent<FeedbackManager>();
        manager = maintenanceManager.GetComponent<MaintenanceManager>();
        if (manager.GetStep("Get Equipment", equipmentStep).IsCompeleted())
        {
            SetEquipmentActive();
        }
        else
        {
            feedbackManager.equipmentFeedback(step);
        }
    }

    public void SetEquipmentActive()
    {
        //feedbackManager.AddFeedback(step);
        grabbableObject.SetActive(true);
        BNG.Grabbable grabbable = grabbableObject.GetComponent<BNG.Grabbable>();
        grabber.GrabGrabbable(grabbable);
    }
}


