using System.Collections;
using UnityEngine;

public class BarrierScript : MonoBehaviour
{

    [Tooltip("The angle the barrier will rotate to, measured by degrees")]
    [SerializeField]
    private float _rotationTarget = 30f;

    [Tooltip("The speed at which the barrier will rotate")]
    [SerializeField]
    private float _rotationSpeed = 3f;

    [Tooltip("The lower rotation of the barrier")]
    private Quaternion _lowerRotation;

    [Tooltip("The upper rotation of the barrier")]
    private Quaternion _upperRotation;

    [Tooltip("When active the barrier will block the fish from moving forward and guide it to the side conveyor path")] 
    private bool _isActive = false;

    void Start()
    {
        //Calculate the lower rotation values based on current local rotation
        _lowerRotation = transform.localRotation;
        _upperRotation = Quaternion.Euler(
            transform.localEulerAngles.x,
            transform.localEulerAngles.y + _rotationTarget,
            transform.localEulerAngles.z
        );
    }

    /// <summary>
    /// Toggles the barrier between the up and _lowerRotation positions
    /// </summary>
    public void toggleBarrier()
    {
        StartCoroutine(Rotate());
    }

    /// <summary>
    /// Returns the current state of the barrier
    /// </summary>
    /// <returns> IEnumerator </returns>
    IEnumerator Rotate()
    {
        float elapsed = 0.0f;
        
        while (elapsed < 1.0f)
        {
            elapsed += Time.deltaTime * _rotationSpeed;
            if (_isActive)
            {
                transform.localRotation = Quaternion.Slerp(_lowerRotation, _upperRotation, elapsed);
            } 
            else
            {
                transform.localRotation = Quaternion.Slerp(_upperRotation, _lowerRotation, elapsed);
            }
            yield return null;
        }
        _isActive = !_isActive;
    }
}
