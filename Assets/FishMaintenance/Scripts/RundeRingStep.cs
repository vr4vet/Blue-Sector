using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RundeRingStep : MonoBehaviour
{
    public GameObject fixedItem;
    public GameObject handheldItem;
    [SerializeField] private BNG.Grabber grabberRight;
    [SerializeField] private MaintenanceManager manager;
    [SerializeField] private string step;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("rundeRing"))
        {
            fixedItem.SetActive(true);
            gameObject.SetActive(false);
            handheldItem.GetComponent<BNG.Grabbable>().DropItem(grabberRight, true, true);
            handheldItem.SetActive(false);
            Task.Step completedStep = manager.GetStep("Daily Round", step);
            manager.CompleteStep(completedStep);
        }
    }
}
