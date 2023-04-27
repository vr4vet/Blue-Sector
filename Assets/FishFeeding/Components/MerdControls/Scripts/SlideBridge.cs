using UnityEngine;

/// <summary>
/// Bridge between slider and FishSystem
/// </summary>
public class SlideBridge : MonoBehaviour
{
    public GameObject FishSystem { get; set; }

    private FishSystemScript fishSystemScript;

    private enum Intensity
    {
        High = 75,
        Medium = 50,
        Low = 25,
        Off = 0,
    }

    public void Start()
    {
        fishSystemScript = FishSystem.GetComponent<FishSystemScript>();
    }

    public void OnSlideChanged(float position)
    {
        switch (position)
        {
            case > (float)Intensity.Medium:
                fishSystemScript.feedingIntensity = FishSystemScript.FeedingIntensity.High;
                fishSystemScript.foodGivenPerSec = fishSystemScript.foodBase * 5 / 3;
                fishSystemScript.emission.rateOverTime = 40;
                break;

            case > (float)Intensity.Low:
                fishSystemScript.feedingIntensity = FishSystemScript.FeedingIntensity.Medium;
                fishSystemScript.foodGivenPerSec = fishSystemScript.foodBase * 1;
                fishSystemScript.emission.rateOverTime = 20;
                break;

            default:
                fishSystemScript.feedingIntensity = FishSystemScript.FeedingIntensity.Low;
                fishSystemScript.foodGivenPerSec = fishSystemScript.foodBase * 1 / 3;
                fishSystemScript.emission.rateOverTime = 5;
                break;
        }
    }
}