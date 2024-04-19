using UnityEngine;

public class OpenGate : MonoBehaviour
{
    // ------------- Private Variables -------------

    private Transform doorRight;

    private Transform doorLeft;

    private const float rightDoorStopPoint = -1f;

    private const float leftDoorStopPoint = 1.45f;

    // ------------- Unity Functions -------------

    void Start()
    {
        doorRight = transform.Find("DoorRight");
        doorLeft = transform.Find("DoorLeft");
    }

    void FixedUpdate()
    {
        if (
            GameManager.Instance.RightHand != GameManager.PlayerHandState.Unsanitized
            && GameManager.Instance.LeftHand != GameManager.PlayerHandState.Unsanitized
        )
        {
            MoveRightGate();
            MoveLeftGate();
        }
    }

    // ------------- Private Functions -------------

    /// <summary>
    /// Move the right gate to the right. The gate stays open in case the player wants to return to the previous room
    /// </summary>
    private void MoveRightGate()
    {
        // slowly move the gameobject to the right. Stop on z = 1
        if (doorRight.localPosition.z > rightDoorStopPoint)
            doorRight.localPosition += new Vector3(0, 0, -0.01f);
    }

    /// <summary>
    /// Move the left gate to the left. The gate stays open in case the player wants to return to the previous room
    /// </summary>
    private void MoveLeftGate()
    {
        // slowly move the gameobject to the left. Stop on z = 1.45
        if (doorLeft.localPosition.z < leftDoorStopPoint)
            doorLeft.localPosition += new Vector3(0, 0, 0.01f);
    }
}
