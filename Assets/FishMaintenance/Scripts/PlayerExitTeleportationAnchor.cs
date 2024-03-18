using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExitTeleportationAnchor : MonoBehaviour
{
    public GameObject cylinder;
    public GameObject cylinderGlow;
    [SerializeField] private GameObject maintenanceManager;
    [SerializeField] private string subTask;
    [SerializeField] private string step;
    private MaintenanceManager manager;
    private FeedbackManager feedbackManager;
    private BoxCollider boxCollider;
    private Vector3 originalSize;

    void Start()
    {
        manager = maintenanceManager.GetComponent<MaintenanceManager>();
        feedbackManager = maintenanceManager.GetComponent<FeedbackManager>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
        originalSize = boxCollider.size;

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cylinder.SetActive(false);
            cylinderGlow.SetActive(false);
            boxCollider.size = new Vector3(1.55380726f, 2.72183228f, 2.2479949f);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            feedbackManager.StopMoreFeedback();
            feedbackManager.emptyInstructions();
            boxCollider.size = originalSize;
            if (string.IsNullOrEmpty(step))
            {
                if (manager.GetSubtask(subTask).Compleated())
                {
                    gameObject.SetActive(false);
                    return;
                }
            }
            else if (manager.GetStep(subTask, step).IsCompeleted())
            {
                gameObject.SetActive(false);
                return;
            }

        }
        cylinder.SetActive(true);
        cylinderGlow.SetActive(true);
    }


}




