using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterHandheldIncrementButton : MonoBehaviour
{
    private float NeutralPosition;
    private bool Descending, Ascending = false;
    private AudioSource Click;

    // Start is called before the first frame update
    void Start()
    {
        NeutralPosition = transform.localPosition.x;
        Click = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (Descending)
        {
            if (transform.localPosition.x < NeutralPosition + 0.005)
                transform.localPosition += new Vector3(0.0005f, 0, 0);
            else
            {
                Click.time = 0.1f; // skip the silence at the start of clip
                Click.Play();
                Descending = false;
                Ascending = true;
            }
        }
        if (Ascending)
        {
            if (transform.localPosition.x > NeutralPosition)
                transform.localPosition += new Vector3(-0.0005f, 0, 0);
            else
                Ascending = false;
        }
    }

    public void DepressButton()
    {
        Descending = true;
        Ascending = false;
    }
}
