using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewCylinderScript : MonoBehaviour
{
    public float radius = 50.0f;
    public float height = 100.0f;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.localScale = new Vector3(radius, height, radius);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
