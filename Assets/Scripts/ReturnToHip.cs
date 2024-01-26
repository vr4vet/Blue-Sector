using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToHip : MonoBehaviour
{
    Vector3 hipPos;
    Vector3 hipRot;

    // Use this for initialization
    void Start()
    {
        hipPos = gameObject.transform.localPosition;
        hipRot = gameObject.transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.parent.parent.name == "torso")
        {
            returnToHip();
        }

    }
    // Returns object to starting position
    public void returnToHip()
    {
        gameObject.transform.localPosition = hipPos;
        gameObject.transform.localEulerAngles = hipRot;
    }
    // Makes the tablet collide with hands only
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "hand")
        {
            Physics.IgnoreCollision(collision.collider, gameObject.GetComponent<BoxCollider>());
        }
    }
}
