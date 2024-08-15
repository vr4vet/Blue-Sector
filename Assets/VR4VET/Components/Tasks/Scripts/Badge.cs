
using System.Collections.Generic;
using UnityEngine;

namespace Task
{

    public class Badge : ScriptableObject
    {


        [Tooltip("Badge Name")]
        [SerializeField] private string _name;

        [Tooltip("How badge is obtained"), TextArea(5, 20)]
        [SerializeField] private string _instruction;




        private Skill _skill;
        [SerializeField] private Sprite _icon;

        private bool locked = true;
        public string Name { get => _name; set => _name = value; }
        public Skill ConnectedSkill { get => _skill; set => _skill = value; }
        public string Instruction { get => _instruction; set => _instruction = value; }

        public Sprite Icon { get => _icon; set => _icon = value; }

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


    }



}
