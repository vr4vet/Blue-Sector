using Meta.WitAi.TTS.Utilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System.Collections;

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

    [Tooltip("How much the manually inputted text will override the automatically discovered text.\n" +
             "\nNone: Only automatically discovered text will be used. \n" +
             "\nAll: Content in 'Manual String Content' only.\n" +
             "\nBefore: Content in 'Manual String Content' will be placed before.\n" +
             "\nAfter: Content in 'Manual String Content' will be placed after.")]
    [SerializeField] private manualTextType ManualTextType = manualTextType.None;
    private enum manualTextType
    {
        None, All, Before, After
    }

    [Tooltip("Type what the text-to-speech will say here if using manual text")]
    [SerializeField] private string ManualStringContent;


    // Start is called before the first frame update
    void Start()
    {
        // fetching the text-to-speech object, and making SetDefaultSpeakerIcon() listen to the event that fires when voice over is finished
        _speaker = TTSSpeaker.GetComponentInChildren<TTSSpeaker>();
        _defaultSpeakerImage = TargetSpeakerImage.sprite;

        // loading the different speaker sprites from resources for animating later
        _speakerImageFull = Resources.Load<Sprite>("Sprites/Button (4)");
        _speakerImageMedium = Resources.Load<Sprite>("Sprites/Button (5)");
        _speakerImageSilent = Resources.Load<Sprite>("Sprites/Button (6)");
    }

    private string GenerateTextContent()
    {   
        // Return manually entered text if 'All' is selected
        if (ManualTextType == manualTextType.All)
            return ManualStringContent;

        // The final string that will be sent to the text-to-speech service
        string _ttsString = string.Empty;

        // Place manually entered text first if 'Before' is selected
        if (ManualTextType == manualTextType.Before)
            _ttsString += ManualStringContent;

        // Adding automatically discovered text to _ttsString, and adding spaces and dots if necessary
        foreach (GameObject targetObject in TargetObjects)
        {
            if (TextType == textType.Text)
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
            else if (TextType == textType.TextMeshPro)
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
                _ttsString = _ttsString.Replace(text, string.Empty);

        // Place manually entered text last if 'After' is selected
        if (ManualTextType == manualTextType.After)
            _ttsString += ManualStringContent;

        Debug.Log(_ttsString);
        return _ttsString;
    }

    /// <summary>
    /// This should be placed in the button's On Click event.
    /// </summary>
    private float buttonPressTime = Mathf.NegativeInfinity;
    public void PlayTTS()
    {
        // Prevent unwanted double clicks
        if (Time.realtimeSinceStartup - buttonPressTime < .4)
            return;

        buttonPressTime = Time.realtimeSinceStartup;

        if (_speaker.IsSpeaking)
            _speaker.StopSpeaking();
        else
        {
            _speaker.Speak(GenerateTextContent());
            StartCoroutine(AnimateSpeaker(0));
        }
    }

    /// <summary>
    /// This method swaps the speaker button icon every half second to animate sound waves while the text-to-speech is talking.
    /// Unfortunately has to be called repeatedly as a coroutine (in this case recursivley) in order to function when Time.timeScale is 0.
    /// An example where Time.timeScale is set to 0 is when the main menu pauses the game.
    /// </summary>
    private int step = 0;
    private IEnumerator AnimateSpeaker(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);

        if (_speaker.IsActive)
        {
            StartCoroutine(AnimateSpeaker(.5f));

            if (step == 0)
                TargetSpeakerImage.sprite = _speakerImageSilent;
            else if (step == 1)
                TargetSpeakerImage.sprite = _speakerImageMedium;
            else if (step == 2)
                TargetSpeakerImage.sprite = _speakerImageFull;

            step = (step + 1) % 3;
        }
        else
        {
            step = 0;
            TargetSpeakerImage.sprite = _defaultSpeakerImage;
        }
    }
}
