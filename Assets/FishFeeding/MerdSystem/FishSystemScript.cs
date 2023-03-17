using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSystemScript : MonoBehaviour
{
    public GameObject fish;
    private GameObject[] fishes;
    private FishScript fishScript;
    public ParticleSystem foodParticles;
    public float radius = 10;
    public float height = 20;
    public float amountFish = 5;
    public float fullnessDivider = 0.7f;
    public float fullnessLimit = 70;
    public float hungerRate = 3.0f;
    public float swimSpeedVertical = 0.2f;
    public float swimSpeedHorizontal = 0.1f;
    public bool feeding = false;    // all fish in the top part ("hunger zone") will be fed when this is true
    /*public int amountFishFed = 0;
    public int amountFishFullFed = 0;*/
    public float foodWasted = 0;
    private int foodGivenFish = 3;
    public int foodGivenPerSec = 1500;

    // Start is called before the first frame update
    void Start()
    {
        // positioning particles at top of fish system
        foodParticles.transform.position = new Vector3(foodParticles.shape.position.x, gameObject.transform.position.y + (height / 2), foodParticles.shape.position.z);
        for (int i = 0; i < amountFish; i++)
        {
            Instantiate(fish, new Vector3(Random.Range(-radius + 3, radius - 3), Random.Range(-height/2 + 3, height/2- 3), Random.Range(-radius + 3, radius - 3)), fish.transform.rotation);
        }
        fishes = GameObject.FindGameObjectsWithTag("Fish");
        InvokeRepeating(nameof(FeedFish), 0.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            feeding = true;
            if (!foodParticles.isPlaying)
            {
                foodParticles.Play();
            }
        } else
        {
            feeding = false;
            foodParticles.Stop();
        }
    }

    void FeedFish()
    {
        // Return if we're not feeding
        if (!feeding)
        {
            return;
        }
        /*Debug.Log("foodWasted: " + foodWasted);*/
        foodWasted = foodGivenPerSec;
        int foodEaten = 0;

        foreach (GameObject i in fishes)
        {
            FishScript script = i.GetComponent<FishScript>();
            float posY = i.transform.position.y;
            // 1/2 likelihood of getting fed if player is feeding
            if (posY > fullnessDivider)
            {
                /*if (Random.Range(0, 100) <= 50)
                {*/
                    if (script.fullness < 100)
                    {
                        /*fullness = 100;*/
                        script.fullness += foodGivenFish;
                        foodWasted -= foodGivenFish;
                        foodEaten += foodGivenFish;
                    }
               /* }*/
            }

        }Debug.Log("foodWasted: " + foodWasted + "FoodEaten: " + foodEaten);
    }
}
