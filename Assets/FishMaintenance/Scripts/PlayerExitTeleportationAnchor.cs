using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExitTeleportationAnchor : MonoBehaviour
{
    public GameObject cylinder;
    public GameObject cylinderGlow;
    public GameObject equipmentArrow;
    public GameObject cageArrows;
    [SerializeField] private GameObject maintenanceManager;
    [SerializeField] private string subTask;
    [SerializeField] private string step;
    [SerializeField] private GameObject floatingToast;
    private MaintenanceManager manager;
    private FeedbackManager feedbackManager;


    private BoxCollider boxCollider;
    private Vector3 originalSize;

    private bool subtaskComplete = false;

    void Start()
    {
        manager = maintenanceManager.GetComponent<MaintenanceManager>();
        feedbackManager = maintenanceManager.GetComponent<FeedbackManager>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
        originalSize = boxCollider.size;
        manager.SubtaskChanged.AddListener(OnSubtaskCompleted);
        // activeSubtask = manager.GetSubtask(subTask);

    }

    void OnSubtaskCompleted(Task.Subtask subtask)
    {
        if (subtask.SubtaskName == subTask)
        {
            floatingToast.SetActive(true);
            subtaskComplete = true;
            if (equipmentArrow)
            {
                equipmentArrow.SetActive(false);
            }
            if (cageArrows)
            {
                cageArrows.SetActive(true);
            }
        };
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            cylinder.SetActive(false);
            cylinderGlow.SetActive(false);
            boxCollider.size = new Vector3(1f, 3f, 1f);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (feedbackManager.getText() != "Bra jobba! Gå videre til neste sylinder.")
            {
                feedbackManager.StopMoreFeedback();
                feedbackManager.emptyInstructions();
            }
            cylinder.SetActive(true);
            cylinderGlow.SetActive(true);
            boxCollider.size = originalSize;
            try
            {
                if (subtaskComplete)
                {
                    gameObject.SetActive(false);
                    return;
                }
                else if (manager.GetStep(subTask, step).IsCompeleted())
                {
                    gameObject.SetActive(false);
                    return;
                }
            }
            catch
            {

            }

        }


    }


}




