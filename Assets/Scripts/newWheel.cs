using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;


public class newWheel : MonoBehaviour {
    public CircularDrive OutputDataFromSteeringWheel;
    private GameObject boat;
    private GameObject engine2;
    private GameObject engine1;
    public Rigidbody rb;
	private float wheelTurn;
	public float resetFactor;

    private float x;
    public float lastValue;
    private float turnLimit;

    // Use this for initialization
    void Start () {
        boat = GameObject.Find("speedBoat");
        rb = boat.GetComponent<Rigidbody>();
		// How fast the wheel resets to regular position
		resetFactor = 1f;
		wheelTurn = 0;
        turnLimit = 90;
        engine1 = GameObject.Find("Engine1");
        engine2 = GameObject.Find("Engine2");
    }

    public IEnumerator TurnWheel(GameObject wheel, float angle)
    {
        gameObject.transform.Rotate(-angle, 0, 0, Space.Self);
        engine1.transform.Rotate(0, angle/3.5f, 0, Space.Self);
        engine2.transform.Rotate(0, angle/3.5f, 0, Space.Self);
        yield return null;
    }

    // Update is called once per frame


    void FixedUpdate() { 

        //  Vector2 touchAxis = OutputDataFromSteeringWheel gets axis of touchpad and only takes x value.
        x = OutputDataFromSteeringWheel.touchAxis.x;
        x = x * 0.5f;
        lastValue = wheelTurn;
        wheelTurn += x;
        // Statements for rotating wheel back to normal position smoothly when released, and else statement for rotating the boat
        if (wheelTurn == lastValue && wheelTurn > -resetFactor && wheelTurn < resetFactor)
        {
            
            StartCoroutine(TurnWheel(gameObject, wheelTurn));
            boat.transform.Rotate(0, wheelTurn, 0, Space.World);
            wheelTurn = 0;
        }

        else if (wheelTurn == lastValue && wheelTurn > 0)
        {
            wheelTurn -= resetFactor;
            StartCoroutine(TurnWheel(gameObject, -resetFactor));
          //  boat.transform.Rotate(0, -resetFactor, 0, Space.World);

        }
        else if (wheelTurn == lastValue && wheelTurn < 0)
        {
            wheelTurn += resetFactor;
            StartCoroutine(TurnWheel(gameObject, resetFactor));
           // boat.transform.Rotate(0, resetFactor, 0, Space.World);
        }

        else
        {
            if (wheelTurn >= turnLimit)
            {
                wheelTurn = turnLimit; 
            }
            else if (wheelTurn <= -turnLimit)
            {
                wheelTurn = -turnLimit;
            }
            if(wheelTurn != turnLimit && wheelTurn != -turnLimit) {
                StartCoroutine(TurnWheel(gameObject, x));
            
            }
            
        }
        boat.transform.Rotate(0, wheelTurn/(2*turnLimit), 0, Space.World);
    }
}
