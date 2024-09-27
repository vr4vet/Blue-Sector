using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentGrabStep : MonoBehaviour
{
    [SerializeField] private MaintenanceManager manager;

    [SerializeField] private string step;
    [SerializeField] private GameObject guidingHand;


    void OnTriggerStay(Collider other)
    {

        if (other.name == "Grabber")
        {
            Debug.Log(manager.GetStep("Get equipment", step) == null);
            Task.Step completedStep = manager.GetStep("Get equipment", step);
            
            //Debug.Log(completedStep == null);
            manager.CompleteStep(completedStep);
            gameObject.SetActive(false);
            guidingHand.SetActive(false);
        }
    }

}
