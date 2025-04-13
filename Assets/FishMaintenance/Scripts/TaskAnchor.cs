using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TaskAnchor : MonoBehaviour
{
    public List<Task.Step> Steps = new();
    public bool RequiresCageArrows = true;
    public bool RequiresEquipmentArrow = false;
    public bool RequiresBackToBoatArrow = false;
    public bool RequiresPlayerExitToContinue = false;

    private WatchManager _watchManager;
    private TaskAnchorHandler _anchorHandler;
    private bool _playerExited = false;
    private bool _waitingForExit = false;

    public UnityEvent m_OnTaskAnchorComplete; 

    private void Start()
    {
        _watchManager = WatchManager.Instance;
        _watchManager.SubtaskChanged.AddListener(CurrentSubtaskUpdate);

        _anchorHandler = GetComponentInParent<TaskAnchorHandler>();
    }

    /// <summary>
    /// Check whether this task anchor has all its steps completed and tell TaskAnchorHandler to move on to next task anchor if that is the case
    /// </summary>
    /// <param name="currentSub"></param>
    private void CurrentSubtaskUpdate(Task.Subtask currentSub)
    {
        bool allStepsCompleted = true;
        foreach (Task.Step step in Steps)
        {
            if (!step.IsCompeleted())
                allStepsCompleted = false;
        }

        if (allStepsCompleted)
        {
            if (!RequiresPlayerExitToContinue)
                _anchorHandler.ActivateNextTaskAnchor();
            else
                StartCoroutine(nameof(WaitBeforeActivatingNextTaskAnchor));

            m_OnTaskAnchorComplete.Invoke();
        }
    }

    /// <summary>
    /// Wait for player to exit collider before setting up next task anchor
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitBeforeActivatingNextTaskAnchor()
    {
        _waitingForExit = true;
        while (!_playerExited)
            yield return null;

        _anchorHandler.ActivateNextTaskAnchor();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_waitingForExit)
                _playerExited = true;
        }
    }

    private void OnDisable()
    {
        _watchManager.SubtaskChanged.RemoveListener(CurrentSubtaskUpdate);
    }
}
