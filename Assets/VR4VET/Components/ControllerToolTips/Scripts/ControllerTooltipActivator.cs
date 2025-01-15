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

    // lists of button mappings for both Oculus hand controllers
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

        // create a list of mappings between buttons and actions. those with action 'None' are removed
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Grabber"))
        {
            // send the necessary information about this object to ControllerTooltipManager
            if (other.GetComponent<Grabber>().HandSide == ControllerHand.Left)
                _controllerTooltipManager.OnHandEntered(new InterractableObject(transform, _buttonMappingsLeft), ControllerHand.Left);
            else if (other.GetComponent<Grabber>().HandSide == ControllerHand.Right)
                _controllerTooltipManager.OnHandEntered(new InterractableObject(transform, _buttonMappingsRight), ControllerHand.Right);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Equals("Grabber"))
        {
            // send the necessary information about this object to ControllerTooltipManager
            if (other.GetComponent<Grabber>().HandSide == ControllerHand.Left)
                _controllerTooltipManager.OnHandExited(new InterractableObject(transform, _buttonMappingsLeft), ControllerHand.Left);
            else if (other.GetComponent<Grabber>().HandSide == ControllerHand.Right)
                _controllerTooltipManager.OnHandExited(new InterractableObject(transform, _buttonMappingsRight), ControllerHand.Right);
        }
    }
}
