using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishDissectionGroup : MonoBehaviour
{
    [SerializeField] private List<DissectionStep> dissectionStates = new();
    private int _currentDisectionStep = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (DissectionStep step in dissectionStates)
        {
            step.m_DissectionStateFinished.AddListener(ProgressToNextDissectionStep);
            step.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bone"))
        {
            other.transform.root.gameObject.SetActive(false);
            SetUpDissectionStep(0);
        }
    }

    private void ProgressToNextDissectionStep(DissectionStep state)
    {
        if (dissectionStates.IndexOf(state) != _currentDisectionStep)
            Debug.LogError("Dissection step was somehow skipped");

        if (_currentDisectionStep < dissectionStates.Count)
        {
            _currentDisectionStep++;
            SetUpDissectionStep(_currentDisectionStep);
        }
    }

    private void SetUpDissectionStep(int stepIndex)
    {
        foreach (DissectionStep state in dissectionStates)
        {
            if (dissectionStates.IndexOf(state) == stepIndex)
                state.gameObject.SetActive(true);
            else
                state.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        foreach (DissectionStep state in dissectionStates)
            state.m_DissectionStateFinished.RemoveListener(ProgressToNextDissectionStep);
    }
}
