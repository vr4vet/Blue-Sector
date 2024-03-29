using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpeedboatScript : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject advancedOcean;
    private AdvancedOcean advancedOceanScript;
    void Start()
    {
        advancedOcean = GameObject.Find("Ocean");
        advancedOceanScript = advancedOcean.GetComponent<AdvancedOcean>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, advancedOceanScript.GetWaterHeight(transform.position) *0.95f - 0.05f, transform.position.z);
        //new Vector3(transform.position.x, advancedOceanScript.GetWaterHeight(transform.position), transform.position.z);
        //new Vector3(transform.position.x, advancedOceanScript.GetWaterHeight(new Vector3(transform.position.x * 16, transform.position.y, transform.position.z * 16))); 
    }
}
