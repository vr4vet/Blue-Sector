using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroscopeTouchScreenButton : MonoBehaviour
{
    private bool IsTouched = false;
    //private List<int> Speeds= new List<int>(){1, 2, 3, 4, 5};
    //private int CurrentSpeedIndex = 0;
    private int CurrentSpeed = 1;
    [SerializeField] private Input input;
    private enum Input
    {
        Up, Down, Left, Right, Magnify, Minimize, Faster, Slower
    }
    private MicroscopeMonitor MicroscopeMonitor;

    private void Start()
    {
        MicroscopeMonitor = transform.root.GetComponent<MicroscopeMonitor>();
        MicroscopeMonitor.SetScrollSpeed(0.001f * CurrentSpeed); // default speed of 0.01 is too quick for a simple button press
    }

    private void FixedUpdate()
    {
        if (IsTouched)
        {
            switch(input)
            {
                case Input.Left:
                    MicroscopeMonitor.ScrollLeft();
                    break;
                case Input.Right:
                    MicroscopeMonitor.ScrollRight();
                    break;
                case Input.Up:
                    MicroscopeMonitor.ScrollUp();
                    break;
                case Input.Down:
                    MicroscopeMonitor.ScrollDown();
                    break;
                default:
                    break;
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // check if player finger tip
        if (other.name == "tip_collider_i")
        {
            IsTouched = true;
                
            switch (input)
            {
                case Input.Magnify:
                    MicroscopeMonitor.Magnify();
                    break;
                case Input.Minimize:
                    MicroscopeMonitor.Minimize();
                    break;
                case Input.Faster:
                    if (CurrentSpeed < 5)
                        CurrentSpeed += 1;
                    MicroscopeMonitor.SetScrollSpeed(0.001f * CurrentSpeed, CurrentSpeed);
                    Debug.Log("Faser: " + CurrentSpeed);
                    break;
                case Input.Slower:
                    if (CurrentSpeed > 0)
                        CurrentSpeed -= 1;
                    MicroscopeMonitor.SetScrollSpeed(0.001f * CurrentSpeed, CurrentSpeed);
                    break;
                default:
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // check if player hand
        if (other.name == "tip_collider_i")
        {
            IsTouched = false;
        }
    }
}
