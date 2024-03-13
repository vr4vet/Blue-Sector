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
            // if (manager.GetStep(subTask, step).IsCompeleted())
            // {
            //     return;
            // }
            if (itemRight)
            {
                BNG.Grabbable grabbable = itemRight.GetComponent<BNG.Grabbable>();
                BNG.Grabber grabber = grabbable.GetPrimaryGrabber();
                grabbable.DropItem(grabber, true, true);
                itemRight.SetActive(false);
            }
            if (itemLeft)
            {
                BNG.Grabbable grabbable = itemLeft.GetComponent<BNG.Grabbable>();
                BNG.Grabber grabber = grabbable.GetPrimaryGrabber();
                grabbable.DropItem(grabber, true, true);
                itemLeft.SetActive(false);
            }


        }
    }
}
