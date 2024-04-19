using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHand : MonoBehaviour
{
    private bool returnToStart = false;
    private float timer;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = new Vector3(20.99939727783203f, 1.2670998573303223f, -64.20687866210938f);
        // targetPosition = new Vector3(0.267800003f, 0.506399989f, 0.155100003f);
        targetPosition = new Vector3(20.9647216796875f, 0.9584999084472656f, -64.42521667480469f);
    }

    // Update is called once per frame
    // void Update()
    // {
    //     GameObject.transform.position = Vector3.MoveTowards(GameObject.transform.position, Vector3(0.267800003,0.506399989,0.155100003), 1f);
    // }

    void Update()
    {
        float distanceToMove = Time.deltaTime * 0.2f;

        // if (timer <= 0)
        // {
        if (!returnToStart)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, distanceToMove);

            // Target reached?  If so, start moving back to the original position
            if (Vector3.Distance(transform.position, targetPosition) <= Mathf.Epsilon)
            {
                returnToStart = true;
                // this.timer = this.chargeRate;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, distanceToMove);

            // Original position reached?  If so, start moving to the target
            if (Vector3.Distance(transform.position, startPosition) <= Mathf.Epsilon)
            {
                returnToStart = false;
                // this.timer = this.chargeRate;
            }
        }
        // }
        // else
        // {
        //     this.timer -= Time.time;
        // }
    }
}
