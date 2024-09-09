using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterHandheldReset : MonoBehaviour
{
    private CounterHandheld CounterHandheld;
    // Start is called before the first frame update
    void Start()
    {
        CounterHandheld = transform.parent.GetComponent<CounterHandheld>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        // check if player finger tip
        if (other.name == "tip_collider_i")
        {
            CounterHandheld.ResetCounter();
        }
    }
}
