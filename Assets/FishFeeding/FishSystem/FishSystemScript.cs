using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FishSystemScript : MonoBehaviour
{
    public GameObject fish;
    private GameObject[] fishes;
    private ParticleSystem foodParticles;
    private ParticleSystem.EmissionModule emission;
    public float radius = 10;
    public float height = 20;
    public int amountOfFish = 30;
    public float fullnessDivider = 0.7f;
    public float swimSpeedVertical = 0.5f;
    public float swimSpeedHorizontal = 1.0f;

    [HideInInspector]
    // private bool feeding = false;    // all fish in the top part ("hunger zone") will be fed when this is true
    public float foodWasted = 0;
    private int eatingAmount = 3;
    private int foodGivenPerSec;

    [HideInInspector]
    public enum FishState   // state of fish within this fish system
    {
        Full,
        Hungry,
        Dying
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
        state = FishState.Full;     // initiate with full state
        feedingIntensity = FeedingIntensity.Medium;     // initiate with medium feeding intensity

        // get position for spawning fish within fish system boundaries
        Vector3 position = gameObject.transform.position;
        
        // initiate particles
        foodParticles = gameObject.GetComponent<ParticleSystem>();
        foodParticles.Play();
        emission = foodParticles.emission;
        emission.rateOverTime = 20;

        // spawn fish
        for (int i = 0; i < amountOfFish; i++)
        {
            GameObject newFish = Instantiate(fish, new Vector3(Random.Range(position.x - radius + 3, position.x + radius - 3), Random.Range(position.y - (height/2)+ 3,position.y + (height/2)- 3), Random.Range(position.z -radius + 3, position.z + radius - 3)), fish.transform.rotation);
            newFish.transform.parent = gameObject.transform;
            newFish.GetComponent<FishScript>().Kill();
        }

        InvokeRepeating(nameof(HandleFishState), 0.0f, 1.0f);
        fishes = GameObject.FindGameObjectsWithTag("Fish");
        foodGivenPerSec = amountOfFish * eatingAmount;
        /*InvokeRepeating(nameof(FeedFish), 0.0f, 1.0f);*/
    }

    // Update is called once per frame
    void Update()
    {

    }

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
                main.startLifetime = 5.0f; // should hit/pass through floor
                HandleFull();
                break;
            case FishState.Hungry:
                main.startLifetime = 1.0f; // should dissapear high in water
                HandleHungry();
                break;
            case FishState.Dying:
                main.startLifetime = 1.0f; // should dissapear high in water
                HandleDying();
                break;
        }
    }

    
    private int fullTicks = 0;  // gets hungry when this passes timeToHungry.
    void HandleFull()
    {
        int timeToHungry = (int)Random.Range(25.0f, 35.0f);
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
        int secondsToDying = (int)Random.Range(-25.0f, -35.0f);
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
                fullTicks += 4; // fish are starvin bruv
                break;
        }
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

    int fishKilled = 0;
    void KillFish()
    {
        if (fishKilled < amountOfFish)
        {
            // kill fish one by one
            gameObject.transform.GetChild(fishKilled).GetComponent<FishScript>().Kill();
            fishKilled++;
        }
    }

    public void SetIntensity(FeedingIntensity intensity)
    {
        feedingIntensity =  intensity;
    }

    /* Feeds each fish if they can eat and computes the food wasted. */
/*    void FeedFish()
    {
        // Return if we're not feeding
        if (!feeding)
        {
            return;
        }
        *//*Debug.Log("foodWasted: " + foodWasted);*//*
        foodWasted = foodGivenPerSec;
        int foodEaten = 0;

        foreach (GameObject i in fishes)
        {
            FishScript script = i.GetComponent<FishScript>();
            float posY = i.transform.position.y;
            if (posY > fullnessDivider)
            {
                if (script.fullness < 100)
                {
                    script.fullness += eatingAmount;
                    foodWasted -= eatingAmount;
                    foodEaten += eatingAmount;
                }
            }

        }
        Debug.Log("foodWasted: " + foodWasted + "FoodEaten: " + foodEaten);
    }*/
}

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
        float top = t.transform.position.y + (height / 2);  // top of merd/water surface
        float bottom = t.transform.position.y - (height / 2); // bottom of merd
        Handles.color = Color.green;
        Handles.DrawWireDisc(position + new Vector3(0, top, 0), transform.up, radius);
        Handles.DrawWireDisc(position + new Vector3(0, bottom, 0), transform.up, radius);
        
        // drawing vertical lines form top to bottom
        Handles.DrawLine(position + new Vector3(radius, top, 0), position + new Vector3(radius, bottom, 0));
        Handles.DrawLine(position + new Vector3(-radius, top, 0), position + new Vector3(-radius, bottom, 0));
        Handles.DrawLine(position + new Vector3(0, top, radius), position + new Vector3(0, bottom, radius));
        Handles.DrawLine(position + new Vector3(0, top, -radius), position + new Vector3(0, bottom, -radius));

        // drawing the divider between the zones of hungry and full fish
        float fullnessDivider = bottom + (((bottom - top) * (t.fullnessDivider)) * -1); // border between hungry and full fish
        Handles.color = Color.red;
        Handles.DrawWireDisc(position + new Vector3(0, fullnessDivider, 0), transform.up, radius);

        // moving food particle system to the top of fish system
        var foodParticlesShape = GameObject.Find("FishSystem").GetComponent<ParticleSystem>().shape;
        foodParticlesShape.position = new Vector3(foodParticlesShape.position.x, GameObject.Find("FishSystem").transform.position.y + (height / 2), foodParticlesShape.position.z);

    }

}
