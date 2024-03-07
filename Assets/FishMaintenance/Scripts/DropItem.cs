using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] private MaintenanceManager manager;
    [SerializeField] private string task;
    [SerializeField] private string subTask;
    [SerializeField] private string step;
    public GameObject item;
    [SerializeField] private BNG.Grabber grabberRight;

    public void OnTriggerExit(Collider player)
    {
        if (manager.GetStep(task, subTask, step).IsCompeleted())
        {
            return;
        }
        item.GetComponent<BNG.Grabbable>().DropItem(grabberRight, true, true);
        item.SetActive(false);
    }
}
