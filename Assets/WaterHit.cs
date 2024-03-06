using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterHit : MonoBehaviour
{
    [SerializeField] private GameObject splash;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            Vector3 updatedPos = new Vector3(transform.position.x, 0.5f, transform.position.z);
            Instantiate(splash, updatedPos, splash.transform.rotation);
            ParticleSystem particleSystem = splash.GetComponent<ParticleSystem>();
            particleSystem.Play();
        }
    }
}
