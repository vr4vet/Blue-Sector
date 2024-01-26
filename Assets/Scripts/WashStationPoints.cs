using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WashStationPoints : MonoBehaviour {

    public bool usedSoap = false;
    public bool washed = false;
    public AddPoints points;
    public Toggle toggle1;

    public void UsedSoap()
    {
        StartCoroutine("SoapActive");
    }
    // If water is used within 10 seconds of the soap, give points
    public void UsedWater()
    {
        if (usedSoap == true && washed == false)
        {
            washed = true;
            points.AddHMS(3);
            toggle1.isOn = true;
        }
    }
    // Sets that the soap has been used for 10 seconds
    IEnumerator SoapActive()
    {
        usedSoap = true;
        yield return new WaitForSeconds(10);
        usedSoap = false;
    }
}
