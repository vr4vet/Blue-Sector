using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    [SerializeField] private Task.TaskHolder taskHolder;
    private MaintenanceManager manager;

    private List<int> completedSteps = new List<int>();
    [SerializeField] private GameObject tablet;
    private Tablet.TaskListLoader1 taskListLoader => tablet.GetComponent<Tablet.TaskListLoader1>();
    private StaticPanelManager staticPanelManager => tablet.GetComponent<StaticPanelManager>();

    private Task.Task task => taskHolder.GetTask("Vedlikehold");


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
        manager = this.gameObject.GetComponent<MaintenanceManager>();
        Task.Subtask subtask = task.GetSubtask("Runde På Ring");
        manager.BadgeChanged.AddListener(CompleteSkill);

    }


    private void CompleteSkill(Task.Skill skill)
    {
        if (skill.IsLocked())
        {
            skill.Unlock();
            manager.SkillCompleted.Invoke(skill);
            taskListLoader.LoadSkillsPage();
            staticPanelManager.OnClickMenuSkills();
        }

    }



}