using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolvingNosePiece : MonoBehaviour
{
    [SerializeField] private MicroscopeMonitor MicroscopeMonitor;
    [SerializeField] private AudioSource ClickSound;
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
            if (RotationDegrees > 0f)
            {
                transform.RotateAround(GetComponent<BoxCollider>().bounds.center, transform.up, RotatingDirection ? -RotationSpeed : RotationSpeed);
                RotationDegrees -= RotationSpeed;
            }
            else
            {
                Rotating = false;
                RotationDegrees = 90f;
                ClickSound.time = 0.1f; // skip the silence at the start of clip
                ClickSound.Play();
            }
                
        }
    }

    public void RotateNosePiece(bool Right)
    {
        if (!Rotating!)
        {
            Rotating = true;
            RotatingDirection = Right;
        }
    }

    public bool IsRotating()
    {
        return Rotating;
    }
}
