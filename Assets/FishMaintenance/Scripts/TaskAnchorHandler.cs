using System.Collections.Generic;
using UnityEngine;

public class TaskAnchorHandler : MonoBehaviour
{
    [SerializeField] private List<TaskAnchor> taskAnchors = new();
    [SerializeField] private GameObject equipmentArrow;
    [SerializeField] private GameObject cageArrows;
    [SerializeField] private GameObject backToBoatArrow;

    private WatchManager _watchManager;
    private TaskAnchor _currentTaskAnchor;

    // Start is called before the first frame update
    void Start()
    {
        _watchManager = WatchManager.Instance;
        _watchManager.SubtaskChanged.AddListener(CurrentSubtaskUpdate);
        
        // setting up first task anchor
        _currentTaskAnchor = taskAnchors[0];
        SetUpTaskAnchor(_currentTaskAnchor);
    }

    public void CurrentSubtaskUpdate(Task.Subtask currentSub)
    {
        if (currentSub != null)
            Debug.Log(currentSub.SubtaskName);

        bool allStepsCompleted = true;
        foreach (Task.Step step in _currentTaskAnchor.Steps)
        {
            if (!step.IsCompeleted())
                allStepsCompleted = false;
        }

        if (allStepsCompleted)
            ActivateNextTaskAnchor();
    }

    private void ActivateNextTaskAnchor()
    {
        int nextTaskAnchor = taskAnchors.IndexOf(_currentTaskAnchor) + 1;
        if (nextTaskAnchor < taskAnchors.Count)
        {
            _currentTaskAnchor = taskAnchors[nextTaskAnchor];
            SetUpTaskAnchor(_currentTaskAnchor);
        }
    }

    private void SetUpTaskAnchor(TaskAnchor taskAnchor)
    {
        foreach (TaskAnchor anchor in taskAnchors)
        {
            if (anchor == taskAnchor)
            {
                anchor.gameObject.SetActive(true);
                cageArrows.SetActive(anchor.RequiresCageArrows);
                equipmentArrow.SetActive(anchor.RequiresEquipmentArrow);
                backToBoatArrow.SetActive(anchor.RequiresBackToBoatArrow);
            }
            else
                anchor.gameObject.SetActive(false);
        }
    }
}

