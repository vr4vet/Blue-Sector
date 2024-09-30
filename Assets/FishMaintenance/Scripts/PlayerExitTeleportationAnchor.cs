using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerExitTeleportationAnchor : MonoBehaviour
{
    public GameObject equipmentArrow;
    public GameObject cageArrows;
    public GameObject GuidingHandSplint;
    public GameObject GuidingHandRope;
    public GameObject GuidingHandBucket;
    [SerializeField] private GameObject maintenanceManager;

    [SerializeField] private GameObject anchorArrow;

    [SerializeField] private GameObject cylinder;
    private MaintenanceManager manager;
    private FeedbackManager feedbackManager;
    private bool activeArrow = false;
    public Task.Subtask subtask;
    [SerializeField] private string stepName;
    private Task.Step step;
    private Task.Subtask currentSubtask;
    private bool playerInside = false;


    void Start()
    {
        step = subtask.GetStep(stepName);
        Debug.Log(step == null);
        manager = maintenanceManager.GetComponent<MaintenanceManager>();
        feedbackManager = maintenanceManager.GetComponent<FeedbackManager>();
        manager.SubtaskChanged.AddListener(OnSubtaskCompleted);
        currentSubtask = manager.MaintenanceTask.GetSubtask("Get Equipment");



        if (subtask.SubtaskName != "Get Equipment")
        {
            step = subtask.GetStep(stepName);
            manager.CurrentSubtask.AddListener(CurrentSubtaskUpdate);
            // activeSubtask = manager.GetSubtask(subTask);
        }
    }
    public void CurrentSubtaskUpdate(Task.Subtask currentSub)
    {
        currentSubtask = currentSub;
        Debug.Log(step == null);
        if (step.CurrentStep && !playerInside)
        {

            activeArrow = true;
        }
        else
        {
            activeArrow = false;
        }
        //anchorArrow.SetActive(activeArrow);
        cylinder.SetActive(activeArrow);
    }

    public void OnSubtaskCompleted(Task.Subtask incomingSubtask)
    {
        if ((incomingSubtask == subtask) && incomingSubtask.Compleated())
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

            playerInside = true;
            //anchorArrow.SetActive(false);
            if (currentSubtask != subtask)
            {
                manager.UpdateCurrentSubtask(subtask);
            }
            else if (subtask.SubtaskName == "Get Equipment")
            {
                manager.UpdateCurrentSubtask(subtask);
            }
        }

    }


    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {


            playerInside = false;
            if (feedbackManager.getText() != "Bra jobba! Følg pilene rundt merden.")
            {
                feedbackManager.StopMoreFeedback();
                feedbackManager.emptyInstructions();
            }

            //anchorArrow.SetActive(activeArrow);
            cylinder.SetActive(activeArrow);

            if (GuidingHandSplint)
            {
                GuidingHandSplint.SetActive(false);
            }
            if (GuidingHandRope)
            {
                GuidingHandRope.SetActive(false);
            }
            if (GuidingHandBucket)
            {
                GuidingHandBucket.SetActive(false);
            }
            if (subtask.Compleated())
            {
                gameObject.SetActive(false);
                return;
            }
            else if (subtask.SubtaskName == "Daily Round" && step.IsCompeleted())
            {
                gameObject.SetActive(false);
                return;
            }






        }

    }
}




