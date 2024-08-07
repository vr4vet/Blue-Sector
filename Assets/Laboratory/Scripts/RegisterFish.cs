using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterFish : MonoBehaviour
{
    public float fishWeight;
    public float fishLength;
    public float conditionRight;
    private void OnTriggerEnter(Collider collisionObject)
    {
        if (collisionObject.GetComponent<Weight>())
        {
            fishWeight = collisionObject.GetComponent<Weight>().ObjectWeight;
            fishWeight = (float)System.Math.Round(fishWeight, 3);

            float lengt = collisionObject.GetComponent<Weight>().fish.transform.localScale.x;
            //fishLength = (float)(477.7606 + (-7.463894 - 477.7606) / (1 + Math.Pow(lengt / 12.7079, 0.9475411)));
            fishLength = (float)(4.58 * Math.Exp(2.33 * lengt) + 10.31);
            fishLength = (float)System.Math.Round(fishLength, 0);
            
            conditionRight = ((fishWeight/fishLength)/fishLength)/fishLength;
            conditionRight *= 100;
            conditionRight = (float)Math.Round(conditionRight, 5);
        }
    }
}