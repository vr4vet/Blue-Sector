using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintenanceManager : MonoBehaviour
{
    public static MaintenanceManager Instance;
    [SerializeField] private Task.TaskHolder taskHolder;
    [SerializeField] private Tablet.TaskListLoader1 taskListLoader;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        foreach (Task.Task task in taskHolder.taskList)
        {
            foreach (Task.Subtask sub in task.Subtasks)
            {
                foreach (Task.Step step in sub.StepList)
                {
                    step.Reset();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CompleteStep(string taskName, string subtaskName, string stepName)
    {
        Task.Subtask sub = taskHolder.GetTask(taskName).GetSubtask(subtaskName);

        Task.Step step = taskHolder.GetTask(taskName).GetSubtask(subtaskName).GetStep(stepName);
        step.CompleateRep();
        taskListLoader.SubTaskPageLoader(sub);
        Debug.Log(step.RepetionNumber.ToString() + " completed: " + step.RepetionsCompleated.ToString());

    }
}
