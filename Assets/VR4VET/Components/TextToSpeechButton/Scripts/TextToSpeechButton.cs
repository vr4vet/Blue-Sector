using Meta.WitAi.TTS.Utilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class TextToSpeechButton : MonoBehaviour
{
    [Tooltip("Objects which contain the desired text content. This script will attempt to fetch the text content automatically")]
    [SerializeField] private List<GameObject> TargetObjects = new List<GameObject>();

    [Tooltip("Point to the TTS instance for this object here")]
    [SerializeField] private GameObject TTSSpeaker;
    private TTSSpeaker _speaker;

    [Tooltip("The speaker image that is placed somewhere on this object.")]
    [SerializeField] private Image TargetSpeakerImage;
    private Sprite _defaultSpeakerImage;
    private Sprite _speakerImageFull;
    private Sprite _speakerImageMedium;
    private Sprite _speakerImageSilent;

    [Tooltip("The class of the objects' text content. Some canvases use  UnityEngine.UI.Text, while others use TMPro.TMP_Text (TextMeshPro)")]
    [SerializeField] private textType TextType = textType.Text;
    private enum textType
    {
        Text, TextMeshPro
    }

    [Tooltip("Strings added to this list will be removed from the text that is acquired automatically.")]
    [SerializeField] private List<string> ExcludeStrings = new List<string>();

    [Tooltip("Enable this if text-to-speech voice over says unintended things. It will then read from the text box below instead.")]
    [SerializeField] private bool UseManualText = false;
    [SerializeField] private string ManualStringContent;


    // Start is called before the first frame update
    void Start()
    {
        // fetching the text-to-speech object, and making SetDefaultSpeakerIcon() listen to the event that fires when voice over is finished
        _speaker = TTSSpeaker.GetComponentInChildren<TTSSpeaker>();
        _speaker.Events.OnAudioClipPlaybackFinished.AddListener(delegate { SetDefaultSpeakerIcon(); });
        _speaker.Events.OnAudioClipPlaybackCancelled.AddListener(delegate { SetDefaultSpeakerIcon(); });
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
        if (TextType == textType.Text)
        {
            foreach (GameObject targetObject in TargetObjects)
            {
                foreach (Text text in targetObject.GetComponentsInChildren<Text>())
                {
                    _ttsString += text.text;

                    if (_ttsString.Last() != '.')
                        _ttsString += ". ";
                    else
                        _ttsString += " ";
                }
            }
        }
        else if (TextType == textType.TextMeshPro)
        {
            foreach (GameObject targetObject in TargetObjects)
            {
                foreach (TMP_Text text in targetObject.GetComponentsInChildren<TMP_Text>())
                {
                    _ttsString += text.text;

                    if (_ttsString.Last() != '.')
                        _ttsString += ". ";
                    else
                        _ttsString += " ";
                }
            }
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

    private void SetDefaultSpeakerIcon()
    {
        if (IsInvoking("AnimateSpeaker"))
            CancelInvoke("AnimateSpeaker");

        TargetSpeakerImage.sprite = _defaultSpeakerImage;
    }
}