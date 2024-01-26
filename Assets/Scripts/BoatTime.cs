using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatTime : MonoBehaviour
{

    public boatPointOverview boatPointTimer;

    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.tag == "BoatPointArea")
        {
            //Passes the timer on to the parent so it is the same for every area
            boatPointTimer.timerOn = false;
            boatPointTimer.ipadBoatObjective(gameObject.name);
            


        } 
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "BoatPointArea")
        {
            
            boatPointTimer.timerOn = true;
            boatPointTimer.timerStarted = true;
            Destroy(gameObject);
        }
        }

}


