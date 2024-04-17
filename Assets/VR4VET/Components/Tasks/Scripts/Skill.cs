/*
 * Developer: Jorge Garcia
 * Ask your questions on github: https://github.com/Jorest
 */

using System.Collections.Generic;
using UnityEngine;

namespace Task
{
    [CreateAssetMenu(fileName = "New Skill", menuName = "Tasks/Skill")]
    public class Skill : ScriptableObject
    {

        [SerializeField] private string _name;

        [Tooltip("Description of this skill"), TextArea(5, 20)]
        [SerializeField] private string _description;


        [Tooltip("Description of how to unlock this skill"), TextArea(5, 20)]

        [SerializeField] private string _instructions;

        [SerializeField] private Sprite _icon;

        public string Name { get => _name; set => _name = value; }
        public string Description { get => _description; set => _description = value; }

        public Sprite Icon { get => _icon; set => _icon = value; }
        public string Instructions { get => _instructions; set => _instructions = value; }
        private bool locked = true;


        private void Awake()
        {
        }
        public void Lock()
        {
            locked = true;
        }
        public void Unlock()
        {
            locked = false;
        }


        public bool IsLocked()
        {
            return locked;
        }


        public int GetArchivedPoints()
        {
            return 0;

        }
    }
}
