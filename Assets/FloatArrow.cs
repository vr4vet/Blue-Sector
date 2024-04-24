using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatArrow : MonoBehaviour
{
    float distantY = 2.817f;
    private float speed = 2;
    private float floatStrength = .5f;
    float proximityY = 2f;
    float currentY;
    private bool entered;
    // Start is called before the first frame update
    void Start()
    {
        this.currentY = this.transform.position.y;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, currentY + ((float)Mathf.Sin(Time.time * speed) * floatStrength), transform.position.z);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !entered)
        {

            currentY = proximityY;
            this.transform.position = new Vector3(transform.position.x, currentY, transform.position.z);
            entered = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && entered)
        {

            currentY = distantY;
            this.transform.position = new Vector3(transform.position.x, currentY, transform.position.z);
            entered = false;
        }
    }

}
