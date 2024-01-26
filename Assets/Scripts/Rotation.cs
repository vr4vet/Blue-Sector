using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {
    public Transform playerTrans;

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        // Tracks the movement of the player's head and rotates the torso accordingly
        float yRotation = playerTrans.eulerAngles.y;
        gameObject.transform.position = playerTrans.position + new Vector3(0f,-1.5f,0);
        gameObject.transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
    }
}
