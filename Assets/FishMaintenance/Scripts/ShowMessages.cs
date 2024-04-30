using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowMessages : MonoBehaviour
{
    public Transform handPosition;
    public GameObject screen;
    public GameObject messageScreen;

    public TextMeshPro textMesh;

    void Update()
    {
        if (textMesh.text.Length < 3)
        {
            messageScreen.SetActive(false);

        }
        else { messageScreen.SetActive(true); }

        Quaternion localRotation = handPosition.localRotation;
        float pitchAngleX = localRotation.eulerAngles.x;
        // float pitchAngleY = Quaternion.Euler(localRotation.eulerAngles).y;
        // if (Mathf.Abs(pitchAngleX) > 350f && Mathf.Abs(pitchAngleX) < 50f && Mathf.Abs(pitchAngleY) > 300f && Mathf.Abs(pitchAngleY) < 335f)

        if ((Mathf.Abs(pitchAngleX) < 70f && Mathf.Abs(pitchAngleX) > 0f) || Mathf.Abs(pitchAngleX) > 345f)
        {
            screen.SetActive(true);
        }
        else
        {
            screen.SetActive(false);

        }
    }
}
