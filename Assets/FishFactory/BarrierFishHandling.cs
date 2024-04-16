using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierFishHandling : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float slideForce = 10f; // You can adjust this value to get the desired sliding effect

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
