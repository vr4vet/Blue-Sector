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


    // emptyInstructions();
    // Next line uses the update InputSustem of unity. This should be used in the future, but because of time constraints the old version was used
    // OpenXRInput.SendHapticImpulse(lefthand, 1, 1, 1, UnityEngine.InputSystem.XR.XRController.leftHand);
    public void addInstructions(string text)
    {
        StartCoroutine(sendHaptics());

        textMesh.SetText(text);

        PlayAudio();
    }

    IEnumerator sendHaptics()
    {
        var device = UnityEngine.InputSystem.XR.XRController.leftHand;
        var command = UnityEngine.InputSystem.XR.Haptics.SendHapticImpulseCommand.Create(0, 1f, 0.7f);
        device.ExecuteCommand(ref command);
        yield return new WaitForSeconds(0.8f);
        device.ExecuteCommand(ref command);
        yield return new WaitForSeconds(0.8f);
        device.ExecuteCommand(ref command);
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

    public string getText()
    {
        return textMesh.GetComponent<TMP_Text>().text;
    }
}