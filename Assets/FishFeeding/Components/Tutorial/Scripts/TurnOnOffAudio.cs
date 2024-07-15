using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class TurnOnOffAudio : MonoBehaviour
{
    public bool turnOnAudio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!turnOnAudio) 
        { 
            TurnOffAudio();
        } 
        else 
        { 
            TurnOnAudio(); 
        }
        
    }

    public void TurnOffAudio() 
    {
        foreach (var audioSource in GetComponentsInChildren<AudioSource>()) {
            audioSource.Stop();
        }

        foreach (var playAudio in GetComponentsInChildren<PlayAudio>()) {
            playAudio.ResetTrigger();
            playAudio.TurnOnTutorialAudio = false;
        }
    }

    public void TurnOnAudio() 
    {
        foreach (var playAudio in GetComponentsInChildren<PlayAudio>()) {
            playAudio.TurnOnTutorialAudio = true;
        }
    }
}
