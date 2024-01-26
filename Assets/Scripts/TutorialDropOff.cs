using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDropOff : MonoBehaviour {

 

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter");
        if (other.gameObject.tag == "Cuttable"){
            TutorialOverview.count += 1;
            Debug.Log("enter fish");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("exit");
        if (other.gameObject.tag == "Cuttable")
        {
            Debug.Log("exit fish");
            TutorialOverview.count -= 1;
        }
    }

}
