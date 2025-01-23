using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    public float underWaterDrag = 3f;
    public float underWaterAngularDrag = 1f;
    public float airDrag = 0f;
    public float AirAngularDrag = 0.05f;
    public float floatingPower = 15f;
    public float waterHeight = 0f;
    private Rigidbody _rigidbody;
    bool underWater;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float differnece = transform.position.y - waterHeight;
        //is object under water?
        if(differnece < 0) {
            //push object up more and more depending on how deep it is
            _rigidbody.AddForceAtPosition (Vector3.up * floatingPower * Mathf.Abs(differnece), transform.position, ForceMode.Force);
            if(!underWater){
                underWater = true;
                SwitchState(true);
            }
        }
        else if(underWater) {
            underWater = false;
            SwitchState(false);
        }
    }

    void SwitchState (bool isUnderWater) {
        if(isUnderWater) {
            _rigidbody.drag = underWaterDrag;
            _rigidbody.angularDrag = underWaterAngularDrag;
        }
        else{
            _rigidbody.drag = airDrag;
            _rigidbody.angularDrag = AirAngularDrag;
        }
    }
}
