using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Despawn : MonoBehaviour {
    // Despawns gamobject on collision with trigger, also checks if it is the right trigger

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "floorCollider")
        {
            Destroy(transform.root.gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {        
        if (other.gameObject.name == "DespawnPoint")
        {
            Destroy(transform.root.gameObject);
        }
        if (other.gameObject.name == "DespawnPointBad")
        {
            transform.root.gameObject.GetComponent<FishGoodBad>().goodDespawn = false;
            Destroy(transform.root.gameObject);
        }
        if(other.gameObject.name == "belt")
        {
            //Turns gravity off for a bit to get the belt is started again
            gameObject.GetComponent<Rigidbody>().useGravity = false;
        }

    }

    void OnTriggerExit(Collider other)
    {
        //Turns gravity on again when belt is started to get the object to move on the belt
        gameObject.GetComponent<Rigidbody>().useGravity = true;
    }
    
}
