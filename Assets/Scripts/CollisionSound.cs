using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour {

    // Plays the gameobjects  sound when the collision is above a certain velocity
    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 2)
            GetComponent<AudioSource>().Play();
    }
}
