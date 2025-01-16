using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used for storing button objects in order to make tooltips point to its assigned button.
/// </summary>
public class ControllerButtonsTransforms
{
    public Transform Oculus, Thumbstick, Primary, Secondary, TriggerFront, TriggerGrip;
}

/// <summary>
/// All the available hand controller buttons on the Quest series.
/// </summary>
public enum ControllerButtons
{
    OculusLeft, ThumbstickLeft, X, Y, TriggerFrontLeft, TriggerGripLeft, OculusRight, ThumbstickRight, A, B, TriggerFrontRight, TriggerGripRight
}

/// <summary>
/// All available button actions.
/// Add more if needed.
/// </summary>
public enum ButtonActions
{
    None, Grab, Click, Start, Open, Teleport, Move, Turn, Pause, Select
}

/// <summary>
/// Record used for creating a mapping between a button and its action.
/// </summary>
/// <param name="Button"></param>
/// <param name="Action"></param>
public record ButtonActionMapping(ControllerButtons Button, ButtonActions Action);

/// <summary>
/// Record used to keep track of currently intersecting objects containing ControllerTooltipActivator script
/// and its related button mapping.
/// </summary>
/// <param name="Object"></param>
/// <param name="ButtonMappings"></param>
public record InterractableObject(Transform Object, List<ButtonActionMapping> ButtonMappings);