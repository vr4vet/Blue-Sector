using Meta.WitAi.TTS.Utilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TextToSpeechButton : MonoBehaviour
{
    [Tooltip("The root of the object which contains text content to be read by text-to-speech. This script will attempt to fetch the text content automatically")]
    [SerializeField] private GameObject TargetObject;

    [Tooltip("Point to the TTS prefab here")]
    [SerializeField] private GameObject TTSSpeaker;
    private TTSSpeaker _speaker;

    [Tooltip("The speaker image that is placed somewhere on this object.")]
    [SerializeField] private Image TargetSpeakerImage;
    private Sprite _defaultSpeakerImage;
    private Sprite _speakerImageFull;
    private Sprite _speakerImageMedium;
    private Sprite _speakerImageSilent;

    [Tooltip("Strings added to this list will be removed from the text that is acquired automatically.")]
    [SerializeField] private List<string> ExcludeStrings = new List<string>();

    [Tooltip("Enable this if text-to-speech voice over says unintended things. It will then read from the text box below instead.")]
    [SerializeField] private bool UseManualText = false;
    [SerializeField] private string ManualStringContent;


    // Start is called before the first frame update
    void Start()
    {
        _speaker = TTSSpeaker.GetComponentInChildren<TTSSpeaker>();
        _defaultSpeakerImage = TargetSpeakerImage.sprite;

        // loading the different speaker sprites from resources for animating later
        _speakerImageFull = Resources.Load<Sprite>("Sprites/Button (4)");
        _speakerImageMedium = Resources.Load<Sprite>("Sprites/Button (5)");
        _speakerImageSilent = Resources.Load<Sprite>("Sprites/Button (6)");
    }

    private string GenerateTextContent()
    {   
        // Return manually entered text if enabled
        if (UseManualText)
            return ManualStringContent;

        // The final string that will be sent to the text-to-speech service
        string _ttsString = string.Empty;

        // Adding automatically discovered text to _ttsString, and adding spaces and dots if necessary 
        foreach (Text text in TargetObject.GetComponentsInChildren<Text>())
        {
            _ttsString += text.text;

            if (_ttsString.Last() != '.')
                _ttsString += ". ";
            else
                _ttsString += " ";
        }

        // Removing content that should be excluded
        if (ExcludeStrings.Count > 0)
            foreach (string text in ExcludeStrings)
                _ttsString.Replace(text, string.Empty);

        return _ttsString;
    }
    public void PlayTTS()
    {
        _speaker.Speak(GenerateTextContent());
        InvokeRepeating("AnimateSpeaker", 0, 0.5f);
    }

    private int step = 0;
    private void AnimateSpeaker()
    {
        if (step == 0)
            TargetSpeakerImage.sprite = _speakerImageSilent;
        else if (step == 1)
            TargetSpeakerImage.sprite = _speakerImageMedium;
        else if (step == 2)
            TargetSpeakerImage.sprite = _speakerImageFull;

        step = (step + 1) % 3;
    }

    public void SetDefaultSpeakerIcon()
    {
        if (IsInvoking("AnimateSpeaker"))
            CancelInvoke("AnimateSpeaker");

        TargetSpeakerImage.sprite = _defaultSpeakerImage;
    }
}
