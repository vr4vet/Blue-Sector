using UnityEngine;
using System.Collections;

public class conveyorMove : MonoBehaviour {

	public bool beltOn = false;
    public float speed;
    public Vector3 newPos = new Vector3(0f,0f,0f);
    public GameObject audioClip;
    public GameObject canvas;




    // Update is called once per frame
    void FixedUpdate()
    {
    }
    // Used to get objects to move when you turn on the belt when they lie still
    IEnumerator Timer()
    {
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
        yield return new WaitForSeconds(0.01f);
        gameObject.GetComponent<BoxCollider>().isTrigger = false;
    }

    // Switches belt on or off
    public void switchBelt(){
		if (beltOn) {
		    beltOn = false;
            audioClip.GetComponent<AudioSource>().enabled = false;
            canvas.SetActive(true);
        }
        else
        {
			beltOn = true;
            canvas.SetActive(false);
            audioClip.GetComponent<AudioSource>().enabled = true;
            StartCoroutine(Timer());  
        }
						

    }
    
    // Moves colliding object id belt is on
    void OnCollisionStay(Collision col)
    {
            moveObject(col);
    }
    // Moves colliding object
    void moveObject(Collision col)
        {

            if (beltOn)
            {
                
                GameObject moveObject = col.gameObject;

                Vector3 movepos = newPos.normalized * speed * Time.deltaTime;


                //moveObject.GetComponent<Rigidbody>().MovePosition(moveObject.transform.position + newPos);
                moveObject.transform.root.position += movepos;
            }
        }
    
}
