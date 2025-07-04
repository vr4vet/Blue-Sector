using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyCheckStep : MonoBehaviour
{
    [SerializeField] private GameObject maintenanceManager;
    [SerializeField] private BNG.Grabber grabber;
    [SerializeField] private GameObject grabbableObject;
    [SerializeField] private Task.Step equipmentStep;

    void OnEnable()
    {
        if (equipmentStep.IsCompeleted())
        {
            SetEquipmentActive();
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


