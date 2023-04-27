using System;
using System.Collections.Generic;
using System.Linq;
using BNG;
using UnityEngine;
using UnityEngine.Animations;

/// <summary>
/// Helper class for 3D sliders,
/// enabling slider-steps
/// </summary>
public class SliderHelper : GrabbableEvents
{
    [Tooltip("How many steps along the slider do you want it to snap to? Set to 0 to disable snap-points")]
    public int steps;

    [Tooltip("What axis does the slider run along?")]
    public Axis axis;

    [Tooltip("What path does the slider follow?")]
    public GameObject sliderPath;

    [Tooltip("Offset how far the slider can go in each end")]
    public float offset;

    private List<float> snapPositions;
    private float length, interval;
    private Vector3 initialPosition, sliderSize;

    private void Start()
    {
        if (steps == 0) return;

        MeshRenderer renderer = sliderPath.GetComponent<MeshRenderer>();
        sliderSize = renderer.bounds.size;

        switch (axis)
        {
            case Axis.X:
                length = sliderSize.x;
                break;

            case Axis.Y:
                length = sliderSize.y;
                break;

            case Axis.Z:
                length = sliderSize.z;
                break;
        }

        initialPosition = transform.localPosition;
        length -= offset;
        steps--;
        interval = length / (float)steps;

        snapPositions = Enumerable.Range(0, steps).Select(i => { return (float)(i * interval); }).ToList(); // Positive values
        snapPositions.AddRange((from e in snapPositions where e > 0f select (e * -1f)).ToList()); // Negative Values
    }

    /// <summary>
    /// Override of BNG.GrabbableEvents
    /// </summary>
    public override void OnRelease()
    {
        if (steps == 0) return;
        SetPosition(GetPosition());
    }

    /// <summary>
    /// Update Poition of slider knob to closest snap-point along axis
    /// </summary>
    /// <param name="axisPosition">
    /// Current position of the slider along axis
    /// </param>
    private void SetPosition(float axisPosition)
    {
        // Find closest snap point
        float closest = snapPositions.Aggregate((x, y) => Math.Abs(x - axisPosition) < Math.Abs(y - axisPosition) ? x : y);

        switch (axis)
        {
            case Axis.X:
                transform.localPosition = new(
                    closest,
                    initialPosition.y,
                    initialPosition.z
                );
                break;

            case Axis.Y:
                transform.localPosition = new(
                    initialPosition.x,
                    closest,
                    initialPosition.z
                );
                break;

            case Axis.Z:
                transform.localPosition = new(
                    initialPosition.x,
                    initialPosition.y,
                    closest
                );
                break;

            default:
                Debug.LogError("Not a valid axis!");
                break;
        }
    }

    /// <summary>
    /// Get local position of knob along selected axis
    /// </summary>
    /// <returns>
    /// Knob local-location on axis
    /// </returns>
    private float GetPosition()
    {
        float position;

        switch (axis)
        {
            case Axis.X:
                position = transform.localPosition.x;
                break;

            case Axis.Y:
                position = transform.localPosition.y;
                break;

            case Axis.Z:
                position = transform.localPosition.z;
                break;

            default:
                Debug.LogError("Not a valid axis!");
                position = 0f;
                break;
        }

        return position;
    }
}