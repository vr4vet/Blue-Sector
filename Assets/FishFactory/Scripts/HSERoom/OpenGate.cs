using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
        if (
            GameManager.instance.RightHand != GameManager.PlayerHandState.Unsanitized
            && GameManager.instance.LeftHand != GameManager.PlayerHandState.Unsanitized
        )
        {
            MoveRightGate();
            MoveLeftGate();
        }
    }

    private void MoveRightGate()
    {
        // get child of this gameobject based on name
        Transform doorRight = transform.Find("DoorRight");

        // slowly move the gameobject to the right. Stop on z = 1
        if (doorRight.transform.localPosition.z > -1f)
            doorRight.localPosition += new Vector3(0, 0, -0.01f);
    }

    private void MoveLeftGate()
    {
        // get child of this gameobject based on name
        Transform doorLeft = transform.Find("DoorLeft");

        // slowly move the gameobject to the left. Stop on z = 1.45
        if (doorLeft.localPosition.z < 1.45)
            doorLeft.localPosition += new Vector3(0, 0, 0.01f);
    }
}
