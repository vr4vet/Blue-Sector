using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroscopeTouchScreenArrow : MonoBehaviour
{
    private bool IsTouched = false;
    [SerializeField] private Input input;
    private enum Input
    {
        Up, Down, Left, Right, Plus, Minus
    }
    private MicroscopeMonitor MicroscopeMonitor;

    private void Start()
    {
        MicroscopeMonitor = transform.root.GetComponent<MicroscopeMonitor>();
        MicroscopeMonitor.SetScrollSpeed(0.001f); // default speed of 0.01 is too quick for a simple button press
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
        // check if player hand
        if (other.name == "Grabber")
        {
            IsTouched = true;

            if (input == Input.Plus)
                MicroscopeMonitor.Magnify();
            else if (input == Input.Minus)
                MicroscopeMonitor.Minimize();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // check if player hand
        if (other.name == "Grabber")
        {
            IsTouched = false;
        }
    }
}
