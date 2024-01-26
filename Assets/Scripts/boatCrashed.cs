using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boatCrashed : MonoBehaviour {
    public boatPointOverview crashed;

    void OnCollisionEnter(Collision collision)
    {
        crashed.crashed = 0;
    }

}
