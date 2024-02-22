
using System.Collections.Generic;
using UnityEngine;

namespace Task
{
    [CreateAssetMenu(fileName = "New Badge", menuName = "Tasks/Badge")]
    public class Badge : ScriptableObject
    {


        [Tooltip("Badge Name")]
        [SerializeField] private string _name;

        [Tooltip("How badge is obtained"), TextArea(5, 20)]
        [SerializeField] private string _instruction;


        [SerializeField] private List<Subtask> _subtasks = new List<Subtask>();


        [SerializeField] private Skill _skill;
        // [SerializeField] public GameObject _icon;


        public string Name { get => _name; set => _name = value; }
        public Skill ConnectedSkill { get => _skill; set => _skill = value; }
        public string Instruction { get => _instruction; set => _instruction = value; }
        public List<Subtask> SubTasks { get => _subtasks; set => _subtasks = value; }
        // public GameObject BadgeIcon { get => _icon; set => _icon = value; }

        private void Awake()
        {
            _skill.ConnectedBadges.Add(this);

        }

        // Temporary, need to decide if badges are unlocked when subtasks are completed or steps. Can for example unlock a badge on the completion of steps in different subtasks, such as communication. 
        public bool IsLocked()
        {
            foreach (Subtask subtask in _subtasks)
            {
                if (!subtask.Compleated())
                {
                    return false;
                }
            }
            return true;
        }



    }



}
