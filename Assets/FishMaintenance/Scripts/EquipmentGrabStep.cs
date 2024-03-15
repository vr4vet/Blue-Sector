using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentGrabStep : MonoBehaviour
{
    [SerializeField] private MaintenanceManager manager;
    [SerializeField] private string equipmentName;

    [SerializeField] private string step;

    void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Hand"))
        {
            manager.CompleteStep("Hent Utstyr", step);
            gameObject.SetActive(false);
        }
    }
    public void GrabEquipment()
    {

        // BNG.Grabbable grabbable = gameObject.GetComponent<BNG.Grabbable>();
        // BNG.Grabber grabber = grabbable.GetPrimaryGrabber();
        // grabbable.DropItem(grabber, true, true);
        // gameObject.Set

    }
}
