using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruler : MonoBehaviour
{
    private Dictionary<GameObject,float> fishLengths = new Dictionary<GameObject, float>();

    private void OnTriggerEnter(Collider collision) 
    {
        if (collision.GetType() == typeof(CapsuleCollider) && collision.GetComponent<Weight>())
        {
            float lengt = collision.GetComponent<Weight>().fish.transform.localScale.x;
            float fishLength = (float)(4.58 * Math.Exp(2.33 * lengt) + 10.31);
            fishLengths.Add(collision.GetComponent<Weight>().fish, fishLength);
        }
    }
}