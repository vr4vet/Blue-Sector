using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExitTeleportationAnchor : MonoBehaviour
{
    public GameObject cylinder;
    public GameObject cylinderGlow;
    public GameObject equipmentArrow;
    public GameObject cageArrows;
    public GameObject videoObject;
    [SerializeField] private GameObject maintenanceManager;
    [SerializeField] private Task.TaskHolder taskHolder;

    [SerializeField] private string subTask;
    [SerializeField] private string step;
    [SerializeField] private GameObject floatingToast;
    [SerializeField] private GameObject anchorArrow;
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
        taskHolder.CurrentSubtask.AddListener(CurrentSubtaskUpdate);
        // activeSubtask = manager.GetSubtask(subTask);

    }

    void CurrentSubtaskUpdate(Task.Subtask currentSub)
    {

        if (currentSub.SubtaskName == subTask)
        {

            if (subTask == "Hent Utstyr")
            {
                equipmentArrow.SetActive(true);

            }
            else if (manager.GetSubtask(subTask).GetCompletedSteps() == (manager.GetStep(subTask, step).getStepNumber() - 1)) ;
            {
                anchorArrow.SetActive(true);
            }



        }
        else
        {
            anchorArrow.SetActive(false);
        }
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
        manager.UpdateCurrentSubtask(manager.GetSubtask(subTask));
        if (other.CompareTag("Player"))
        {

            cylinder.SetActive(false);
            cylinderGlow.SetActive(false);
            anchorArrow.SetActive(false);
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
            anchorArrow.SetActive(true);
            if (videoObject)
            {
                videoObject.SetActive(false);
            }
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




