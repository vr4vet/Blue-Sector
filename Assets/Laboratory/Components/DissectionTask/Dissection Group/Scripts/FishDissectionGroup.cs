using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class FishDissectionGroup : MonoBehaviour
{
    [SerializeField] private List<DissectionStep> dissectionSteps = new();
    private int _currentDisectionStep = 0;
    public UnityEvent m_OnSalmonEntered;
    public UnityEvent m_OnFirstCutComplete;

    // Start is called before the first frame update
    void Start()
    {
        m_OnSalmonEntered ??= new();
        m_OnFirstCutComplete ??= new();

        // add listener to invidual dissection steps completion, and hide/disable them
        foreach (DissectionStep step in dissectionSteps)
        {
            step.m_DissectionStateFinished.AddListener(ProgressToNextDissectionStep);
            step.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // set up the first dissection step if the salmon is placed in trigger
        if (other.CompareTag("Bone"))
        {
            other.transform.root.gameObject.SetActive(false); // deactivate the salmon object that entered trigger
            SetUpDissectionStep(0);

            m_OnSalmonEntered.Invoke();
        }
    }

    private void ProgressToNextDissectionStep(DissectionStep step)
    {
        // mismatch between whic step was completed and current index in this script
        if (dissectionSteps.IndexOf(step) != _currentDisectionStep)
            Debug.LogError("Dissection step was somehow skipped");

        if (_currentDisectionStep < dissectionSteps.Count)
        {
            _currentDisectionStep++;
            SetUpDissectionStep(_currentDisectionStep);
        }
    }

    private void SetUpDissectionStep(int stepIndex)
    {
        if (stepIndex == 1)
            m_OnFirstCutComplete.Invoke();

        foreach (DissectionStep step in dissectionSteps)
        {
            if (dissectionSteps.IndexOf(step) == stepIndex)
                step.gameObject.SetActive(true);
            else
                step.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        foreach (DissectionStep state in dissectionSteps)
            state.m_DissectionStateFinished.RemoveListener(ProgressToNextDissectionStep);
    }
}
