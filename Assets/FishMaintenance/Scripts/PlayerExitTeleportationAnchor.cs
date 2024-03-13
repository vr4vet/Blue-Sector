using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExitTeleportationAnchor : MonoBehaviour
{
    public GameObject cylinder;
    public GameObject cylinderGlow;
    public GameObject teleportationAnchor;
    [SerializeField] private MaintenanceManager manager;
    [SerializeField] private AddInstructionsToWatch watch;

    [SerializeField] private string subTask;
    [SerializeField] private string step;

    public void OnTriggerExit(Collider other)
    {
        if (subTask == "Håndforing") SetAnchor();
    }

    public void SetAnchor()
    {
        if (manager.GetStep(subTask, step).IsCompeleted())
        {
            teleportationAnchor.SetActive(false);
            manager.PlaySuccess();
            watch.emptyInstructions();
            return;
        }
        cylinder.SetActive(true);
        cylinderGlow.SetActive(true);
    }

}
