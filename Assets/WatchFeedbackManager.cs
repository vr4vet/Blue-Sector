using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchFeedbackManager : MonoBehaviour
{
    private AddInstructionsToWatch watch;
    private WatchManager watchManager;
    private int feedbackStep = 0;
    
    [Serializable]
    public struct WatchInfo {
    public string step;
    public List<string> watchFeedback;
    }

    public WatchInfo[] watchInfo;

    void Start()
    {
        watchManager = gameObject.GetComponent<WatchManager>();
        watchManager.SubtaskChanged.AddListener(feedbackOnTaskComplete);

        watch = this.gameObject.GetComponent<AddInstructionsToWatch>();
    }


    public void addFeedback(string subtaskName)
    {
        watch.addInstructions(watchInfo[0].watchFeedback[0]);
        StartCoroutine(moreFeedback(subtaskName));
    }

    public void incrementFeedback()
    {
        feedbackStep++;
        watch.addInstructions(watchInfo[feedbackStep].watchFeedback[0]);
    }

    IEnumerator moreFeedback(string subtaskName)
    {
        yield return new WaitForSeconds(20f);
        if (subtaskName == "Hent Utstyr" && watchManager.stepCount < 2)
        {
            //watch.addInstructions(feedback[subtaskName][1]);
        }
    }

    IEnumerator emergencyFeedback(string subtaskName)
    {
        yield return new WaitForSeconds(40f);
        //watch.addInstructions(feedback[subtaskName][4]);
    }

    public void feedbackOnTaskComplete(Task.Subtask subtask)
    {
        if (subtask.Compleated() /* && (subtask.SubtaskName == "Hent Utstyr" || subtask.SubtaskName == "Håndforing") */)
        {
            //watch.addInstructions(feedback[subtask.SubtaskName][2]);
        }
    }

    public void equipmentFeedback(string subtaskName)
    {
        //watch.addInstructions(feedback[subtaskName][3]);
    }

    public void StopMoreFeedback()
    {
        StopAllCoroutines();
    }

    public void emptyInstructions()
    {
        watch.emptyInstructions();
        StopAllCoroutines();
    }

    public string getText()
    {
        return watch.getText();
    }
}
