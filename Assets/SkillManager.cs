using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;
    private WatchManager watchManager;

    void Start()
    {
        if (gameObject.GetComponent<WatchManager>() != null)
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
            if (watchManager)
            {
                watchManager.SkillCompleted.Invoke(skill);
            }

        }

    }

}