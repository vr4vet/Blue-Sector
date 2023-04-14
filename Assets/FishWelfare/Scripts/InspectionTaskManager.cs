using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tablet;
using BNG;

public class InspectionTaskManager : MonoBehaviour
{
    private int inspectedFish = 0;
    private int inspectionTarget = 1;
    private TaskHolder taskHolder;
    // Start is called before the first frame update
    void Start()
    {
        taskHolder = GameObject.FindObjectOfType<TaskHolder>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(taskHolder.GetTaskList()[0].activities[0].aktivitetName);
    }

    public void test(string var1)
    {
        Debug.Log(var1);
    }

    public void ProgressInspection() 
    {
        Debug.Log("Progressing task");
        inspectedFish++;
        if(inspectedFish == inspectionTarget){
            taskHolder.GetTaskList()[0].activities[2].AktivitetIsDone(true);
            taskHolder.GetTaskList()[0].activities[2].SetAchievedPoeng(10);
        }
    }
}
