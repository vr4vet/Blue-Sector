using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskAnchorHandler : MonoBehaviour
{
    [SerializeField] private List<TaskAnchor> taskAnchors = new();
    [SerializeField] private GameObject equipmentArrow;
    [SerializeField] private GameObject cageArrows;
    [SerializeField] private GameObject backToBoatArrow;
    [SerializeField] private Task.Skill efficientSkill;
    [SerializeField] private Task.Skill problemSolverSkill;

    private TaskAnchor _currentTaskAnchor;
    private float _startTime;

    // Start is called before the first frame update
    void Start()
    {
        // setting up first task anchor
        _currentTaskAnchor = taskAnchors[0];
        SetUpTaskAnchor(_currentTaskAnchor);
        _startTime = Time.time;
    }

    public void ActivateNextTaskAnchor()
    {
        int nextTaskAnchor = taskAnchors.IndexOf(_currentTaskAnchor) + 1;
        if (nextTaskAnchor < taskAnchors.Count)
        {
            _currentTaskAnchor = taskAnchors[nextTaskAnchor];
            SetUpTaskAnchor(_currentTaskAnchor);

            if (taskAnchors.IndexOf(_currentTaskAnchor) >= 2 && Time.time - _startTime < 20)
                WatchManager.Instance.invokeBadge(efficientSkill);

            if (taskAnchors.IndexOf(_currentTaskAnchor) == 4)
                WatchManager.Instance.invokeBadge(problemSolverSkill);
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

