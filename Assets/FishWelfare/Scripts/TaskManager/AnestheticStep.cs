using System.Collections;
using System.Collections.Generic;
using Task;
using UnityEngine;

public class AnestheticStep : MonoBehaviour
{
    private TaskHolder _taskHolder;
    private Step _anestheticStep;
    [SerializeField] private TankController _controller;
    // Start is called before the first frame update
    void Start()
    {
        _taskHolder = FindObjectOfType<TaskHolder>();
        _anestheticStep = _taskHolder.GetTask("Inspect the fish").GetSubtask("Anesthetize the fish").GetStep("Pour anesthetic into the water");
        _anestheticStep.SetCompleated(false, false);
        Debug.Log(_anestheticStep.IsCompeleted());
        //_taskHolder.GetTask("Inspect the fish").GetSubtask("Anesthetize the fish").GetStep("Pour anesthetic into the water").SetCompleated(false, false);
        //_anestheticStep.CompleateAllReps();
    }

    // Update is called once per frame
    void Update()
    {
        if (_controller.sedativeConsentration > 0.01f)
        {
            //_taskHolder.GetTask("Inspect the fish").GetSubtask("Anesthetize the fish").GetStep("Pour anesthetic into the water").SetCompleated(true, true);
            _anestheticStep.SetCompleated(true, true);
            /*            _anestheticStep.SetCompleated(true);
                        _anestheticStep.CompleateAllReps();*/
        }

          
    }
}
