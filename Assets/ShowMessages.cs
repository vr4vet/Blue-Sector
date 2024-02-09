using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMessages : MonoBehaviour
{
    public Transform handPosition;
    public GameObject screen;

    // Update is called once per frame
    void Update()
    {
        float handTiltAngleForward = Vector3.Angle(Vector3.up, handPosition.forward);
        float handTiltAngleUp = Vector3.Angle(Vector3.forward, handPosition.up);
        if (handTiltAngleForward > 85f && handTiltAngleForward < 130f && handTiltAngleUp > 20f && handTiltAngleUp < 60f)
        {
            screen.SetActive(true);
        }
        else
        {
            screen.SetActive(false);
        }
    }
}
