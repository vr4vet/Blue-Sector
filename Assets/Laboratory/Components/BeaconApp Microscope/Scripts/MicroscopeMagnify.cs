using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroscopeMagnify : MonoBehaviour
{
    [SerializeField] private MicroscopeMonitor MicroscopeMonitor;
    [SerializeField] private Magnification magnification;
    private enum Magnification
    {
        Magnify, Minimize
    }

    public void Magnify()
    {
        if (magnification == Magnification.Magnify)
            MicroscopeMonitor.Magnify();
        else
            MicroscopeMonitor.Minimize();
    }
}
