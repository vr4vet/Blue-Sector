using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{

    Camera m_Camera;

    void Start()
    {
        m_Camera = Camera.main;
    }

    void Update()
    {
        //Attached to a canvas, the canvas will always face the camera/player
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
                         m_Camera.transform.rotation * Vector3.up);
    }
}