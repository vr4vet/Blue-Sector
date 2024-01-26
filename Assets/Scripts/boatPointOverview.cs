using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class boatPointOverview : MonoBehaviour {
    float timer;
    public bool timerOn;
    public TotalPoints total;
    public bool timerStarted;
    public int crashed;
    public Toggle boatObjective1;
    public Toggle boatObjective2;
    public Toggle boatObjective3;

    void Start() {
        timerOn = false;
        total.baat = 0;
        crashed = 5;
     
      
    }

    // Update is called once per frame
    void Update() {
        if (timerOn) {
            timer += Time.deltaTime;
        }

    }

    public void ipadBoatObjective(string area) {
        if (area == "boatPointCube") {
            boatObjective1.isOn = true;
            
        }
        if (area == "boatPointCube (1)") {
            boatObjective2.isOn = true;
            total.baat += 5;
            total.baat += crashed;
            if (timer <= 60)
            {
                total.baat += 5;
            }
        }
        if (area == "boatPointCube (2)") {
            boatObjective3.isOn = true;
            total.baat += 5;
            total.baat += crashed;
            if (timer <= 60)
            {
                total.baat += 5;
            }
        }
    }
}
