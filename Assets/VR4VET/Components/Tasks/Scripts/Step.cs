/* Developer: Jorge Garcia
 * Ask your questions on github: https://github.com/Jorest
 */

using UnityEngine;

namespace Task
{
    [System.Serializable]
    public class Step
    {
        [SerializeField] private string _stepName;
        [SerializeField][Range(1, 20)] private int _repetionNumber = 1;
        private int stepNumber;

        private bool _compleated = false;
        private bool _currentStep = false;
        private int _repetionsCompleated = 0;
        private Subtask _parentSubtask;
        public Subtask ParentSubtask { get => _parentSubtask; set => _parentSubtask = value; }

        public int RepetionNumber { get => _repetionNumber; set => _repetionNumber = value; }
        public int RepetionsCompleated { get => _repetionsCompleated; set => _repetionsCompleated = value; }
        public string StepName { get => _stepName; set => _stepName = value; }

        public bool CurrentStep { get => _currentStep; set => _currentStep = value; }


        public void CompleateRep()
        {
            if (_repetionsCompleated < _repetionNumber)
            {
                _repetionsCompleated++;
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
        }

        /// Set the name of this aktivitet (Legacy)

        public void SetAktivitetName(string value)
        {
            _stepName = value;
        }
    }
}