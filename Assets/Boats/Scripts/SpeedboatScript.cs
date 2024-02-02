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
        // multiply by Time.timeScale to dip boat with speed proportionally to the current game speed. prevents floating/sinking when opening the pause menu.
        transform.position = new Vector3(transform.position.x, transform.position.y + (Mathf.Sin(Time.time * Time.timeScale) * 0.0005f), transform.position.z);
    }
}
