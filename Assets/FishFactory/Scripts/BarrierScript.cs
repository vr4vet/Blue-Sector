using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    [SerializeField] float targetRotation = 45f;
    [SerializeField] float speed = 20f;

    [SerializeField] bool reverse = false;
     //when active the barrier will block the fish from moving forward and guide it to the correct path
    private bool isActive = false;

    public void toggleBarrier()
    {
        if (isActive)
        {
            StartCoroutine(RotateTo(reverse ? targetRotation : -targetRotation, speed));
        }
        else
        {
            StartCoroutine(RotateTo(reverse ? -targetRotation : targetRotation , speed));
        }
        isActive = !isActive;
    }

    IEnumerator RotateTo(float targetRotation, float speed)
    {
        Quaternion from = transform.localRotation;
        //Calculate the the new rotation based on local rotation
        Quaternion to = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y + targetRotation, transform.localEulerAngles.z);        
        float elapsed = 0.0f;

        while (elapsed < 1.0f)
        {
            elapsed += Time.deltaTime * speed;
            transform.localRotation = Quaternion.Slerp(from, to, elapsed);
            yield return null;
        }
    }
}
