/* Developer: Jorge Garcia
 * Ask your questions on github: https://github.com/Jorest
 */

using System;
using UnityEngine;
using TMPro;

namespace Task
{
    [System.Serializable]

    [CreateAssetMenu(fileName = "New Step", menuName = "Tasks/Step")]
    public class Step : ScriptableObject
    {
        [SerializeField] private string _stepName;
        [SerializeField][Range(1, 20)] private int _repetionNumber = 1;
        private int stepNumber;

        [Tooltip(">0 : count down, <0 : no timer, 0 : count up")]
        [SerializeField] private int _timer = -1;

        [Tooltip("Time in seconds before triggering idle notifications (0 = disabled)")]
        [SerializeField] private int _idleTimeoutSeconds = 120; // Default to 2 minutes

        private bool _compleated = false;
        private bool _currentStep = false;
        private int _repetionsCompleated = 0;
        private TimeSpan _counter;
        private Subtask _parentSubtask;
        public Subtask ParentSubtask { get => _parentSubtask; set => _parentSubtask = value; }

        public int RepetionNumber { get => _repetionNumber; set => _repetionNumber = value; }
        public int RepetionsCompleated { get => _repetionsCompleated; set => _repetionsCompleated = value; }
        public string StepName { get => _stepName; set => _stepName = value; }
        public int Timer { get => _timer; set => _timer = value; }
        public TimeSpan Counter { get => _counter; set => _counter = value; }
        public bool CurrentStep { get => _currentStep; set => _currentStep = value; }

        // Property for idle timeout
        public int IdleTimeoutSeconds { get => _idleTimeoutSeconds; set => _idleTimeoutSeconds = value; }

        public void CompleateRep()
        {
            if (_repetionsCompleated < _repetionNumber)
            {
                _repetionsCompleated++;
            }
            if (FindObjectsOfType<Tablet.TaskListLoader1>().Length > 0)
            {
                Tablet.TaskListLoader1 taskLoader = GameObject.FindObjectsOfType<Tablet.TaskListLoader1>()[0];
                taskLoader.updateCheckMarks();
            }
        }

        public int getStepNumber()
        {
            return stepNumber;
        }

        public void setStepNumber(int stepNumber)
        {
            this.stepNumber = stepNumber;
        }

        public float CompleatedPercent()
        {
            float porcent = _repetionsCompleated * 100 / _repetionNumber;
            return porcent;
        }

        public void CompleateAllReps()
        {
            _repetionsCompleated = _repetionNumber;
            Tablet.TaskListLoader1 taskLoader = GameObject.FindObjectsOfType<Tablet.TaskListLoader1>()[0];
            taskLoader.updateCheckMarks();
        }

        public bool IsCompeleted()
        {
            if (_repetionNumber == _repetionsCompleated) _compleated = true;
            return _compleated;
        }

        /// Mark this step as done
        public void SetCompleated(bool value)
        {
            _compleated = value;
            if (FindObjectsOfType<Tablet.TaskListLoader1>().Length > 0)
            {
                Tablet.TaskListLoader1 taskLoader = GameObject.FindObjectsOfType<Tablet.TaskListLoader1>()[0];
                taskLoader.updateCheckMarks();
            }
        }

        public void Reset()
        {
            _compleated = false;
            _repetionsCompleated = 0;
        }

        //overload to compleate reps
        public void SetCompleated(bool value, bool compleateReps)
        {
            _compleated = value;
            if (compleateReps)
            {
                RepetionsCompleated = RepetionNumber;
            }
            Tablet.TaskListLoader1 taskLoader = GameObject.FindObjectsOfType<Tablet.TaskListLoader1>()[0];
            taskLoader.updateCheckMarks();
        }

        /// Set the name of this aktivitet (Legacy)
        public void SetAktivitetName(string value)
        {
            _stepName = value;
        }

        public void StartTimer()
        {
            Tablet.TaskListLoader1 taskLoader = GameObject.FindObjectsOfType<Tablet.TaskListLoader1>()[0];
            taskLoader.startTimer(Timer, this);
        }

        /// <summary>
        /// Gets the idle timeout for this step in seconds
        /// </summary>
        /// <returns>The idle timeout in seconds</returns>
        public int GetIdleTimeoutSeconds()
        {
            return _idleTimeoutSeconds;
        }
    }
}
