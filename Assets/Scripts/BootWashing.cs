using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BootWashing : MonoBehaviour {

    public bool hasStartedBelt;
    public bool hasWashed;
    public AddPoints points;
    public Toggle toggleBefore;
    public Toggle toggleAfter;

    // Detects that fillet-belt has been started, and knows to switch to the 2nd wash-objective
    public void StartedBelt()
    {
        hasStartedBelt = true;
        hasWashed = false;
    }

    public void WashBoots()
    {
        if (!hasStartedBelt && !hasWashed)
        {
            points.AddHMS(3);
            toggleBefore.isOn = true;
            hasWashed = true;
        }
        if (hasStartedBelt && !hasWashed)
        {
            points.AddHMS(3);
            toggleAfter.isOn = true;
            hasWashed = true;
        }
    }

}
