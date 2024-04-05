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

    private int maxSteps;


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

        Task.Subtask subtask = taskHolder.GetTask("Vedlikehold").GetSubtask("Runde På Ring");
        maxSteps = subtask.StepList.Count;
        manager.StepCompleted.AddListener(UpdateBadges);

    }

    public void UpdateBadges(Task.Step step)
    {
        if (manager.GetSubtask("Runde På Ring").StepList.Contains(step))
        {
            StepwiseBadge(step.getStepNumber());
        }

    }

    private void CompleteBadge(string skillName, string badgeName)
    {
        Task.Skill skill = taskHolder.GetSkill(skillName);
        Task.Badge badge = skill.GetBadge(badgeName);
        badge.Unlock();
        // if (!skill.ConnectedBadges.Any(b => b.IsLocked()))
        // {
        manager.SkillCompleted.Invoke(skill);
        // }
    }

    public void StepwiseBadge(int stepNumber)
    {
        if (!completedSteps.Contains(stepNumber)) completedSteps.Add(stepNumber);
        if (completedSteps.Count == maxSteps)
        {
            bool isIncremental = completedSteps.Zip(completedSteps.Skip(1), (current, next) => current + 1 == next).All(x => x);
            if (isIncremental)
                CompleteBadge("Problemløsning", "Stegmester");
        }
    }


}

