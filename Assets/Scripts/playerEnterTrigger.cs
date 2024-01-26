using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerEnterTrigger : MonoBehaviour {
    //public GameObject screenToShow;
    public iPadNotification notifyer;
    public  GameObject screenToDisplay;
    public GameObject startScreen;

    public void changeBool()
    {
        if (notifyer.notifying)
        {
            notifyer.notifying = false;           
        }
        
    }

    public void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.tag == "playerCollider")
        {
            //Passes the timer on to the parent so it is the same for every area
            notifyer.notifying = true;
            //screenToShow.SetActive(false);
            //notifyer.notifying = true;
            StartCoroutine(notifyer.Notification());
            screenToDisplay.SetActive(true);
            startScreen.SetActive(false);

        }
    }
    public void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "playerCollider")
        {
            screenToDisplay.SetActive(false);
            startScreen.SetActive(true);
            notifyer.notifying = false;
            Destroy(gameObject);
        }
    }
}
