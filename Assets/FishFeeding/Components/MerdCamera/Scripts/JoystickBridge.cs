using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public sealed class JoystickBridge : MonoBehaviour
{
    private MerdCameraController merdCameraController;

    [field: SerializeField]
    public GameObject MerdCameraHost { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        merdCameraController = MerdCameraHost.GetComponent<MerdCameraController>();
    }

    public void OnJoystickChanged(Vector2 position)
    {
        if (merdCameraController == null)
        {
            return;
        }

        merdCameraController.Look(new Vector3(position.x, -position.y, 0));
    }

    public void OnLeverChanged(float percentage)
    {
        if (merdCameraController == null)
        {
            return;
        }

        if (TryGetNormalizedPercentage(percentage, out var normalized))
        {
            merdCameraController.Move(normalized);
        }
    }

    public void OnElevatorChanged(float percentage)
    {
        if (merdCameraController == null)
        {
            return;
        }

        if (TryGetNormalizedPercentage(percentage, out var normalized))
        {
            merdCameraController.Elevate(normalized);
        }
    }

    private static bool TryGetNormalizedPercentage(float percentage, out float normalized)
    {
        var position = percentage / 100f;
        normalized = (position - .5f) / .5f;
        const float deadzone = .05f;
        return MathF.Abs(normalized) >= deadzone;
    }
}
