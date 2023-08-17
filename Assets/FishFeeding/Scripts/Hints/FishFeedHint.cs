using System;
using UnityEngine;

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
            tutorial.Trigger();
            enabled = false;
        }
    }

    protected abstract bool ShouldTrigger();
}
