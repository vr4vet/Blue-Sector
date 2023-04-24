using System;
using UnityEngine;
using FeedingIntensity = FishSystemScript.FeedingIntensity;
using FishState = FishSystemScript.FishState;

public abstract class FishFeedHint : MonoBehaviour
{
    public GameObject Merd;
    private Tutorial tutorial;
    private DateTime canTriggerAt;
    private TimeSpan triggerDelay;

    protected FishSystemScript FishSystem { get; set; }

    protected virtual void Start()
    {
        tutorial = GetComponentInChildren<Tutorial>();
        triggerDelay = TimeSpan.FromSeconds(UnityEngine.Random.Range(10, 60));
        canTriggerAt = DateTime.MaxValue;
        Debug.Assert(tutorial != null);
        if (Merd != null)
        {
            FishSystem = Merd.GetComponent<FishSystemScript>();
        }
    }

    protected virtual void Update()
    {
        if (!tutorial.Triggered && ShouldTrigger())
        {
            canTriggerAt = DateTime.Now + triggerDelay;
        }

        if (canTriggerAt < DateTime.Now)
        {
            tutorial.Trigger();
            enabled = false;
        }
    }

    protected abstract bool ShouldTrigger();
}
