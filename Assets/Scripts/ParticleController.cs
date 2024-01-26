using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{

    void Start()
    {
        //gameObject.GetComponent<ParticleSystem>().Emit(0);
        gameObject.GetComponent<ParticleSystem>().Stop();
    }
    // Emits particles for 2 seconds
    void Update()
    {
        if (gameObject.GetComponent<ParticleSystem>().time > 2)
        {
            gameObject.GetComponent<ParticleSystem>().Stop();
        }
    }
    public void ParticlesEmit()
    {
        gameObject.GetComponent<ParticleSystem>().Play();
    }
}