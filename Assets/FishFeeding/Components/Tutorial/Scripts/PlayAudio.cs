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
        if (TurnOnTutorialAudio && triggered && !audioSource.isPlaying) 
        {
            audioSource.Play();
            Invoke(nameof(ResetTrigger), audioSource.clip.length);
            //Debug.Log(audioSource.clip.length);
        }
    }

/*    private void PlayAudioSource() 
    {
        audioSource.Play();
    }*/

    public void ResetTrigger() 
    {
        triggered = false;
    }

    public void Trigger() 
    {
        triggered = true;
    }
}
