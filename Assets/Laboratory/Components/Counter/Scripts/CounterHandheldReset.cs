using UnityEngine;

public class CounterHandheldReset : MonoBehaviour
{
    private CounterHandheld CounterHandheld;
    private bool Descending, Ascending = false;
    private float ResetButtonNeutralPosition;
    private AudioSource Click;
    private bool Clicked = false;

    // Start is called before the first frame update
    void Start()
    {
        CounterHandheld = transform.parent.GetComponent<CounterHandheld>();
        ResetButtonNeutralPosition = transform.localPosition.y;
        Click = GetComponent<AudioSource>();
    }


    private void FixedUpdate()
    {
        if (Descending)
        {
            if (transform.localPosition.y > ResetButtonNeutralPosition - 0.005)
                transform.localPosition += new Vector3(0, -0.0005f, 0);
            else
            {
                if (!Clicked)
                {
                    Clicked = true; // prevent rapid clicks when button is held down
                    Click.time = 0.1f; // skip the silence at the start of clip
                    Click.Play();
                    CounterHandheld.ResetCounter();
                }
            }
        }
        if (Ascending)
        {
            Clicked = false;
            if (transform.localPosition.y < ResetButtonNeutralPosition)
                transform.localPosition += new Vector3(0, 0.0005f, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // check if player finger tip
        if (other.name == "tip_collider_i")
        {
            Descending = true;
            Ascending = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // check if player finger tip
        if (other.name == "tip_collider_i")
        {
            Descending = false;
            Ascending = true;
        }
    }
}
