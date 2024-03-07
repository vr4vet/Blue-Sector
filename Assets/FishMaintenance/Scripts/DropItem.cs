using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] private MaintenanceManager manager;
    [SerializeField] private string task;
    [SerializeField] private string subTask;
    [SerializeField] private string step;
    public GameObject handSplint;
    [SerializeField] private BNG.Grabber grabberRight;

    public void OnTriggerExit(Collider player)
    {
        if (manager.GetStep(task, subTask, step).IsCompeleted())
        {
            return;
        }
        handSplint.GetComponent<BNG.Grabbable>().DropItem(grabberRight, true, true);
        handSplint.SetActive(false);
    }
}
