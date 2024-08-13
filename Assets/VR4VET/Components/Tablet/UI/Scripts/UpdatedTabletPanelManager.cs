using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpdatedTabletPanelManager : MonoBehaviour
{
    [SerializeReference] UpdatedTabletTaskListLoader TaskList;
    //references
    [Header("Menu references")]
    [SerializeReference] GameObject TaskListMenu;
    [SerializeReference] GameObject TaskAboutMenu;
    [SerializeReference] GameObject SubtaskAboutMenu;
    [SerializeReference] GameObject SkillListMenu;
    [SerializeReference] GameObject NotificationAlertMenu;

    [SerializeReference] Image SubtaskBackground;
    [SerializeReference] Image TaskBackground;

    [Header("Language selection")]
    [SerializeField] public Language activeLanguage;
    [SerializeReference] private TMP_Text skillsMenuTextSkillPage;
    [SerializeReference] private TMP_Text taskMenuTextSkillPage;
    [SerializeReference] private TMP_Text skillsMenuTextTaskPage;
    [SerializeReference] private TMP_Text taskMenuTextTaskPage;
    [SerializeReference] private TMP_Text skilsHeaderText;
    [SerializeReference] private TMP_Text taskButtonText;
    [SerializeReference] private TMP_Text taskPageHeader;
    [SerializeReference] private TMP_Text subtaskPageHeader;


    private AddInstructionsToWatch watch;
    private MaintenanceManager manager;
    private WatchManager watchManager;
    private List<GameObject> allMenus = new();

    private bool taskPageOpen = false; 
    private bool subtaskPageOpen = false;

    public enum Language
    {
        English,
        Norwegian,
    }

    void Start()
    {
        if (activeLanguage == Language.English)
        {
            skillsMenuTextSkillPage.SetText("Skills");
            taskMenuTextSkillPage.SetText("Tasks");
            skillsMenuTextTaskPage.SetText("Skills");
            taskMenuTextTaskPage.SetText("Tasks");
            skilsHeaderText.SetText("My Skills");
            taskButtonText.SetText("Tasks");
            taskPageHeader.SetText("All tasks");
            subtaskPageHeader.SetText("All subtasks");
        }
        else if (activeLanguage == Language.Norwegian)
        {
            skillsMenuTextSkillPage.SetText("Ferdigheter");
            taskMenuTextSkillPage.SetText("Oppgaver");
            skillsMenuTextTaskPage.SetText("Ferdigheter");
            taskMenuTextTaskPage.SetText("Oppgaver");
            skilsHeaderText.SetText("Mine ferdigheter");
            taskButtonText.SetText("Opg.");
            taskPageHeader.SetText("Alle oppgaver");
            subtaskPageHeader.SetText("Alle deloppgaver");
        }


        allMenus.AddRange(new List<GameObject>() { TaskListMenu, TaskAboutMenu, SubtaskAboutMenu, SkillListMenu });

        foreach (var item in allMenus)
        {
            item.SetActive(false);
        }
        SelectSubtask();

        if (FindObjectsOfType<MaintenanceManager>().Length > 0)
        {
            manager = GameObject.FindObjectsOfType<MaintenanceManager>()[0];
            manager.CurrentSubtask.AddListener(OnCurrentSubtaskChanged);
            manager.SkillCompleted.AddListener(OnSkillCompleted);
        }
        else 
        {
            watchManager = GameObject.FindObjectsOfType<WatchManager>()[0];
            watchManager.CurrentSubtask.AddListener(OnCurrentSubtaskChanged);
            watchManager.SkillCompleted.AddListener(OnSkillCompleted);
        }
        watch = GameObject.FindObjectsOfType<AddInstructionsToWatch>()[0];
        watch.IncomingMessage.AddListener(SetAlertMenu);
    }

    private void OnCurrentSubtaskChanged(Task.Subtask subtask)
    {
        SwitchMenuTo(SubtaskAboutMenu);
    }

    private void OnSkillCompleted(Task.Skill skill)
    {
        SwitchMenuTo(SkillListMenu);
    }

    public void OnClickOpenTasks()
    {
        if (subtaskPageOpen)
        {
            OnClickOpenSubtask();
        }

        if (!taskPageOpen)
        {
            TaskListMenu.SetActive(true);
            TaskBackground.color = Color.blue;
            taskPageOpen = true;
            TaskList.LoadTaskPage();
            return;
        }
        else if (taskPageOpen)
        {
            TaskListMenu.SetActive(false);
            TaskBackground.color = Color.white;
            taskPageOpen = false;
            return; 
        }
    }

    public void OnClickOpenSubtask()
    {
        if (taskPageOpen)
        {
            OnClickOpenTasks();
        }
        if (!subtaskPageOpen)
        {
            TaskAboutMenu.SetActive(true);
            SubtaskBackground.color = Color.blue;
            subtaskPageOpen = true;
            TaskList.SubtaskPageLoader(TaskList.activeTask);
            return;
        }
        else if (subtaskPageOpen)
        {
            TaskAboutMenu.SetActive(false);
            SubtaskBackground.color = Color.white;
            subtaskPageOpen = false;
            return; 
        }
    }

    public void OnClickBackToAboutTask()
    {
        SwitchMenuTo(TaskAboutMenu);
    }
    public void OnClickBackToSkillMenu()
    {
        SwitchMenuTo(SkillListMenu);
    }

    void SwitchMenuTo(GameObject b)
    {
        foreach (var item in allMenus)
        {
            item.SetActive(false);
        }
        b.SetActive(true);
    }

    public void OnClickMenuTask()
    {
        SwitchMenuTo(TaskListMenu);
    }
    public void OnClickMenuSkills()
    {
        if (taskPageOpen)
        {
            OnClickOpenTasks();
        }
        if (subtaskPageOpen)
        {
            OnClickOpenSubtask();
        }
        SwitchMenuTo(SkillListMenu);
    }

    public void SetAlertMenu()
    {
        if (gameObject.activeInHierarchy)
        {
            StopAllCoroutines();
            NotificationAlertMenu.SetActive(true);
            StartCoroutine(sendAlertMenu());
        }
        else
        {
            NotificationAlertMenu.SetActive(false);
        }
    }

    IEnumerator sendAlertMenu()
    {
        yield return new WaitForSeconds(3f);
        NotificationAlertMenu.SetActive(false);
    }

    void OnDisable()
    {
        NotificationAlertMenu.SetActive(false);
    }

    public void SelectSubtask()
    {
        SwitchMenuTo(SubtaskAboutMenu);
    }
}
