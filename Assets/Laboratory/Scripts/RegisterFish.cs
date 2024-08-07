using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterFish : MonoBehaviour
{
    public float fishWeight;
    public float fishLength;
    public float conditionRight;
    public GameObject fishObject;
    private void OnTriggerEnter(Collider collisionObject)
    {
        if (collisionObject.GetComponent<Weight>())
        {
            fishObject = collisionObject.GetComponent<Weight>().fish;
            
            // Get weight
            fishWeight = collisionObject.GetComponent<Weight>().ObjectWeight;
            fishWeight = (float)System.Math.Round(fishWeight, 3);

            // Get length
            float lengt = collisionObject.GetComponent<Weight>().fish.transform.localScale.x;
            //fishLength = (float)(477.7606 + (-7.463894 - 477.7606) / (1 + Math.Pow(lengt / 12.7079, 0.9475411))); //1.4 scale up
            fishLength = (float)(4.58 * Math.Exp(2.33 * lengt) + 10.31); // 1 scale
            fishLength = (float)System.Math.Round(fishLength, 0);
            
            // Calculate condition factor
            conditionRight = ((fishWeight/fishLength)/fishLength)/fishLength;
            conditionRight *= 100;
            conditionRight = (float)Math.Round(conditionRight, 5);
        }
    }
}