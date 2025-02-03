using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;
    private MaintenanceManager manager;
    private WatchManager watchManager;

    void Start()
    {
        if (gameObject.GetComponent<MaintenanceManager>() != null)
        {
            manager = this.gameObject.GetComponent<MaintenanceManager>();
            manager.BadgeChanged.AddListener(CompleteSkill);
        }
        else if (gameObject.GetComponent<WatchManager>() != null)
        {
            watchManager = this.gameObject.GetComponent<WatchManager>();
            watchManager.BadgeChanged.AddListener(CompleteSkill);
        }


    }

    public void CompleteSkill(Task.Skill skill)
    {
        if (skill.IsLocked())
        {
            skill.Unlock();
            if (manager)
            {
                manager.SkillCompleted.Invoke(skill);
            }
            else if (watchManager)
            {
                watchManager.SkillCompleted.Invoke(skill);
            }

        }

    }

}