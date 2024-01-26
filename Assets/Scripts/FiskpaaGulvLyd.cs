using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiskpaaGulvLyd : MonoBehaviour {
    public AudioSource lyd;


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Cuttable")
        {
           
            Debug.Log("inside if");
            lyd.Play();
        }
        
    }
}
