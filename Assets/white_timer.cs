using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class white_timer : MonoBehaviour
{

    public float time = 20f;
    public AudioSource hai;

    // Use this for initialization
    void Start()
    {
    }

    public void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
        }
        else
        {
            hai.enabled = true;
        }


    }
}


