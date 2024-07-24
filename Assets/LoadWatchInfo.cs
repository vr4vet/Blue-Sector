using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadWatchInfo : MonoBehaviour
{
    private MaintenanceManager manager;
    [SerializeField] public WatchManager watchManager;
    private AddInstructionsToWatch watch;
    private Task.TaskHolder taskHolder;

    // [Header("Canvas Elements")]
    // [SerializeField] private GameObject subtaskCanvas;
    // // [SerializeField] private GameObject statusCanvas;

    [Header("Subtask Elements")]
    [SerializeField] private GameObject subtaskContent;
    [SerializeField] private TMP_Text subtaskTitle;



    [Header("StatusBar Elements")]
    [SerializeField] private GameObject StatusBar;
    [SerializeField] private TMP_Text ProgressText;
    [SerializeField] private TMP_Text StatusText;

    [Header("Skill Elements")]
    [SerializeField] private GameObject skillContent;


    [Header("Prefab Entries")]
    [SerializeField] private GameObject StepItem;
    [SerializeField] private GameObject SkillItem;
    private Vector2 originalTransform;


    private float progress = 0;

    public static LoadWatchInfo Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

    }
    private void Start()
    {
        watch = GameObject.FindObjectsOfType<AddInstructionsToWatch>()[0];
        taskHolder = GameObject.FindObjectsOfType<Task.TaskHolder>()[0];

        if (FindObjectsOfType<MaintenanceManager>().Length > 0)
        {
            manager = GameObject.FindObjectsOfType<MaintenanceManager>()[0];
            manager.SkillCompleted.AddListener(HandleSkillUnlocked);
            manager.CurrentSubtask.AddListener(HandleCurrentSubtask);
            HandleCurrentSubtask(manager.MaintenanceTask.GetSubtask("Hent Utstyr"));
        }
        else
        {
            watchManager = GameObject.FindObjectsOfType<WatchManager>()[0];
            watchManager.SkillCompleted.AddListener(HandleSkillUnlocked);
            watchManager.CurrentSubtask.AddListener(HandleCurrentSubtask);
            
            HandleCurrentSubtask(watchManager.Task.Subtasks[0]);
        }

        originalTransform = StatusBar.GetComponent<RectTransform>().sizeDelta;

        // For å fikse bug, bør endres i Maintenancemanager for å oppdatere på start.
    }

    private void HandleCurrentSubtask(Task.Subtask subtask)
    {
        //if the subtask is not null, load the subtask page
        if (subtask != null)
        {
            LoadSubtasks(subtask);
            UpdateProgressBar(subtask.ParentTask);
        }
    }
    private void HandleSkillUnlocked(Task.Skill skill)
    {


        GameObject SkillEntry = Instantiate(SkillItem, Vector3.zero, Quaternion.identity);

        SkillEntry.transform.SetParent(skillContent.transform);
        SkillEntry.transform.localPosition = Vector3.zero;
        SkillEntry.transform.localScale = Vector3.one;
        SkillEntry.transform.localRotation = Quaternion.Euler(0, 0, 0);

        // Set title to be name of this skill
        TMP_Text skillName = SkillEntry.transform.Find("SkillName").GetComponent<TMP_Text>();
        skillName.text = skill.Name;

    }





    public void UpdateProgressBar(Task.Task task)
    {


        RectTransform barTransform = StatusBar.GetComponent<RectTransform>();
        float taskProgress = task.GetProgress() / task.Subtasks.Count * originalTransform.x;
        barTransform.sizeDelta = new Vector2(taskProgress, originalTransform.y);

        // Det bør flyttes et annet sted hvis det skal brukes
        ProgressText.text = Mathf.Round(task.GetProgress()) + "/" + task.Subtasks.Count;
        string userStatus = "Lærling";
        if (task.GetProgress() == task.Subtasks.Count)
            userStatus = "Sjef";
        else if (task.GetProgress() > Mathf.Round(task.Subtasks.Count / 2))
            userStatus = "Månedens Ansatt";
        else if (task.GetProgress() > Mathf.Round(task.Subtasks.Count / 3))
            userStatus = "Fagarbeider";
        StatusText.text = userStatus;

    }
    public void LoadSubtasks(Task.Subtask subtask)
    {

        subtaskTitle.GetComponent<TMP_Text>().text = subtask.SubtaskName;


        //cleaning list before loading the new subtasks
        foreach (Transform child in subtaskContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Task.Step step in subtask.StepList)
        {
            GameObject item = Instantiate(StepItem, Vector3.zero, Quaternion.identity);
            item.transform.SetParent(subtaskContent.transform);
            item.transform.localPosition = Vector3.zero;
            item.transform.localScale = Vector3.one;
            item.transform.localRotation = Quaternion.Euler(0, 0, 0);


            if (step.IsCompeleted())
            {
                GameObject checkCircle = item.transform.Find("Checkcircle").gameObject;
                checkCircle.SetActive(true);
            };
            TMP_Text caption = item.transform.Find("txt_Desc").GetComponent<TMP_Text>();
            caption.text = step.StepName;

            TMP_Text reps = item.transform.Find("txt_repetitionNr").GetComponent<TMP_Text>();
            if (step.RepetionNumber > 1) reps.text = step.RepetionsCompleated + "/" + step.RepetionNumber;

            TMP_Text number = item.transform.Find("txt_SubTaskNr").GetComponent<TMP_Text>();
            number.text = step.getStepNumber() + "";
        }

    }
}
