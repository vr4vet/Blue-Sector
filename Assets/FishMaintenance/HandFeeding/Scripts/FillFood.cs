using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillFood : MonoBehaviour
{

    [SerializeField] private GameObject spawnPellet;
    [SerializeField] private MaintenanceManager manager;
    // [SerializeField] private BNG.Grabber grabber;
    private GameObject foods;
    private Vector3 velocity;

    void Awake()
    {
        foods = transform.GetChild(0).gameObject;


    }


    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Bucket"))
        {
            foods.SetActive(true);
        }

    }

    void OnTriggerStay(Collider other)
    {


        if (other.CompareTag("Merd") && foods.activeSelf)
        {
            Vector3 shovelVelocity = gameObject.GetComponent<Rigidbody>().velocity;
            velocity = new Vector3(3f + shovelVelocity.x * Mathf.Pow(10, 7), 4f, 3f + shovelVelocity.z * Mathf.Pow(10, 7));
            foods.SetActive(false);
            ReleasePellets();
        }
    }

    private void ReleasePellets()
    {
        int spawnNumber = 1;
        foreach (Transform child in foods.transform)
        {

            GameObject newPellet = Instantiate(spawnPellet, child.position, child.rotation);
            newPellet.GetComponent<WaterHit>().SpawnNumber = spawnNumber;
            Rigidbody rb = newPellet.GetComponent<Rigidbody>();
            rb.velocity = gameObject.GetComponent<Rigidbody>().velocity;
            rb.AddForce(velocity, ForceMode.VelocityChange);
            spawnNumber++;
        }
        manager.CompleteStep("Håndforing", "Kast mat til fisken");
    }


    void Update()
    {



    }

}