using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroscopeTouchScreenButton : MonoBehaviour
{
    private bool IsTouched = false;
    [SerializeField] private Input input;
    private enum Input
    {
        Up, Down, Left, Right, Magnify, Minimize, Faster, Slower
    }
    private MicroscopeMonitor MicroscopeMonitor;

    private void Start()
    {
        MicroscopeMonitor = transform.root.GetComponentInChildren<MicroscopeMonitor>();
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
            //MicroscopeMonitor.SetScrollSpeedConstant(0.001f); // default speed of 0.01 is too quick for a simple button press
            //MicroscopeMonitor.SetScrollSpeed();

            IsTouched = true;
                
            switch (input)
            {
                case Input.Magnify:
                    if (!MicroscopeMonitor.RevolvingNosePiece.IsRotating())
                        MicroscopeMonitor.Magnify();
                    break;
                case Input.Minimize:
                    if (!MicroscopeMonitor.RevolvingNosePiece.IsRotating())
                        MicroscopeMonitor.Minimize();
                    break;
                case Input.Faster:
                    MicroscopeMonitor.IncreaseScrollSpeed();
                    break;
                case Input.Slower:
                    MicroscopeMonitor.DecreaseScrollSpeed();
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
