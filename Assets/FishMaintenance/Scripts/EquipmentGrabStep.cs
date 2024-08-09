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

        if (other.CompareTag("Hand"))
        {
            Task.Step completedStep = manager.GetStep("Hent Utstyr", step);
            manager.CompleteStep(completedStep);
            gameObject.SetActive(false);
            guidingHand.SetActive(false);
        }
    }

}
