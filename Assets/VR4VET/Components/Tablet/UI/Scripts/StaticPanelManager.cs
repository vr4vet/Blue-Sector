using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    private List<GameObject> allMenus = new();





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

    void Start()
    {
        allMenus.AddRange(new List<GameObject>() { TaskListMenu, TaskAboutMenu, SubtaskAboutMenu, SkillListMenu });

        foreach (var item in allMenus)
        {
            item.SetActive(false);
        }
        SelectSubtask();


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




    public void OnClickTask()
    {

        SwitchMenuTo(TaskAboutMenu);

    }












    public void SelectSubtask()
    {


        SwitchMenuTo(SubtaskAboutMenu);
    }



}
