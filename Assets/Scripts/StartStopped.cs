using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStopped : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //Stops the particlesystem on attached object
        gameObject.GetComponent<ParticleSystem>().Stop();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
