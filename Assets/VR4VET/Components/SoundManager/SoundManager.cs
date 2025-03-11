using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioSource _musicSource, _effectsSource, _voiceSource;
    private List<AudioSource> _voiceSoures;
    //Singleton function (only an instance of this class will run )
     void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Search for text-to-speech audio sources in scene after a scene is loaded
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        _voiceSoures = GameObject.FindObjectsOfType<AudioSource>().ToList().FindAll(source => source.name == "TTSSpeakerAudio");
    }

    //Play Functions
    public void PlayEffect(AudioClip clip)
    {
        _effectsSource.PlayOneShot(clip);
    }

    public void PlayVoice(AudioClip clip)
    {
        _voiceSource.PlayOneShot(clip);
    }


    public void PlayMusic(AudioClip clip)
    {
        _musicSource.PlayOneShot(clip);
    }


    //Change Volume Functions

    public void ChangeMasterVolume (float value)
    {
        AudioListener.volume = value;
    }

    public void ChangeMusicVolume(float value)
    {
        _musicSource.volume = value;
    }

    public void ChangeEffectVolume(float value)
    {
        _effectsSource.volume = value;
    }
    public void ChangeVoiceVolume(float value)
    {
        if (_voiceSoures.Count > 0)
        {
            foreach (AudioSource source in _voiceSoures)
                source.volume = value;
        }
        else
            _voiceSource.volume = value;
    }

//getter for volume
//

    public float GetMasterVolume()
    {
        return AudioListener.volume;
    }

    public float GetMusicVolume()
    {
        return AudioListener.volume;
    }

    public float GetEffectsVolume()
    {
        return AudioListener.volume;
    }
    public float GetVoiceVolume()
    {
        return AudioListener.volume;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
