using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierScript : MonoBehaviour
{
     //when active the barrier will block the fish from moving forward and guide it to the correct path
    private bool isActive = false;
    private float speed = 20f;

    // Start is called before the first frame update
    void Start()
    {
        toggleBarrier();
    }

    public void toggleBarrier()
    {
        if (isActive)
        {
            StartCoroutine(RotateTo(0, speed));
            // transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), speed * Time.deltaTime);
        }
        else
        {
            StartCoroutine(RotateTo(45, speed));
            // transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 45, 0), speed * Time.deltaTime);
        }
        isActive = !isActive;
    }

    IEnumerator RotateTo(float targetRotation, float speed)
    {
        Quaternion from = transform.rotation;
        Quaternion to = Quaternion.Euler(0, targetRotation, 0);
        float elapsed = 0.0f;

        while (elapsed < 1.0f)
        {
            elapsed += Time.deltaTime * speed;
            transform.rotation = Quaternion.Slerp(from, to, elapsed);
            yield return null;
        }
    }
}
