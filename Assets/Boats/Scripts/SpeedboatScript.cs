using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpeedboatScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1.0f) // only move boat if time is in 1x speed, or else the pause causes an offset (floating/sinking boat)
            transform.position = new Vector3(transform.position.x, transform.position.y + (Mathf.Sin(Time.time) * 0.0005f), transform.position.z);
    }
}
