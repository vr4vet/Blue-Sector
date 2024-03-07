using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExitTeleportationAnchor : MonoBehaviour
{
    public GameObject cylinder;
    public GameObject cylinderGlow;
    public GameObject teleportationAnchor;
    [SerializeField] private MaintenanceManager manager;
    [SerializeField] private string task;
    [SerializeField] private string subTask;
    [SerializeField] private string step;

    public void OnTriggerExit(Collider player)
    {
        if (manager.GetStep(task, subTask, step).IsCompeleted())
        {
            teleportationAnchor.SetActive(false);
            return;
        }
        cylinder.SetActive(true);
        cylinderGlow.SetActive(true);
    }
}
