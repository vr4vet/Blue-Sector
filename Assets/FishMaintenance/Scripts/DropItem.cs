using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] private MaintenanceManager manager;
    [SerializeField] private string subTask;
    [SerializeField] private string step;
    public GameObject itemRight;
    // [SerializeField] private BNG.Grabber grabberRight;
    public GameObject itemLeft;

    // [SerializeField] private BNG.Grabber grabberLeft;

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (manager.GetStep(subTask, step).IsCompeleted())
            {
                return;
            }
            if (itemRight)
            {
                itemRight.GetComponent<BNG.Grabbable>().DropItem(true, true);
                itemRight.SetActive(false);
            }
            if (itemLeft)
            {
                itemLeft.GetComponent<BNG.Grabbable>().DropItem(true, true);
                itemLeft.SetActive(false);
            }


        }
    }
}
