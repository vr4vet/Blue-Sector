using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroscopeMagnify : MonoBehaviour
{
    [SerializeField] private MicroscopeMonitor MicroscopeMonitor;

    public void Magnify()
    {
        MicroscopeMonitor.Magnify(); 
    }
}
