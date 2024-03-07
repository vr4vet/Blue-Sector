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
            Vector3 updatedPos = new Vector3(transform.position.x, 0.1f, transform.position.z);
            Instantiate(splash, updatedPos, splash.transform.rotation);
            ParticleSystem particleSystem = splash.GetComponent<ParticleSystem>();
            particleSystem.Play();
        }
    }
    void OnCollisionEnter(Collision collision)
    {


        if (collision.gameObject.CompareTag("OceanFloor"))
        {
            Destroy(this.gameObject);
        }
    }
}
