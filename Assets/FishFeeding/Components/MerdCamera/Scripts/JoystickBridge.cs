using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public sealed class JoystickBridge : MonoBehaviour
{
    private MerdCameraController merdCameraController;
    private float previousLeverPercentage;

    [field: SerializeField]
    public GameObject MerdCameraHost { get; set; }
    [field: SerializeField]
    public GameObject Lever { get; set; }

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

        //if (float.IsFinite(previousLeverPercentage) && percentage == previousLeverPercentage)
        //{
        //    return;
        //}

        var position = percentage / 100f;
        var normalized = (position - .5f) / .5f;
        const float deadzone = .05f;
        if (MathF.Abs(normalized) < deadzone)
        {
            return;
        }

        merdCameraController.Move(normalized);

        previousLeverPercentage = percentage;
    }
}
