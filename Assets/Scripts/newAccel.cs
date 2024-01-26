using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;
public class newAccel : MonoBehaviour {
	public LinearMapping OutputDataFromAccel;
    public SteamVR_Controller touchpad;
    private GameObject boat;
    public Rigidbody rb;
    public float speed = 10f;
	public float angle = 0.5f;
	public float prevAngle = 0.5f;
	public float accelTurnAngle = 0;

    public AudioSource source;
    private float volLowRange = 0.5f;
    private float volHighRange = 1f;

    // Use this for initialization
    void Start () {
        boat = GameObject.Find("speedBoat");
		rb = boat.GetComponent<Rigidbody> ();
		OutputDataFromAccel = GetComponent<LinearMapping>();

        source = GetComponent<AudioSource>();
    }
	public IEnumerator TurnAccel(GameObject accel, float angle)
	{
		gameObject.transform.Rotate(0, 0, -angle, Space.Self);
		accelTurnAngle += angle;
		Debug.Log (accelTurnAngle);
		yield return null;
	}
		
	
	// Update is called once per frame
	void FixedUpdate () {

        if (/*source.time >= 3.7f ||*/ !source.isPlaying)
        {
            source.Stop();
            source.Play();
            //source.time = 2.9f;
           

        }

        prevAngle = angle; 
		angle = OutputDataFromAccel.value;
		if (angle > 0.55f) {
			rb.AddForce (transform.right * speed * angle);
            source.volume = 0.7f;
            
           
            source.pitch = angle;
            
         



            //if (accelTurnAngle >= -45 && prevAngle != angle) {
            //StartCoroutine (TurnAccel (gameObject, angle));
            //}


        } else if (angle < 0.35f) {
			rb.AddForce (-transform.right * speed * angle);
            source.pitch = 1f - angle;
            if (angle == 0) {
               
                
                
             
                rb.AddForce (-transform.right * speed * 0.5f);
                //if (accelTurnAngle <= 45 && prevAngle != angle) {
                //	StartCoroutine (TurnAccel (gameObject, -1));
                //}

            }
			//else if (accelTurnAngle <= 45 && prevAngle != angle) {
				//StartCoroutine (TurnAccel (gameObject, -angle));
			//}



        
		}
        else
        {

            source.pitch = 0;
            source.volume = 0.4f;
          
        }
    }
}

