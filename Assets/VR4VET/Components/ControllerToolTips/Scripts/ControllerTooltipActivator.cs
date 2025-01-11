using System.Collections.Generic;
using UnityEngine;

public class ControllerTooltipActivator : MonoBehaviour
{
    [Tooltip("The trigger area will be this many times larger than the bounds of the object")]
    [SerializeField] private float TriggerSizeFactor = 1.5f;

    private ControllerTooltipManager _controllerTooltipManager;

    [Header("Left controller buttons")]
    [SerializeField] private ControllerTooltipManager.ButtonFunctions OculusLeft;
    [SerializeField] private ControllerTooltipManager.ButtonFunctions ThumbstickLeft;
    [SerializeField] private ControllerTooltipManager.ButtonFunctions X;
    [SerializeField] private ControllerTooltipManager.ButtonFunctions Y;
    [SerializeField] private ControllerTooltipManager.ButtonFunctions TriggerFrontLeft;
    [SerializeField] private ControllerTooltipManager.ButtonFunctions TriggerGripLeft;

    [Header("Left controller buttons")]
    [SerializeField] private ControllerTooltipManager.ButtonFunctions OculusRight;
    [SerializeField] private ControllerTooltipManager.ButtonFunctions ThumbstickRight;
    [SerializeField] private ControllerTooltipManager.ButtonFunctions A;
    [SerializeField] private ControllerTooltipManager.ButtonFunctions B;
    [SerializeField] private ControllerTooltipManager.ButtonFunctions TriggerFrontRight;
    [SerializeField] private ControllerTooltipManager.ButtonFunctions TriggerGripRight;

    private List<ControllerTooltipManager.ButtonMapping> _buttonMappings;

    // Start is called before the first frame update
    void Start()
    {
        _controllerTooltipManager = GameObject.Find("ControllerToolTipManager").GetComponent<ControllerTooltipManager>();
        BoxCollider collider = gameObject.AddComponent(typeof(BoxCollider)) as BoxCollider;
        collider.isTrigger = true;
        collider.size = transform.parent.GetComponent<MeshFilter>().mesh.bounds.size * TriggerSizeFactor;
        collider.center = transform.parent.GetComponent<MeshFilter>().mesh.bounds.center;

        // setting up list of buttons and their individual function to send to ControllerTooltipManager.cs
        _buttonMappings = new List<ControllerTooltipManager.ButtonMapping>
        {
            new(ControllerTooltipManager.ControllerButtons.OculusLeft, OculusLeft),
            new(ControllerTooltipManager.ControllerButtons.ThumbstickLeft, ThumbstickLeft),
            new(ControllerTooltipManager.ControllerButtons.X, X),
            new(ControllerTooltipManager.ControllerButtons.Y, Y),
            new(ControllerTooltipManager.ControllerButtons.TriggerFrontLeft, TriggerFrontLeft),
            new(ControllerTooltipManager.ControllerButtons.TriggerGripLeft, TriggerGripLeft),
            new(ControllerTooltipManager.ControllerButtons.OculusRight, OculusRight),
            new(ControllerTooltipManager.ControllerButtons.ThumbstickRight, ThumbstickRight),
            new(ControllerTooltipManager.ControllerButtons.A, A),
            new(ControllerTooltipManager.ControllerButtons.B, B),
            new(ControllerTooltipManager.ControllerButtons.TriggerFrontRight, TriggerFrontRight),
            new(ControllerTooltipManager.ControllerButtons.TriggerGripRight, TriggerGripRight)
        };


    }


    private int _handsCnt = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Grabber"))
        {
            if (other.transform.parent.name.Equals("LeftController"))
                Debug.Log("Left hand entered");
            else if (other.transform.parent.name.Equals("RightController"))
                Debug.Log("Right hand entered");

            if (!_controllerTooltipManager.OculusModelsActive())
                _controllerTooltipManager.SetOculusHandModels(_buttonMappings);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Equals("Grabber"))
        {
            if (other.transform.parent.name.Equals("LeftController"))
                Debug.Log("Left hand exited");
            else if (other.transform.parent.name.Equals("RightController"))
                Debug.Log("Right hand exited");
        
            if (_controllerTooltipManager.OculusModelsActive())
                _controllerTooltipManager.SetDefaultHandModels();
        }

    }
}
