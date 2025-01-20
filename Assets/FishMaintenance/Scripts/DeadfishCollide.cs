using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadfishCollide : MonoBehaviour
{
    [SerializeField] private MaintenanceManager mm;
    [SerializeField] private Task.Subtask subtask;
    [SerializeField] private AudioClip splash;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Deadfish"))
        {
            mm.CompleteStep(subtask.GetStep("Push the dead fish into the tub"));
            //other.GetComponent<Outline>().enabled = false;
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
