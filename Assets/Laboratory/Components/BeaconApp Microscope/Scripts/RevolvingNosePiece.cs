using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolvingNosePiece : MonoBehaviour
{
    [SerializeField] private MicroscopeMonitor MicroscopeMonitor;

    private void OnTriggerEnter(Collider other)
    {
        // check if player hand
        if (other.name == "Grabber")
        {
            MicroscopeMonitor.Magnify();
        }    
    }
}
