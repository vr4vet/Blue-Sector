using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour
{

    [SerializeField] Color completedColor;
    //ui counterpart for Task class
    Task.Task _task; //the task
    public Task.Task Task
    {
        get
        {
            return _task;
        }
    } 

    [SerializeField] GameObject checkmark;
    [SerializeField] TextMeshProUGUI textBox;
    [SerializeField] Button btn;

    List<SubTaskUI> _subtasks; //this is so we can check for completion and open these

    public void InitializeInterface(Task.Task t)
    {
        _task = t;
        Refresh();
        Debug.Log("Initialized task " + t.TaskName);

    }

    private void OnTriggerEnter(Collider other) {
        // player index finger tips
        if (other.name.Equals("hands_coll:b_r_index3") || other.name.Equals("hands_coll:b_r_index3 (1)")/*other.gameObject.tag == "Finger"*/)
        {           
            UpdatedTabletTaskListLoader tl = FindObjectOfType<UpdatedTabletTaskListLoader>();
            WatchManager manager = FindObjectOfType<WatchManager>();
            manager.Task = _task;
            tl.activeTask = _task;
            tl.LoadTaskPage();
            tl.SubtaskPageLoader(_task);
            tl.StepPageLoader(_task.Subtasks[0]);
        }
    }


    public void OnClickTaskUI()
    {
        //reference DynamicDataManager, set current Task to this, then refresh children and run StaticPanelManager view method.

        DynamicDataDisplayer.Instance.OnClickTask(this);
    }



    public void ToggleChildVisibility()
    {
        foreach (var item in _subtasks)
        {
            item.gameObject.SetActive(true);
        }
    }

    public void Refresh()
    {
        ToggleCheck(_task.Compleated());
    }


    void ToggleCheck(bool b)
    {
        checkmark.SetActive(b);
    }


}
