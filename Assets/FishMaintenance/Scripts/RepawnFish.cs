using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepawnFish : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Deadfish"))
        {
            other.GetComponent<Transform>().position = new Vector3(-0.5608958f,1.0359474f,-2.769325f);
        }
    }
}
