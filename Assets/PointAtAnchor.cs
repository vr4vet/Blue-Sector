using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAtAnchor : MonoBehaviour
{
    // public GameObject reticle;
    public Material mat;
    // public GameObject anchor;
    // private bool pointingAtAnchor = false;

    // Update is called once per frame
    void Update()
    {
        // CheckBeamCollision();
        // OnCollisionEnter();
    }

    // void CheckBeamCollision()
    // {
    //     RaycastHit hit;
    //     if (Physics.Raycast(transform.position, transform.forward, out hit))
    //     {
    //         if (hit.collider.CompareTag("TeleportAnchor"))
    //         {
    //             reticle.SetActive(false);
    //             Debug.Log("treffer");
    //         }
    //         reticle.SetActive(true);
    //         Debug.Log("treffer, men ikke riktig");
    //     }
    //     reticle.SetActive(true);
    //     Debug.Log("nei");
    // }

    // void OnCollisionEnter(Collision c)
    // {
    //     if (c.gameObject.tag == "TeleportAnchor")
    //     {
    //         reticle.SetActive(false);
    //         halo.GetComponent<Renderer>().material.color = Color.green;
    //     }
    //     else
    //     {
    //         reticle.SetActive(true);
    //         halo.GetComponent<Renderer>().material.color = Color.blue;
    //     }
    // }

    void OnTriggerEnter(Collider c)
    {
        // mat.color = Color.red;
        Debug.Log("Colloding");
    }
}
