using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSystemScript : MonoBehaviour
{
    public GameObject fish;
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

    // Start is called before the first frame update
    void Start()
    {
        // positioning particles at top of fish system
        foodParticles.transform.position = new Vector3(foodParticles.shape.position.x, gameObject.transform.position.y + (height / 2), foodParticles.shape.position.z);
        for (int i = 0; i < amountFish; i++)
        {
            Instantiate(fish, new Vector3(Random.Range(-radius + 3, radius - 3), Random.Range(-height/2 + 3, height/2- 3), Random.Range(-radius + 3, radius - 3)), fish.transform.rotation);
        }
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
}
