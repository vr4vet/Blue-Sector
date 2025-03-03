using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using UnityEngine.Localization;

public class UpdatedTabletTaskListLoader : MonoBehaviour
{
    private List<Task.Task> _tasks = new List<Task.Task>();
    private List<Task.Skill> _skills = new List<Task.Skill>();

    private UpdatedTabletPanelManager panelManager;
    private MaintenanceManager manager;
    private WatchManager watchManager;
    //main pages

    [Header("Main Page Canvas")]
    public GameObject tasksListCanvas;

    public GameObject subtaskPageCanvas;
    public GameObject TaskPageCanvas;
    public GameObject skillsListPageCanvas;

    //parents objects to load the buttons in
    [Header("Content Spaces")]
    public GameObject taskContent;

    public GameObject TaskSubtaskContent;
    public GameObject skillContent;
    [SerializeField] private GameObject _subtaskContent;
    [SerializeField] private GameObject _skillSubtaskContent;


    [Header("subtask other")]
    [SerializeField] private TMP_Text _subtaskNameTab;



    [Header("Experience Name")]
    [SerializeField] private string Exp_Name;

    [Header("UI Prefabs")]
    [SerializeField] private GameObject _stepListEntry;
    [SerializeField] private GameObject _subtaskListEntry;
    [SerializeField] private GameObject _taskListEntry;
    [SerializeField] private GameObject _skillBadgesList;
    [SerializeField] private GameObject _horizontalSkill;
    [SerializeField] private GameObject _simpleSkill;

    [Header("Additional Events")]
    [SerializeField] private UnityEvent _skillPageOpen;
    [SerializeField] private UnityEvent _tasksListOpen;
    [SerializeField] private UnityEvent _taskPageOpen;
    [SerializeField] private UnityEvent _subtaskPageOpen;

    [Header("StatusBar Elements")]
    [SerializeField] private GameObject StatusBar;
    [SerializeField] private TMP_Text ProgressText;

    private List<GameObject> _skillsClones = new List<GameObject>();
    private Vector2 originalTransform;
    public Task.Task activeTask;
    private Task.Subtask activeSubtask;

    private void Start()
    {
        //setting loading the scriptable objects
        Task.TaskHolder th = GameObject.FindObjectsOfType<Task.TaskHolder>()[0];
        _tasks = th.taskList;
        _skills = th.skillList;
        activeTask = _tasks[0];
        watchManager = WatchManager.Instance;
        if (FindObjectsOfType<MaintenanceManager>().Length > 0)
        {
            manager = GameObject.FindObjectsOfType<MaintenanceManager>()[0];
            manager.CurrentSubtask.AddListener(HandleCurrentSubtask);
            manager.SkillCompleted.AddListener(HandleSkillUnlocked);
        }
        else
        {
            watchManager.CurrentSubtask.AddListener(HandleCurrentSubtask);
            watchManager.SkillCompleted.AddListener(HandleSkillUnlocked);
        }
        panelManager = gameObject.GetComponent<UpdatedTabletPanelManager>();
        
        originalTransform = StatusBar.GetComponent<RectTransform>().sizeDelta;

        LoadSkillsPage();
        StepPageLoader(_tasks[0].Subtasks[0]);

        LocalizationSettings.SelectedLocaleChanged += OnSelectedLocaleChanged;
    }
    public void UpdateProgressBar(Task.Task task)
    {
        RectTransform barTransform = StatusBar.GetComponent<RectTransform>();
        float taskProgress = task.GetProgress() / task.Subtasks.Count * originalTransform.x;
        barTransform.sizeDelta = new Vector2(taskProgress, originalTransform.y);
        ProgressText.text = Mathf.Round(task.GetProgress()) + "/" + task.Subtasks.Count;
    }

    private void HandleCurrentSubtask(Task.Subtask subtask)
    {
        //if the subtask is not null, load the subtask page
        if (subtask != null)
        {
            StepPageLoader(subtask);
        }
    }
    private void HandleSkillUnlocked(Task.Skill skill)
    {
        LoadSkillsPage();
    }

