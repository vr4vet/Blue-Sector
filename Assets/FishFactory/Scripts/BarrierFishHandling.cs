using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierFishHandling : MonoBehaviour
{
    [Tooltip("The force applied to the collsion object when it hits the barrier")]
    [SerializeField] 
    private float _slideForce = 10f; 

    void OnCollisionEnter(Collision collision)
    {
        Rigidbody rigidbody = collision.gameObject.GetComponent<Rigidbody>();

        if (rigidbody != null)
        {
            Vector3 direction = collision.contacts[0].point - transform.position;
            direction = -direction.normalized;
            rigidbody.AddForce(direction * _slideForce, ForceMode.Impulse);
        }
    }
}
