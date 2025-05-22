using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FishDissectionGroup : MonoBehaviour
{
    [SerializeField] private List<DissectionStep> dissectionSteps = new();
    private int _currentDisectionStep = 0;
    private float _salmonEnteredTime;

    private DialogueBoxController _dialogueBoxController;

    public UnityEvent m_OnSalmonEntered;
    public UnityEvent m_OnFirstCutComplete;
    public UnityEvent m_OnSecondCutComplete;
    public UnityEvent m_OnThirdCutComplete;
    public UnityEvent m_OnTaskReset;
    public UnityEvent m_OnSkillAchieved;

    // Start is called before the first frame update
    void Start()
    {
        m_OnSalmonEntered ??= new();
        m_OnFirstCutComplete ??= new();
        m_OnSecondCutComplete ??= new();
        m_OnThirdCutComplete ??= new();
        m_OnTaskReset ??= new();
        m_OnSkillAchieved ??= new();

        // add listener to invidual dissection steps completion, and hide/disable them
        foreach (DissectionStep step in dissectionSteps)
        {
            step.m_DissectionStateFinished.AddListener(ProgressToNextDissectionStep);
            step.gameObject.SetActive(false);
        }

        if (!(_dialogueBoxController = FindObjectOfType<DialogueBoxController>()))
            Debug.LogError("Could not find DialogueBoxController!");
    }

    private void OnTriggerEnter(Collider other)
    {
        // set up the first dissection step if the salmon is placed in trigger (this is set in inspector)
        if (other.CompareTag("Bone") && _dialogueBoxController.dialogueTreeRestart.name == "DissectionDialogue")
        {
            other.transform.root.gameObject.SetActive(false);
            m_OnSalmonEntered.Invoke();
            _salmonEnteredTime = Time.time; // keep track of time to unlock "speedy surgeon" skill if player dissects fast enough
        }
    }

    private void ProgressToNextDissectionStep(DissectionStep step)
    {
        // mismatch between which step was completed and current index in this script
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
                m_OnThirdCutComplete.Invoke();
                if (Time.time - _salmonEnteredTime < 20) // unlock "speedy surgeon" skill if dissection took less than 20 seconds
                    m_OnSkillAchieved.Invoke();
                break;
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
