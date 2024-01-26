using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPoints : MonoBehaviour {

    // Imports the script that contains total points
    public TotalPoints total;


    //Functions to be used by other gameobjects to add points to the total score
    public void AddDyktighet(int points)
    {
        total.dyktighet += points;
    }
    public void AddTilsyn(int points)
    {
        total.tilsyn += points;
    }
    public void AddNoyaktighet(int points)
    {
        total.noyaktighet += points;
    }
    public void AddFysikk(int points)
    {
        total.fysikk += points;
    }
    public void AddBaat(int points)
    {
        total.baat += points;
    }
    public void AddHMS(int points)
    {
        total.hms += points;
    }

}
