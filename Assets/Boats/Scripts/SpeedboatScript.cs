using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpeedboatScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + (Mathf.Sin(Time.time) * 0.0005f), transform.position.z);
        //new Vector3(transform.position.x, advancedOceanScript.GetWaterHeight(transform.position), transform.position.z);
        //new Vector3(transform.position.x, advancedOceanScript.GetWaterHeight(new Vector3(transform.position.x * 16, transform.position.y, transform.position.z * 16))); 
    }
}
