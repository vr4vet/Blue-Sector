using BNG;
using System.Collections.Generic;
using UnityEngine;


public class ControllerTooltipActivator : MonoBehaviour
{
    [Tooltip("The initial state of the activator")]
    [SerializeField] private TooltipState InitialState = TooltipState.Active;
    private enum TooltipState
    {
        Active, Inactive
    }

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

    // decides if activator should respond when player enters its trigger
    private bool _active = true;

    // Start is called before the first frame update
    void Start()
    {
        if (InitialState == TooltipState.Inactive)
            _active = false;

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

    /// <summary>
    /// Activate the activator
    /// </summary>
    public void Activate() => _active = true;

    /// <summary>
    /// Deactivate the activator
    /// </summary>
    public void Deactivate() => _active = false;

    public List<ButtonActionMapping> GetButtonMappingsLeft() => _buttonMappingsLeft;

    public List<ButtonActionMapping> GetButtonMappingsRight() => _buttonMappingsRight;
}
