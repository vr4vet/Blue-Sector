using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierFishHandling : MonoBehaviour
{
    // You can adjust this value to get the desired sliding effect
    [SerializeField] float slideForce = 10f; 

    void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 direction = collision.contacts[0].point - transform.position;
            direction = -direction.normalized;

            rb.AddForce(direction * slideForce, ForceMode.Impulse);
        }
    }
}
