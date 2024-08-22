using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolvingNosePiece : MonoBehaviour
{
    [SerializeField] private MicroscopeMonitor MicroscopeMonitor;
    private float RotationSpeed = 10f;
    private bool Rotating = false;
    private bool RotatingDirection = false; // false is left, true is right

    private void OnTriggerEnter(Collider other)
    {
        // check if player hand
        if (other.name == "Grabber" || other.name == "tip_collider_i")
        {
            MicroscopeMonitor.Magnify();
        }    
    }

    private float RotationDegrees = 90f;
    private void FixedUpdate()
    {
        if (Rotating)
        {
            Debug.Log(RotationDegrees);
            if (RotationDegrees > 0f)
            {
                transform.RotateAround(GetComponent<BoxCollider>().bounds.center, transform.up, RotatingDirection ? -RotationSpeed : RotationSpeed);
                RotationDegrees -= RotationSpeed;
            }
            else
            {
                Rotating = false;
                RotationDegrees = 90f;
            }
                
        }
    }

    public void RotateNosePiece(bool Right)
    {
        Rotating = true;
        RotatingDirection = Right;
    }
}
