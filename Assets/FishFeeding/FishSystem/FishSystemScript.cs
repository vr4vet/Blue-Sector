using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FishSystemScript : MonoBehaviour
{
    public GameObject fish;
    private GameObject[] fishes;
    private ParticleSystem foodParticles;
    public float radius = 10;
    public float height = 20;
    public float amountOfFish = 30;
    public float fullnessDivider = 0.7f;
    public float fullnessLimit = 70;
    public float hungerRate = 3.0f;
    public float swimSpeedVertical = 0.5f;
    public float swimSpeedHorizontal = 1.0f;
    public bool feeding = false;    // all fish in the top part ("hunger zone") will be fed when this is true
    public float foodWasted = 0;
    private int foodGivenFish = 3;
    public int foodGivenPerSec = 1500;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = gameObject.transform.position;
        foodParticles = gameObject.GetComponent<ParticleSystem>();
        var shape = foodParticles.shape;
        // positioning particles at top of fish system
        //shape.position = new Vector3(shape.position.x, gameObject.transform.position.y + (height / 2), shape.position.z);
        for (int i = 0; i < amountOfFish; i++)
        {
            GameObject newFish = Instantiate(fish, new Vector3(Random.Range(position.x - radius + 3, position.x + radius - 3), Random.Range(position.y - (height/2)+ 3,position.y + (height/2)- 3), Random.Range(position.z -radius + 3, position.z + radius - 3)), fish.transform.rotation);
            newFish.transform.parent = gameObject.transform;
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
            foodParticles.Play();
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

        }
        Debug.Log("foodWasted: " + foodWasted + "FoodEaten: " + foodEaten);
    }
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
