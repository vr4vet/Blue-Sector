using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatArrow : MonoBehaviour
{
    float originalY;
    private float speed = 2;
    private float floatStrength = .5f;

    // Start is called before the first frame update
    void Start()
    {
        this.originalY = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, originalY + ((float)Mathf.Sin(Time.time * speed) * floatStrength), transform.position.z);
    }
}
