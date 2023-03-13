using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSystemScript : MonoBehaviour
{
    public GameObject fish;
    public float radius = 10;
    public float height = 10;
    public float amountFish = 5;
    public float fullnessDivider = 7;
    public float fullnessLimit = 70;
    public float hungerRate = 3.0f;
    public float upDownSpeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < amountFish; i++)
        {
            Instantiate(fish, new Vector3(Random.Range(-radius + 3, radius - 3), Random.Range(-height + 3, height - 3), Random.Range(-radius + 3, radius - 3)), fish.transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
