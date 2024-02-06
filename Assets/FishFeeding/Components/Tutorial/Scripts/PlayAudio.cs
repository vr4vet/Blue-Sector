using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    private AudioSource audioSource;
    private bool triggered;
    public bool TurnOnTutorialAudio { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        TurnOnTutorialAudio = true;
    }

    // Update is called once per frame
    void Update()
    {
 
    }

    public void ResetTrigger() 
    {
        triggered = false;
    }

    public void Trigger() 
    {
        if (!triggered) 
        {
            GetComponentInParent<TurnOnOffAudio>().TurnOffAudio();
            GetComponentInParent<TurnOnOffAudio>().TurnOnAudio();
            triggered = true;
        }

        if (TurnOnTutorialAudio && triggered && !audioSource.isPlaying) {
            audioSource.Play();
            Invoke(nameof(ResetTrigger), audioSource.clip.length);
        }
    }
}
