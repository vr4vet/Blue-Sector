using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpeedboatScript : MonoBehaviour
{
    private float initialYPosition;
    // Start is called before the first frame update
    void Start()
    {
        initialYPosition = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // multiply by Time.timeScale to dip boat with speed proportionally to the current game speed. prevents floating/sinking when opening the pause menu.
        transform.position = new Vector3(transform.position.x, initialYPosition + (Mathf.Sin(Time.time * Time.timeScale) * 0.05f), transform.position.z);
    }
}
