using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierScript : MonoBehaviour
{
     //when active the barrier will block the fish from moving forward and guide it to the correct path
    private bool isActive = false;
    private float speed = 45f;

    // Update is called once per frame
    public void toggleBarrier()
    {
        if (isActive)
        {
            // Rotate back to original position at a certain speed
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), speed * Time.deltaTime);
            isActive = false;
        }
        else
        {
            // Rotate 45 degrees at a certain speed
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 45, 0), speed * Time.deltaTime);
            isActive = true;
        }
    }
}
