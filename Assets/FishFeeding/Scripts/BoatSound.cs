using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSound : MonoBehaviour
{
    //[SerializeField]
    private PlayAudio playAudio;
    //[SerializeField]
    private SceneLoader sceneLoader;

    // Start is called before the first frame update
    void Start()
    {
        playAudio = GetComponent<PlayAudio>();
        sceneLoader = GetComponent<SceneLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudioThenTransition()
    {
        GetComponent<AudioSource>().Play();
        Invoke("TransitionScene", 4.0f);
    }

    private void TransitionScene() 
    {
        sceneLoader.LoadScene("FishWelfare");
    }

}
