using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points : MonoBehaviour {

    public TotalPoints total;
    public int dyktighet;
    public int tilsyn;
    public int noyaktighet;
    public int fysikk;
    public int baat;
    public int hms;
    bool wrong = false;

    private void Start()
    {
        total = GameObject.Find("PlayerScore").GetComponent<TotalPoints>();
    }
    // Gives points on destroy
    private void OnDestroy()
    {
        if (!wrong)
        {
            total.dyktighet += dyktighet;
            total.tilsyn += tilsyn;
            total.noyaktighet += noyaktighet;
            total.fysikk += fysikk;
            total.baat += baat;
            total.hms += hms;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Negative point if goodfish is placed wrong, plus points if badfish is placed right
        if (other.gameObject.name == "DespawnPointBad")
        {
            dyktighet *= -1;
            tilsyn *= -1;
            noyaktighet *= -1;
            fysikk *= -1;
            baat *= -1;
            hms *= -1;
        }

        //For Box sorting
        if (other.gameObject.name == "OutsideBoxDetection")
        {
            Debug.Log("I'm outside the box!");
            wrong = true;
            total.dyktighet -= dyktighet;
            total.tilsyn -= tilsyn;
            total.noyaktighet -= noyaktighet;
            total.fysikk -= fysikk;
            total.baat -= baat;
            total.hms -= hms;
        }
        
        

    }
}
