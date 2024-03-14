using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExitTeleportationAnchor : MonoBehaviour
{
    public GameObject cylinder;
    public GameObject cylinderGlow;
    [SerializeField] private MaintenanceManager manager;
    [SerializeField] private AddInstructionsToWatch watch;

    [SerializeField] private string subTask;
    [SerializeField] private string step;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cylinder.SetActive(false);
            cylinderGlow.SetActive(false);

        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            watch.emptyInstructions();

            if (manager.GetStep(subTask, step).IsCompeleted())
            {
                gameObject.SetActive(false);
                return;
            }

            cylinder.SetActive(true);
            cylinderGlow.SetActive(true);
        }

    }


}




