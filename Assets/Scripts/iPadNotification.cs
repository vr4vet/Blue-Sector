using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iPadNotification : MonoBehaviour {

    public GameObject screen;
    public GameObject startScreen;
    public bool notifying;
    private void Start()
    {
        notifying = false;
        screen.SetActive(false);
        StartCoroutine(Notification());
        
    }

    private void Update()
    {
        if (startScreen.activeInHierarchy)
        {
            //notifying = false;
            screen.SetActive(false);
        }
    }

    public IEnumerator Notification()
    {
        while (notifying)
        {
            screen.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            screen.SetActive(false);
            yield return new WaitForSeconds(0.3f);
        }
        
        
    }
}
