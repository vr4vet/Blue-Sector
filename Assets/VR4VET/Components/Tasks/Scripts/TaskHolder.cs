/* Developer: Jorge Garcia
 * Ask your questions on github: https://github.com/Jorest
 */

using System.Collections.Generic;
using UnityEngine;

namespace Task
{

    /// <summary>
    /// This script is meant to hold the relevant task data (task and skills) 
    /// this works an interface for the data to be modified on runtime
    /// </summary>
    public class TaskHolder : MonoBehaviour
    {
        public static TaskHolder Instance;
        private List<TaskxTarget> _taskAndTargerts = new List<TaskxTarget>();

        [Header("Profession Tasks")]
        [SerializeField] public List<Skill> skillList = new List<Skill>();

        [SerializeField] public List<Task> taskList = new List<Task>();

        [SerializeField] public List<Badge> badgeList = new List<Badge>();

        //making the task holder a singleton
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

        public Task GetTask(string taskName)
        {
            Task returnTask = null;

            foreach (Task task in taskList)
            {
                if (task.TaskName == taskName)
                {
                    returnTask = task;
                    break;
                }
            }

            return returnTask;
        }

        public Skill GetSkill(string skillName)
        {
            Skill returnSkill = null;

            foreach (Skill skill in skillList)
            {
                if (skill.Name == skillName)
                {
                    returnSkill = skill;
                    break;
                }
            }

            return returnSkill;
        }

        public Badge GetBadge(string badgeName)
        {
            Badge returnBadge = null;

            foreach (Badge badge in badgeList)
            {
                if (badge.Name == badgeName)
                {
                    returnBadge = badge;
                    break;
                }
            }

            return returnBadge;
        }

    }



    //this sub-class can be usefull in the future to set targets 
    [System.Serializable]
    public class TaskxTarget
    {
        public Subtask subtask;
        public GameObject target;
    }
}