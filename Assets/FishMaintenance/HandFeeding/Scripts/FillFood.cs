using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillFood : MonoBehaviour
{

    public GameObject spawnPellet;


    // private List<GameObject> food= new List<GameObject>();
    // Start is called before the first frame update
    void Awake()
    {

        //   this.food=gameObject.transform.GetChild(0).gameObject;
        // int children = gameObject.transform.childCount;
        // for(int i = 0; i < children; ++i){
        //     food.Add(gameObject.transform.GetChild(i).gameObject);

    }

    // Update is called once per frame

    void OnCollisionEnter(Collision collision)
    {
        GameObject foods = transform.GetChild(0).gameObject;
        // for(int i = 0; i < children; ++i){

        if (collision.gameObject.tag == "Bucket")
        {
            foods.SetActive(true);
        }

        if (collision.gameObject.tag == "Merd")
        {
            foods.SetActive(false);


            Instantiate(spawnPellet, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            spawnPellet.GetComponent<Rigidbody>().AddForce(new Vector3(0, 10f, 0));



            //  Rigidbody rb = child.GetComponent<Rigidbody>();
            //   rb.isKinematic = false;
            // rb.detectCollisions = true;
            // 
            // rb.AddForce(throwForce);
        }


    }

    void Update()
    {



    }

}