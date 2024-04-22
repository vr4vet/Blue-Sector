using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerExitTeleportationAnchor : MonoBehaviour
{
    public GameObject equipmentArrow;
    public GameObject cageArrows;
    public GameObject videoObject;
    [SerializeField] private GameObject maintenanceManager;

    [SerializeField] private GameObject anchorArrow;
    private MaintenanceManager manager;
    private FeedbackManager feedbackManager;
    private bool activeArrow = false;
    public Task.Subtask subtask;
    [SerializeField] private string stepName;
    private Task.Step step;


    void Start()
    {
        step = subtask.GetStep(stepName);
        manager = maintenanceManager.GetComponent<MaintenanceManager>();
        feedbackManager = maintenanceManager.GetComponent<FeedbackManager>();
        manager.SubtaskChanged.AddListener(OnSubtaskCompleted);
        if (subtask.SubtaskName != "Hent Utstyr") manager.CurrentSubtask.AddListener(CurrentSubtaskUpdate);
        // activeSubtask = manager.GetSubtask(subTask);

    }

    void CurrentSubtaskUpdate(Task.Subtask currentSub)
    {
        if (currentSub == subtask)
        {

            if ((subtask.GetCompletedSteps() + 1) == (step.getStepNumber()))
            {
                activeArrow = true;


            }
            else
            {
                activeArrow = false;
            }

        }
        else
        {
            activeArrow = false;
        }
        anchorArrow.SetActive(activeArrow);
    }

    public void OnSubtaskCompleted(Task.Subtask completedSubtask)
    {
        if (completedSubtask == subtask)
        {

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
        //if (currentSubtask && currentSubtask.SubtaskName != subTask)
        // {
        //    manager.UpdateCurrentSubtask(manager.GetSubtask(subTask));
        // }

        if (other.CompareTag("Player"))
        {

            anchorArrow.SetActive(false);

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

            anchorArrow.SetActive(activeArrow);
            if (videoObject)
            {
                videoObject.SetActive(false);
            }


            if (subtask.Compleated())
            {
                gameObject.SetActive(false);
                return;
            }
            else if (step.IsCompeleted() && subtask.SubtaskName != "Hent Utstyr")
            {
                gameObject.SetActive(false);
                return;
            }


        }


    }


}




