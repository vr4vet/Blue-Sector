using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;


public class StaticPanelManager : MonoBehaviour
{

    static StaticPanelManager _instance;

    public static StaticPanelManager Instance
    {
        get
        {
            return _instance;
        }
    }


    //references
    [Header("Menu references")]
    [SerializeReference] GameObject TaskListMenu;
    [SerializeReference] GameObject TaskAboutMenu;
    [SerializeReference] GameObject SubtaskAboutMenu;
    [SerializeReference] GameObject SkillListMenu;
    [SerializeReference] GameObject NotificationAlertMenu;
    private AddInstructionsToWatch watch;
    private MaintenanceManager manager;
    private List<GameObject> allMenus = new();

    private bool activeAlert = false;





    //methods below, categorized by the screen they are relevant to
    #region UnityMethods
    void Awake()
    {
        if (StaticPanelManager.Instance != null)
        {
            throw new System.Exception(name + " - FloatingUIManager.Awake() - Tried to initialize duplicate singleton.");
        }
        else
        {
            _instance = this;
        }

    }


    #endregion



    #region Navigation Methods


    void Start()
    {
        allMenus.AddRange(new List<GameObject>() { TaskListMenu, TaskAboutMenu, SubtaskAboutMenu, SkillListMenu });

        foreach (var item in allMenus)
        {
            item.SetActive(false);
        }
        SelectSubtask();
        manager = GameObject.FindObjectsOfType<MaintenanceManager>()[0];
        watch = GameObject.FindObjectsOfType<AddInstructionsToWatch>()[0];
        watch.IncomingMessage.AddListener(SetAlertMenu);
        manager.CurrentSubtask.AddListener(OnCurrentSubtaskChanged);
        manager.SkillCompleted.AddListener(OnSkillCompleted);

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


    public void SetExperienceName(string b)
    {
        // TXT_Task_Exp.text = "Task - " + b;
        // TXT_TaskAbout_Exp.text = "Task - " + b;
        // TXT_Subtask_Exp.text = "Task - " + b;
        // TXT_Skill_Exp.text = "Skills - " + b;
        // TXT_SkillAbout_Exp.text = "Skills - " + b;
        // TXT_Score_Exp.text = "Score - " + b;
        // TXT_Help_Exp.text = "Help - " + b;


        // TXT_TaskAbout_Exp.text = "Oppgaver - " + b;
    }

    void SwitchMenuTo(GameObject b)
    {
        foreach (var item in allMenus)
        {
            item.SetActive(false);
        }
        //run whatever graphical resetting methods here if we do things like expanding/collapsing categories | clear text areas of text here as well
        b.SetActive(true);
    }


    void Update()
    {

    }
    #endregion

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
