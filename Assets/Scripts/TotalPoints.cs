using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalPoints : MonoBehaviour {
    // Keeps track of all the points and displays it on the tablet
    public int totalPoints;
    public int dyktighet;
    public int tilsyn;
    public int noyaktighet;
    public int fysikk;
    public int baat;
    public int hms;
    public GameObject totalText;
    public GameObject dyktighetText;
    public GameObject tilsynText;
    public GameObject noyaktighetText;
    public GameObject fysikkText;
    public GameObject baatText;
    public GameObject hmsText;

    // Use this for initialization
    void Start () {
		dyktighet = tilsyn = noyaktighet = fysikk = baat = hms = 0;
	}
	
	// Update is called once per frame
	void Update () {

        totalPoints = dyktighet + tilsyn + noyaktighet + fysikk + baat + hms;
        totalText.GetComponent<Text>().text = "Total score: " + totalPoints;
        dyktighetText.GetComponent<Text>().text = "Dyktighet: " + dyktighet;
        tilsynText.GetComponent<Text>().text = "Tilsyn av fisk og noter: " + tilsyn;
        noyaktighetText.GetComponent<Text>().text = "Nøyaktighet: " + noyaktighet;
        fysikkText.GetComponent<Text>().text = "God fysikk og \n hands - on evne:" + fysikk;
        baatText.GetComponent<Text>().text = "Kjøring av båt score: " + baat;
        hmsText.GetComponent<Text>().text = "HMS og mattrygghet: " + hms;	
	}

}
