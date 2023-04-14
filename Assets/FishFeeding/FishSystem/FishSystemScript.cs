using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FishSystemScript : MonoBehaviour
{
    public GameObject fish;
    private ParticleSystem foodParticles;
    public ParticleSystem.EmissionModule emission;
    public float radius = 10;
    public float height = 20;
    public int amountOfFish = 30;
    private readonly int amountOfRandomFish = 10;
    public float fullnessDivider = 0.7f;
    public float swimSpeedVertical = 0.5f;
    public float swimSpeedHorizontal = 1.0f;
    public float foodWasted;
    private readonly int eatingAmount = 3;
    public int foodBase;
    public int foodGivenPerSec;
    public float modifier = 1.0f;

    [HideInInspector]
    public enum FishState   // state of fish within this fish system
    {
        Full,
        Hungry,
        Dying,
        Idle
    }

    // [HideInInspector]
    public FishState state;

    [HideInInspector]
    public enum FeedingIntensity
    {
        High = 40,
        Medium = 20,
        Low = 5,
        Off = 0,
    }

    public FeedingIntensity feedingIntensity;
    // Start is called before the first frame update
    void Start()
    {
        foodBase = amountOfFish * eatingAmount;
        state = FishState.Idle;     // initiate in Idle state
        feedingIntensity = FeedingIntensity.Medium;     // initiate with medium feeding intensity
        foodGivenPerSec = foodBase; // initiate foodGivenPerSec at medium level when fish is full

        // get position for spawning fish within fish system boundaries
        Vector3 position = gameObject.transform.position;
        
        // initiate particles
        foodParticles = gameObject.GetComponent<ParticleSystem>();
        foodParticles.Play();
        emission = foodParticles.emission;
        emission.rateOverTime = 20;

        // set particle radius and move to top
        var foodParticlesShape = gameObject.GetComponent<ParticleSystem>().shape;
        foodParticlesShape.radius = radius;
        foodParticlesShape.position = new Vector3(0, height / 2, 0);

        // spawn fish
        for (int i = 0; i < amountOfFish; i++)
        {
            GameObject newFish = Instantiate(fish, new Vector3(Random.Range(position.x - radius + 3, position.x + radius - 3), Random.Range(position.y - (height/2), position.y + (height/2)), Random.Range(position.z -radius + 3, position.z + radius - 3)), fish.transform.rotation);
            newFish.transform.parent = gameObject.transform;
        }

        // spawn random fish
        for (int i = 0; i < amountOfRandomFish; i++)
        {
            GameObject newFish = Instantiate(fish, new Vector3(Random.Range(position.x - radius + 3, position.x + radius - 3), Random.Range(position.y - (height / 2), position.y + (height / 2)), Random.Range(position.z - radius + 3, position.z + radius - 3)), fish.transform.rotation);
            newFish.transform.parent = gameObject.transform;
            newFish.GetComponent<FishScript>().SetRandomFish();
        }

        InvokeRepeating(nameof(HandleFishState), 0.0f, 1.0f);
        InvokeRepeating(nameof(ComputeFoodWaste), 0.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(gameObject.transform.position.y);
        //Debug.Log(height);
        if (Input.GetKeyDown(KeyCode.J))
        {
            feedingIntensity = FeedingIntensity.Low;
            foodGivenPerSec = foodBase * 1 / 3;
            emission.rateOverTime = 5;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            feedingIntensity = FeedingIntensity.Medium;
            foodGivenPerSec = foodBase * 1;
            emission.rateOverTime = 20;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            feedingIntensity = FeedingIntensity.High;
            foodGivenPerSec = foodBase * 5 / 3;
            emission.rateOverTime = 40;
        }
        //Debug.Log(feedingIntensity);
    }

    // functions for setting idle state
    public void SetIdle() 
    {
        state = FishState.Idle;
        hungerStatus = 0;
    }

    public void ReleaseIdle() => state = FishState.Full;

    /* Make fish hungry after som random time between 25-35 seconds when not feeding,
     * full if fed for 10-12 seconds, 
     * dying if not fed while hungry for 25-35 seconds.
     * This function is called once every second.
     */
    void HandleFishState()
    {
        var main = foodParticles.main;    // used for setting particle life time to indiciate if fish are eating
        switch(state)
        {
            case FishState.Full:
                main.startLifetime = 5.0f; // food particles should hit/pass through floor
                HandleFull();
                break;
            case FishState.Hungry:
                main.startLifetime = 0.4f; // food particles should dissapear high in water
                HandleHungry();
                break;
            case FishState.Dying:
                main.startLifetime = 0.4f; // food particles should dissapear high in water
                HandleDying();
                break;
            case FishState.Idle:
                HandleIdle();
                break;
        }   

    }

    private int fullTicks = 0;  // gets hungry when this passes timeToHungry.
    void HandleFull()
    {
        int timeToHungry = (int)Random.Range(25.0f * modifier, 35.0f * modifier);
        // switch state to hungry, and reset timer
        if (fullTicks >= timeToHungry)
        {
            state = FishState.Hungry;
            fullTicks = 0;
            return;
        }

        switch(feedingIntensity)
        {
            case FeedingIntensity.High:
                fullTicks = 0;  // reset when fed, not getting hungry
                break;
            case FeedingIntensity.Medium:
                fullTicks++;
                break;
            case FeedingIntensity.Low:
                fullTicks += 2;  // fish get hungry twice as fast
                break;
            case FeedingIntensity.Off:
                fullTicks += 4; // fish are starvin bruv
                break;
        }
    }


    private int hungerStatus = 0;   // dies if <= secondsToDying, gets full if >= secondsToFull
    void HandleHungry()
    {
        int secondsToFull = (int)Random.Range(10.0f, 12.0f);
        int secondsToDying = (int)Random.Range(-25.0f * modifier, -35.0f * modifier);
        Debug.Log("hungerstatus: " + hungerStatus);
        if (hungerStatus >= secondsToFull)
        {
            // switch to full state, and reset status
            state = FishState.Full;
            hungerStatus = 0;   
            return;
        } else if (hungerStatus <= secondsToDying)
        {
            // switch to dying state
            state = FishState.Dying;
            return;
        }

        switch (feedingIntensity)
        {
            case FeedingIntensity.High:
                hungerStatus++;      
                break;
            case FeedingIntensity.Medium:
                hungerStatus--;  
                break;
            case FeedingIntensity.Low:
                hungerStatus -= 2;  // quickly starts starving
                break;
            case FeedingIntensity.Off:
                hungerStatus -= 4; // fish are starvin bruv
                break;
        }
    }

    void HandleIdle()
    {
        if (IsInvoking(nameof(KillFish)))
            CancelInvoke(nameof(KillFish));
    }

    void HandleDying()
    {           
        if (feedingIntensity == FeedingIntensity.High)
        {
            CancelInvoke(nameof(KillFish));
            state = FishState.Hungry;
            hungerStatus = -20;     // fish should still be almost starving, requiring high feeding intensity to prevent death
        } else
        {
            if (!IsInvoking(nameof(KillFish)))
            {
                InvokeRepeating(nameof(KillFish), 0.0f, 5.0f);      // new fish dies every 5 second
            }
        }
    }

    public int fishKilled = 0;
    void KillFish()
    {
        if (fishKilled < amountOfFish)
        {
            // kill fish one by one
            gameObject.transform.GetChild(1 + fishKilled).GetComponent<FishScript>().Kill();
            fishKilled++;
        }
    }

    /* Gives the wasted food in this second based on the state to the merd and the feeding intensity. */
   void ComputeFoodWaste()
    {
        if (state == FishState.Full)
        {
            foodWasted = foodGivenPerSec;
        } else
        {
            foodWasted = 0;
        }
        //Debug.Log("foodWasted: " + foodWasted);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(FishSystemScript))]
public class FishSystemVisualization : Editor
{
    public void OnSceneGUI()
    {
        // getting parameters for drawing visualization
        var t = target as FishSystemScript;
        var transform = t.transform;
        var position = t.transform.position;
        float height = t.height;
        float radius = t.radius;

        // drawing top and bottom of system
        float top = position.y + (height / 2);  // top of merd/water surface
        float bottom = position.y - (height / 2); // bottom of merd
        Handles.color = Color.green;
        Handles.DrawWireDisc(position + new Vector3(0, height/2, 0), transform.up, radius);
        Handles.DrawWireDisc(position + new Vector3(0, -height/2, 0), transform.up, radius);
        
        // drawing vertical lines form top to bottom
        Handles.DrawLine(position + new Vector3(radius, height/2, 0), position + new Vector3(radius, -height/2, 0));
        Handles.DrawLine(position + new Vector3(-radius, height/2, 0), position + new Vector3(-radius, -height/2, 0));
        Handles.DrawLine(position + new Vector3(0, height/2, radius), position + new Vector3(0, -height/2, radius));
        Handles.DrawLine(position + new Vector3(0, height/2, -radius), position + new Vector3(0, -height/2, -radius));

        // drawing the divider between the zones of hungry and full fish
        float fullnessDivider = bottom + ((top - bottom) * t.fullnessDivider); // border between hungry and full fish
        Handles.color = Color.red;
        Handles.DrawWireDisc(new Vector3(position.x, fullnessDivider, position.z), transform.up, radius);

        // set particle radius and move to top
        var foodParticlesShape = t.GetComponent<ParticleSystem>().shape;
        foodParticlesShape.radius = radius;
        foodParticlesShape.position = new Vector3(0, height/2, 0); 
    }

}
#endif
