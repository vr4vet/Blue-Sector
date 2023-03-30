#nullable enable
using BNG;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public sealed class MerdCameraController : MonoBehaviour
{
    private const float DefaultTrackLength = 40;
    private static readonly (Vector3 Min, Vector3 Max) DefaultTrackRange = (default, Vector3.forward * DefaultTrackLength);

    private (Vector3 Min, Vector3 Max) localTrackRange = DefaultTrackRange;
    private Camera? selectedCamera;
    private Transform? movementTrack;
    private int selectedCameraIndex = -1;

    [field: SerializeField]
    public Camera[] Cameras { get; set; } = Array.Empty<Camera>();

    [field: SerializeField]
    public RenderTexture? TargetTexture { get; set; }

    private Transform? MovementTrack
    {
        get => movementTrack;
        set
        {
            if (movementTrack != value)
            {
                movementTrack = value;
            }

            if (value == null)
            {
                return;
            }

            var scale = value.localScale;
            // get the dominant axis in the x-z plane based on scale
            var trackAxis = Vector3.up + (scale.x > scale.z
                ? Vector3.right
                : Vector3.forward);
            var min = Vector3.Scale(trackAxis, value.position - (trackAxis * scale.magnitude) / 2f);
            localTrackRange = (min, min + trackAxis * scale.magnitude);
            movementTrack = value;
        }
    }

    public Camera? SelectedCamera
    {
        get => selectedCamera;
        set
        {
            if (selectedCamera == value)
            {
                return;
            }

            if (selectedCamera is not null)
            {
                selectedCamera.targetTexture = null;
            }

            selectedCamera = value;
            MovementTrack = value == null ? null : value
                .GetComponentsInChildren<Transform>()
                .First(x => x.parent == value.transform);

            if (value != null)
            {
                value.targetTexture = TargetTexture;
            }

            SelectedFishSystemChanged.Invoke(SelectedFishSystem);
        }
    }

    public FishSystemScript? SelectedFishSystem
    {
        get => selectedCamera == null ? null : selectedCamera.GetComponentInParent<FishSystemScript>();
    }

    public UnityEvent<FishSystemScript?> SelectedFishSystemChanged { get; } = new();

    // Start is called before the first frame update
    void Start()
    {
        selectedCameraIndex = 0;
        SelectedCamera = Cameras.Length > 0 ? Cameras[selectedCameraIndex] : null;
        if (SelectedCamera != null && MovementTrack?.position != null)
        {
            SelectedCamera.transform.position = MovementTrack.position;
        }
    }

    void Update()
    {
        //Look(90 /* degrees */, .01f);
    }

    /// <summary>
    /// Pans the camera in the specified direction
    /// </summary>
    /// <param name="angle">The angle in degrees</param>
    /// <param name="intensity">The speed at which the camera is panned.</param>
    public void Look(float angle, float intensity)
    {
        intensity = Math.Clamp(intensity, 0f, 1f);
        var horizontal = Vector3.up * (MathF.Sin(angle * MathF.PI / 180)); // 0deg = up, angle deg = right, etc.
        var vertical = Vector3.right * MathF.Cos(angle * MathF.PI / 180);
        Look((horizontal + vertical) * intensity);
    }

    /// <summary>
    /// Pans the camera in the specified direction
    /// </summary>
    /// <param name="direction">The direction and intensity</param>
    public void Look(Vector3 direction)
    {
        if (selectedCamera == null)
        {
            return;
        }
        var target = selectedCamera.transform;
        target.localRotation = Quaternion.Euler(target.localRotation.eulerAngles + direction);
    }

    /// <summary>
    /// Moves the camera along the track.
    /// </summary>
    /// <param name="direction">A normalized value between -1 and 1, indicating the rate of movement along the track.</param>
    public void Move(float direction)
    {
        // Select away the up-down axis
        Transform(direction, Vector3.forward + Vector3.right);
    }

    /// <summary>
    /// Moves the camera along the track.
    /// </summary>
    /// <param name="direction">A normalized value between -1 and 1, indicating the rate of movement along the track.</param>
    public void Elevate(float direction)
    {
        Transform(direction, Vector3.up);
    }

    /// <summary>
    /// Changes the active camera to the next camera.
    /// </summary>
    public void NextCamera()
    {
        selectedCameraIndex = (selectedCameraIndex + 1) % Cameras.Length;
        SelectedCamera = Cameras[selectedCameraIndex];
    }

    /// <summary>
    /// Changes the active camera to the previous camera.
    /// </summary>
    public void PreviousCamera()
    {
        selectedCameraIndex = (selectedCameraIndex - 1) < 0 ? Cameras.Length - 1 : (selectedCameraIndex - 1);
        SelectedCamera = Cameras[selectedCameraIndex];
    }

    private void Transform(float direction, Vector3 mask)
    {
        const float maxSpeed = .01f; // units/s
        direction = Math.Clamp(direction, -1f, 1f);

        if (selectedCamera == null)
        {
            return;
        }

        // Select away the up-down axis
        var axis = Vector3.Scale(localTrackRange.Max - localTrackRange.Min, mask);

        var target = selectedCamera.transform;
        var trackLength = axis.magnitude;
        var unit = axis.normalized;
        var offset = (Vector3.Dot(target.position, unit) - Vector3.Dot(localTrackRange.Min, unit)) / trackLength;
        var newOffset = Math.Clamp(offset + direction * maxSpeed / trackLength, 0f, 1f);
        target.position = localTrackRange.Min + newOffset * axis;
    }
}
