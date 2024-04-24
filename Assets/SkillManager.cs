using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;
    private MaintenanceManager manager;

    private List<int> completedSteps = new List<int>();

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
        manager.BadgeChanged.AddListener(CompleteSkill);


    }


    public void CompleteSkill(Task.Skill skill)
    {
        if (skill.IsLocked())
        {
            skill.Unlock();
            manager.SkillCompleted.Invoke(skill);

        }

    }



}