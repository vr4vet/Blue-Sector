using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayAudio : MonoBehaviour
{
    private AudioSource audioSource;
    private bool triggered;
    public bool TurnOnTutorialAudio { get; set; }
    public bool validTeleportTrigger { get; set; }

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

        AudioBoxColliderTriggered.Invoke(this);

        if (TurnOnTutorialAudio && validTeleportTrigger && triggered && !audioSource.isPlaying) {
            audioSource.Play();
            Invoke(nameof(ResetTrigger), audioSource.clip.length);
        }
    }

    /// <summary>
    /// Gets an event which is raised when the box collider to the audio is triggered.
    /// </summary>
    public UnityEvent<PlayAudio?> AudioBoxColliderTriggered { get; } = new();

}
