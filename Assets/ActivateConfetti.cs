using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateConfetti : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {



    }

    // Update is called once per frame



    void OnEnable()
    {
        foreach (Transform child in gameObject.transform)
        {

            child.GetComponent<ParticleSystem>().Play();

        }
    }
}
