using BNG;
using System.Collections.Generic;
using UnityEngine;


public class ControllerTooltipActivator : MonoBehaviour
{
    [Tooltip("The trigger area will be this many times larger than the bounds of the object")]
    [SerializeField] private float TriggerSizeFactor = 1.5f;

    private ControllerTooltipManager _controllerTooltipManager;

    [Header("Left controller buttons")]
    [SerializeField] private ButtonActions OculusLeft;
    [SerializeField] private ButtonActions ThumbstickLeft;
    [SerializeField] private ButtonActions X;
    [SerializeField] private ButtonActions Y;
    [SerializeField] private ButtonActions TriggerFrontLeft;
    [SerializeField] private ButtonActions TriggerGripLeft;

    [Header("Right controller buttons")]
    [SerializeField] private ButtonActions OculusRight;
    [SerializeField] private ButtonActions ThumbstickRight;
    [SerializeField] private ButtonActions A;
    [SerializeField] private ButtonActions B;
    [SerializeField] private ButtonActions TriggerFrontRight;
    [SerializeField] private ButtonActions TriggerGripRight;

    private List<ButtonActionMapping> _buttonMappingsLeft, _buttonMappingsRight;

    // Start is called before the first frame update
    void Start()
    {
        _controllerTooltipManager = GameObject.Find("ControllerToolTipManager").GetComponent<ControllerTooltipManager>();

        // create a trigger collider that is TriggerSizeFactor times larger than the object itself
        BoxCollider collider = gameObject.AddComponent(typeof(BoxCollider)) as BoxCollider;
        collider.isTrigger = true;
        collider.size = transform.parent.GetComponent<MeshFilter>().mesh.bounds.size * TriggerSizeFactor;
        collider.center = transform.parent.GetComponent<MeshFilter>().mesh.bounds.center;

        // create a list of mappings between buttons and actions
        _buttonMappingsLeft = new List<ButtonActionMapping>
        {
            new(ControllerButtons.OculusLeft, OculusLeft),
            new(ControllerButtons.ThumbstickLeft, ThumbstickLeft),
            new(ControllerButtons.X, X),
            new(ControllerButtons.Y, Y),
            new(ControllerButtons.TriggerFrontLeft, TriggerFrontLeft),
            new(ControllerButtons.TriggerGripLeft, TriggerGripLeft),
        };
        _buttonMappingsRight = new List<ButtonActionMapping>
        {
            new(ControllerButtons.OculusRight, OculusRight),
            new(ControllerButtons.ThumbstickRight, ThumbstickRight),
            new(ControllerButtons.A, A),
            new(ControllerButtons.B, B),
            new(ControllerButtons.TriggerFrontRight, TriggerFrontRight),
            new(ControllerButtons.TriggerGripRight, TriggerGripRight)
        };
    }


    private int _handsCnt = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Grabber"))
        {
            if (other.transform.parent.name.Equals("LeftController"))
                _controllerTooltipManager.SetOculusHandModel(_buttonMappingsLeft, ControllerHand.Left);
            else if (other.transform.parent.name.Equals("RightController"))
                _controllerTooltipManager.SetOculusHandModel(_buttonMappingsRight, ControllerHand.Right);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Equals("Grabber"))
        {
            if (other.transform.parent.name.Equals("LeftController"))
                _controllerTooltipManager.SetDefaultHandModel(ControllerHand.Left);
            else if (other.transform.parent.name.Equals("RightController"))
                _controllerTooltipManager.SetDefaultHandModel(ControllerHand.Right);
        }
    }
}
