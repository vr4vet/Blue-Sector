using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatedTabletPanelManager : MonoBehaviour
{
   


    //references
    [Header("Menu references")]
    [SerializeReference] GameObject TaskListMenu;
    [SerializeReference] GameObject TaskAboutMenu;
    [SerializeReference] GameObject SubtaskAboutMenu;
    [SerializeReference] GameObject SkillListMenu;
    [SerializeReference] GameObject NotificationAlertMenu;
    private AddInstructionsToWatch watch;
    private MaintenanceManager manager;
    private WatchManager watchManager;
    private List<GameObject> allMenus = new();


    void Start()
    {
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

    public void OnClickBackToTasks()
    {
        SwitchMenuTo(TaskListMenu);
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


    void Update()
    {

    }

    public void OnClickMenuTask()
    {
        SwitchMenuTo(TaskListMenu);
    }
    public void OnClickMenuSkills()
    {
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

    public void OnClickTask()
    {
        SwitchMenuTo(TaskAboutMenu);
    }

    public void SelectSubtask()
    {
        SwitchMenuTo(SubtaskAboutMenu);
    }
}