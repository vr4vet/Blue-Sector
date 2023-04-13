using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class SlideBridge : MonoBehaviour
{
    private enum Intensity
    {
        High = 75,
        Medium = 50,
        Low = 25,
        Off = 0,
    }

    private FishSystemScript fishSystemScript;
    private Enum intensity;
    private BNG.Slider slider;
    
    [field: SerializeField]
    public GameObject FishSystem { get; set; }

    private void start (){}

    public void OnSlideChanged(float position){

        fishSystemScript = FishSystem.GetComponent<FishSystemScript>();

        // Debug.Log("Current POS:" + position + " Intensity: " + fishSystemScript.feedingIntensity);

        switch(position)
        {
            case > (float) Intensity.Medium:
            fishSystemScript.SetIntensity(FishSystemScript.FeedingIntensity.High);
            break;
                
            case > (float) Intensity.Low:
            fishSystemScript.SetIntensity(FishSystemScript.FeedingIntensity.Medium);
            break;

            default:
            fishSystemScript.SetIntensity(FishSystemScript.FeedingIntensity.Low);
            break;
        }
    }
}