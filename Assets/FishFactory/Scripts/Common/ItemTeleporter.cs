using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTeleporter : MonoBehaviour
{
    private Vector3 initialPosition;

    [SerializeField]
    private float radius = 5;

    private void Awake()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (Vector3.Distance(initialPosition, transform.position) > radius)
        {
            transform.position = initialPosition;
            Rigidbody body = GetComponent<Rigidbody>();
            if (body != null)
            {
                body.velocity = Vector3.zero;
            }
        }
    }
}