    public void LoadSkillsPage()
    {
        if (_skillPageOpen != null) _skillPageOpen.Invoke();
        GameObject horizontal1 = skillContent.transform.Find("SkillHorizontal").gameObject;
        GameObject horizontal2 = skillContent.transform.Find("SkillHorizontal2").gameObject;
        foreach (Transform child in horizontal1.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in horizontal2.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        //loads each skill on the parent object
        Task.TaskHolder th = GameObject.FindObjectsOfType<Task.TaskHolder>()[0];

        for (int i = 0; i < th.skillList.Count; i++)
        {
            GameObject horizontalParent = horizontal1;

            if (i > 1)
            {
                horizontalParent = horizontal2;
            }

            Task.Skill skill = th.skillList[i];
            GameObject skillBadgesContent = Instantiate(_simpleSkill, Vector3.zero, Quaternion.identity);
            // Add the horizontal list to vertical content list
            skillBadgesContent.transform.SetParent(horizontalParent.transform);
            skillBadgesContent.transform.localPosition = Vector3.zero;
            skillBadgesContent.transform.localScale = Vector3.one;
            skillBadgesContent.transform.localRotation = Quaternion.Euler(0, 0, 0);

            // Set title to be name of this skill
            TMP_Text skillName = skillBadgesContent.transform.Find("Txt_SkillName").GetComponent<TMP_Text>();
            skillName.text = LocalizeName(skill);

            // Find Badge Image and replace with the Icon for this skill
            GameObject badgeItem = skillBadgesContent.transform.Find("BadgeItem").gameObject;
            UnityEngine.UI.Image buttonIcon = badgeItem.transform.Find("icon_badge").GetComponent<UnityEngine.UI.Image>();
            buttonIcon.sprite = skill.Icon;

            // Set icon with shader and padlock if badge is locked, if unlocked set green badge active
            GameObject padlock = badgeItem.transform.Find("padlock").gameObject;
            GameObject completedBackground = badgeItem.transform.Find("CompletedBackground").gameObject;
            padlock.SetActive(skill.IsLocked());
            completedBackground.SetActive(!skill.IsLocked());
        }
        UpdateProgressBar(activeTask);
    }
    
    //gets called on Start since the list of task is always the same
    public void LoadTaskPage()
    {
        if (_tasksListOpen != null) _tasksListOpen.Invoke();

        Task.TaskHolder th = GameObject.FindObjectsOfType<Task.TaskHolder>()[0];
        _tasks = th.taskList;

        //cleaning list before loading the new subtasks
        foreach (Transform child in taskContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        //loads each task on the parent object
        foreach (Task.Task task in _tasks)
        {
            //task for the list
            GameObject item = Instantiate(_taskListEntry, Vector3.zero, Quaternion.identity);
            item.transform.SetParent(taskContent.transform);
            item.transform.localPosition = Vector3.zero;
            item.transform.localScale = new Vector3 (0.6f, 0.6f, 0.6f);
            item.transform.localRotation = Quaternion.Euler(0, 0, 0);
            
            TaskUI taskUI = item.transform.GetComponentInChildren<TaskUI>();
            taskUI.InitializeInterface(task);
            TMP_Text caption = item.transform.Find("txt_TaskNr").GetComponent<TMP_Text>();
            caption.text = LocalizeName(task);

            Button button = item.transform.Find("btn_Task").GetComponent<Button>();
            GameObject completedButton = item.transform.Find("btn_TaskComplete").gameObject;
            GameObject checkmark = item.transform.Find("img_Checkmark").gameObject;
            if (task == activeTask)
            {
                button.GetComponent<Image>().color = new Color32(0x1C, 0x45, 0x6E, 0xFF);//Color.blue;
            }
            if (task.Compleated())
            {
                checkmark.SetActive(true);
                completedButton.SetActive(true);
            }

            button.onClick.AddListener(() => SubtaskPageLoader(activeTask));
        }
    }

    public void SubtaskPageLoader(Task.Task task)
    {
        //for extra events
        if (_taskPageOpen != null) _taskPageOpen.Invoke();

        //cleaning list before loading the new subtasks
        foreach (Transform child in TaskSubtaskContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Task.Subtask sub in task.Subtasks)
        {
            //task for the list
            GameObject item = Instantiate(_subtaskListEntry, Vector3.zero, Quaternion.identity);
            item.transform.SetParent(TaskSubtaskContent.transform);
            item.transform.localPosition = Vector3.zero;
            item.transform.localScale = new Vector3 (0.6f, 0.6f, 0.6f);
            item.transform.localRotation = Quaternion.Euler(0, 0, 0);

            TMP_Text caption = item.transform.Find("txt_SubTaskNr").GetComponent<TMP_Text>();
            // GameObject points = item.transform.Find("PointText").gameObject; points for later
            caption.text = LocalizeName(sub);

            Button button = item.transform.Find("btn_SubTask").GetComponent<Button>();
            GameObject completedButton = item.transform.Find("btn_SubTaskComplete").gameObject;
            GameObject checkmark = item.transform.Find("img_Checkmark").gameObject;
            if (sub == activeSubtask)
            {
                button.GetComponent<Image>().color = new Color32(0x1C, 0x45, 0x6E, 0xFF);//Color.blue;
            }
            if (sub.Compleated())
            {
                checkmark.SetActive(true);
                completedButton.SetActive(true);
            }
            button.onClick.AddListener(() => StepPageLoader(sub));
        }
        watchManager.UIChanged.Invoke();
    }

    public void StepPageLoader(Task.Subtask subtask)
    {
        if (_subtaskPageOpen != null) _subtaskPageOpen.Invoke();
        TaskPageCanvas.SetActive(false);
        subtaskPageCanvas.SetActive(true);
        activeSubtask = subtask;
        SubtaskPageLoader(activeTask);

        _subtaskNameTab.GetComponent<TMP_Text>().text = LocalizeName(subtask);
        UpdateProgressBar(activeTask);

        //cleaning list before loading the new subtasks
        foreach (Transform child in _subtaskContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        // int stepNumber = 1;
        foreach (Task.Step step in subtask.StepList)
        {
            GameObject item = Instantiate(_stepListEntry, Vector3.zero, Quaternion.identity);
            item.transform.SetParent(_subtaskContent.transform);
            item.transform.localPosition = Vector3.zero;
            item.transform.localScale = Vector3.one;
            item.transform.localRotation = Quaternion.Euler(0, 0, 0);


            if (step.IsCompeleted())
            {
                GameObject checkCircle = item.transform.Find("Checkcircle").gameObject;
                checkCircle.SetActive(true);
            }
            TMP_Text caption = item.transform.Find("txt_Desc").GetComponent<TMP_Text>();
            caption.text = LocalizeName(step);

            TMP_Text reps = item.transform.Find("txt_repetitionNr").GetComponent<TMP_Text>();
            if (step.RepetionNumber > 1) reps.text = step.RepetionsCompleated + "/" + step.RepetionNumber;

            TMP_Text number = item.transform.Find("txt_SubTaskNr").GetComponent<TMP_Text>();
            number.text = step.getStepNumber() + "";
        }
        watchManager.UIChanged.Invoke();
    }

    /// <summary>
    /// Checks if the provided object is a Task, Subtask, Step, or Skill,
    /// and returns the correct localized(translated) version from the correct string table.
    /// The name is used as the key when looking up the localized value.
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    private string LocalizeName(object o)
    {
        StringTable stringTable = null;
        string name = "";
        if (o is Task.Task task)
        {
            if (!watchManager.taskHolder.LocalizedStringTableTasks.IsEmpty)
                stringTable = watchManager.taskHolder.LocalizedStringTableTasks.GetTable();

            name = task.TaskName;
        }
        else if (o is Task.Subtask subtask)
        {
            if (!watchManager.taskHolder.LocalizedStringTableSubtasks.IsEmpty)
                stringTable = watchManager.taskHolder.LocalizedStringTableSubtasks.GetTable();

            name = subtask.SubtaskName;
        }
        else if (o is Task.Step step)
        {
            if (!watchManager.taskHolder.LocalizedStringTableSteps.IsEmpty)
                stringTable = watchManager.taskHolder.LocalizedStringTableSteps.GetTable();

            name = step.StepName;
        }
        else if (o is Task.Skill skill)
        {
            if (!watchManager.taskHolder.LocalizedStringTableSkills.IsEmpty)
                stringTable = watchManager.taskHolder.LocalizedStringTableSkills.GetTable();

            name = skill.Name;
        }
        else
            Debug.LogError("Provided object is not of type Task, Subtask, Step, or Skill!");

        return (stringTable && stringTable[name].LocalizedValue != null) ? stringTable[name].LocalizedValue : name;
    }

    private void OnSelectedLocaleChanged(Locale locale)
    {
        StepPageLoader(activeSubtask);
        LoadSkillsPage();
    }

    private void OnDestroy()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnSelectedLocaleChanged;
    }
}
