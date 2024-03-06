using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillFood : MonoBehaviour
{

    [SerializeField] private GameObject spawnPellet;
    private GameObject foods;


    // private List<GameObject> food= new List<GameObject>();
    // Start is called before the first frame update
    void Awake()
    {
        foods = transform.GetChild(0).gameObject;

        //   this.food=gameObject.transform.GetChild(0).gameObject;
        // int children = gameObject.transform.childCount;
        // for(int i = 0; i < children; ++i){
        //     food.Add(gameObject.transform.GetChild(i).gameObject);

    }


    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Bucket"))
        {
            foods.SetActive(true);
        }
        // if (other.CompareTag("Merd") && foods.activeSelf)
        // {
        //     foods.SetActive(false);
        //     ReleasePellets();
        // }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Merd") && foods.activeSelf)
        {
            foods.SetActive(false);
            ReleasePellets();
        }
    }

    private void ReleasePellets()
    {

        foreach (Transform child in foods.transform)
        {

            Rigidbody rb = spawnPellet.GetComponent<Rigidbody>();

            rb.AddForce(new Vector3(5f, 10f, 0));
            Instantiate(spawnPellet, child.position, child.rotation);
        }
    }
    void Update()
    {



    }

}