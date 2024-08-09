using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveHand : MonoBehaviour
{
    float originalY;
    private float speed = 2;
    private float floatStrength = .1f;

    // Start is called before the first frame update
    void Start()
    {
        this.originalY = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, originalY + Math.Abs((float)Mathf.Sin(Time.time * speed) * floatStrength), transform.position.z);
    }
}
