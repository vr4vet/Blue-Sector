using System.Collections;
using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    // -------- Editor Variables --------

    [Tooltip("The angle the barrier will rotate to")]
    [SerializeField]
    private float targetRotation = 30f;

    [Tooltip("The speed at which the barrier will rotate")]
    [SerializeField]
    private float speed = 3f;

    // -------- Private Variables --------

    private Quaternion down;

    private Quaternion up;

    //when active the barrier will block the fish from moving forward and guide it to the side conveyor path
    private bool isActive = false;

    // -------- Unity Functions --------

    void Start()
    {
        //Calculate the lower rotation based on current local rotation
        down = transform.localRotation;

        //Calculate the upper rotation based on local rotation
        up = Quaternion.Euler(
            transform.localEulerAngles.x,
            transform.localEulerAngles.y + targetRotation,
            transform.localEulerAngles.z
        );
    }

    // -------- Public Functions --------

    /// <summary>
    /// Toggles the barrier between the up and down positions
    /// </summary>
    public void toggleBarrier()
    {
        StartCoroutine(Rotate(speed));
    }

    /// <summary>
    /// Returns the current state of the barrier
    /// </summary>
    /// <param name="speed"> The speed at which the barrier will rotate </param>
    /// <returns> IEnumerator </returns>
    IEnumerator Rotate(float speed)
    {
        float elapsed = 0.0f;

        // Opens or closes the barrier based on the current state
        if (isActive)
        {
            while (elapsed < 1.0f)
            {
                elapsed += Time.deltaTime * speed;
                transform.localRotation = Quaternion.Slerp(down, up, elapsed);
                isActive = false;
                yield return null;
            }
        }
        else
        {
            while (elapsed < 1.0f)
            {
                elapsed += Time.deltaTime * speed;
                transform.localRotation = Quaternion.Slerp(up, down, elapsed);
                isActive = true;
                yield return null;
            }
        }
    }
}
