using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadfishCollide : MonoBehaviour
{
    [SerializeField] private WatchManager watchManager;
    [SerializeField] private Task.Subtask subtask;
    [SerializeField] private AudioClip splash;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Deadfish"))
        {
            watchManager.CompleteStep(subtask.GetStep("Push the dead fish into the tub"));
            PlayAudio();
        }
    }

    public void PlayAudio()
    {

        //if the gameobject has audiosource
        if (TryGetComponent<AudioSource>(out AudioSource audioSource))
        {
            audioSource.Stop();
            audioSource.PlayOneShot(splash);
            return;
        }

        //otherwise create audiosource
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.PlayOneShot(splash);

    }
}
