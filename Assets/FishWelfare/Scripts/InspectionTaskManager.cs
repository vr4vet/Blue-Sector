using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tablet;
using BNG;

public class ActivityManager : MonoBehaviour
{

    public TaskHolder taskHolder;
    // Start is called before the first frame update
    void Start()
    {
        taskHolder = GameObject.FindObjectOfType<TaskHolder>();
        Debug.Log("Lets go");
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
}
