using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.OpenXR.Input;
using UnityEngine.InputSystem;
using TMPro;
using BNG;
using System;

public class AddInstructionsToWatch : MonoBehaviour
{
    public TextMeshPro textMesh;

    [SerializeField] private AudioClip incoming;

    [Tooltip("Text to display"), TextArea(5, 20)]
    public string text;


    public void addInstructions()
    {
        // Next line uses the update InputSustem of unity. This should be used in the future, but because of time constraints the old version was used
        // OpenXRInput.SendHapticImpulse(lefthand, 1, 1, 1, UnityEngine.InputSystem.XR.XRController.leftHand);

        var device = UnityEngine.InputSystem.XR.XRController.leftHand;
        var command = UnityEngine.InputSystem.XR.Haptics.SendHapticImpulseCommand.Create(0, 0.3f, 1);
        device.ExecuteCommand(ref command);

        textMesh.SetText(text);

        PlayAudio();
    }

    public void emptyInstructions()
    {
        textMesh.SetText("");
    }


    public void PlayAudio()
    {
        //if the gameobject has audiosource
        if (TryGetComponent<AudioSource>(out AudioSource audioSource))
        {
            audioSource.Stop();
            audioSource.PlayOneShot(incoming);
            return;
        }

        //otherwise create audiosource
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.PlayOneShot(incoming);
    }
}