using BNG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MicroscopeKnob : MonoBehaviour
{
    [SerializeField] private MicroscopeMonitor MicroscopeMonitor;

    [SerializeField]
    private Axis axis;
    private enum Axis
    {
        X, Y
    }

    private float PreviousRotation = 0;

    public void ScrollMicroscope()
    { 
        if (axis == Axis.X)
            MicroscopeMonitor.ScrollX(transform.eulerAngles.y > PreviousRotation);
        else
            MicroscopeMonitor.ScrollY(transform.eulerAngles.y > PreviousRotation);

        PreviousRotation = transform.eulerAngles.y;
    }
}
