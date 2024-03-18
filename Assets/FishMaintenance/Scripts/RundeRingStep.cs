using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RundeRingStep : MonoBehaviour
{
    public GameObject fixedItem;
    public GameObject itemGuide;
    public GameObject handheldItem;
    [SerializeField] private BNG.Grabber grabberRight;
    [SerializeField] private MaintenanceManager manager;
    [SerializeField] private string step;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "rundeRing")
        {
            fixedItem.SetActive(true);
            gameObject.SetActive(false);
            handheldItem.GetComponent<BNG.Grabbable>().DropItem(grabberRight, true, true);
            handheldItem.SetActive(false);
            manager.CompleteStep("Runde På Ring", step);
        }
    }
}
