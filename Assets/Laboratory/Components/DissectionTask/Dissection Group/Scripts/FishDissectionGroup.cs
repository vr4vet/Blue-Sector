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
    public UnityEvent m_OnSecondCutComplete;
    public UnityEvent m_OnThirdCutComplete;
    public UnityEvent m_OnTaskReset;

    // Start is called before the first frame update
    void Start()
    {
        m_OnSalmonEntered ??= new();
        m_OnFirstCutComplete ??= new();
        m_OnSecondCutComplete ??= new();
        m_OnThirdCutComplete ??= new();
        m_OnTaskReset ??= new();

        // add listener to invidual dissection steps completion, and hide/disable them
        foreach (DissectionStep step in dissectionSteps)
        {
            step.m_DissectionStateFinished.AddListener(ProgressToNextDissectionStep);
            step.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // set up the first dissection step if the salmon is placed in trigger (this is set in inspector)
        if (other.CompareTag("Bone"))
        {
            other.transform.root.gameObject.SetActive(false);
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

    public void SetUpDissectionStep(int stepIndex)
    {
        foreach (DissectionStep step in dissectionSteps)
        {
            if (dissectionSteps.IndexOf(step) == stepIndex)
                step.gameObject.SetActive(true);
            else
                step.gameObject.SetActive(false);
        }

        switch (stepIndex)
        {
            case 1:
                m_OnFirstCutComplete.Invoke(); break;
            case 2:
                m_OnSecondCutComplete.Invoke(); break;
            case 3:
                m_OnThirdCutComplete.Invoke(); break;
        }
    }

    public void ResetDissectionGroup()
    {
        _currentDisectionStep = 0;
        
        foreach (DissectionStep step in dissectionSteps)
            step.gameObject.SetActive(false);
        
        m_OnTaskReset.Invoke();
    }

    private void OnDestroy()
    {
        foreach (DissectionStep state in dissectionSteps)
            state.m_DissectionStateFinished.RemoveListener(ProgressToNextDissectionStep);
    }
}
