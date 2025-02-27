using Meta.WitAi.TTS.Utilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System.Collections;
using System.Text.RegularExpressions;

public class TextToSpeechButton : MonoBehaviour
{
    [Tooltip("Objects which contain the desired text content. This script will attempt to fetch the text content automatically")]
    [SerializeField] private List<GameObject> TargetObjects = new();

    [Tooltip("Point to the TTS instance for this object here")]
    [SerializeField] private GameObject TTSSpeaker;
    private TTSSpeaker _speaker;

    [Tooltip("Place children speaker icons here in increasing order (speaker without sound waves at index 0)")]
    [SerializeField] private List<Image> SpeakerIcons;

    [Tooltip("The class of the objects' text content. Some canvases use  UnityEngine.UI.Text, while others use TMPro.TMP_Text (TextMeshPro)")]
    [SerializeField] private textType TextType = textType.Text;
    private enum textType
    {
        Text, TextMeshPro
    }

    [Tooltip("Strings added to this list will be removed from the text that is acquired automatically.")]
    [SerializeField] private List<string> ExcludeStrings = new();

    [Tooltip("How much the manually inputted text will override the automatically discovered text.\n" +
             "\nNone: Only automatically discovered text will be used. \n" +
             "\nAll: Content in 'Manual String Content' only.\n" +
             "\nBefore: Content in 'Manual String Content' will be placed before.\n" +
             "\nAfter: Content in 'Manual String Content' will be placed after.")]
    [SerializeField] private ManualTextType manualTextType = ManualTextType.None;
    private enum ManualTextType
    {
        None, All, Before, After
    }

    [Tooltip("Type what the text-to-speech will say here if using manual text")]
    [SerializeField] private string ManualStringContent;


    // Start is called before the first frame update
    void Start()
    {
        // fetching the text-to-speech object
        _speaker = TTSSpeaker.GetComponentInChildren<TTSSpeaker>();
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

        // stop tts if currently running, otherwise manipulate text content before sending to tts service
        if (_speaker.IsActive)
            _speaker.Stop();
        else
        {
            string fetchedTextContent = FetchTextContent();
            string[] splitTextContent = GenerateSplitTextContent(fetchedTextContent);
            StartCoroutine(_speaker.SpeakQueuedAsync(splitTextContent));
            StartCoroutine(nameof(AnimateSpeaker));
        }
    }

    /// <summary>
    /// Searches through children of the objects in 'TargetObjects' and discovers text content of the selected class set in 'manualTextType'. 
    /// Overrides using manual text if 'manualTextType' is not set to 'None'.
    /// </summary>
    /// <returns></returns>
    private string FetchTextContent()
    {   
        // Return manually entered text if 'All' is selected
        if (manualTextType == ManualTextType.All)
            return ManualStringContent;

        // The final string that will be returned
        string _ttsString = string.Empty;

        // Place manually entered text first if 'Before' is selected
        if (manualTextType == ManualTextType.Before)
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
                        _ttsString += ".";
                }
            }
            else if (TextType == textType.TextMeshPro)
            {
                foreach (TMP_Text text in targetObject.GetComponentsInChildren<TMP_Text>())
                {
                    _ttsString += text.text;

                    if (_ttsString.Last() != '.')
                        _ttsString += ".";
                }
            }
        }

        // Removing content that should be excluded
        if (ExcludeStrings.Count > 0)
            foreach (string text in ExcludeStrings)
                _ttsString = _ttsString.Replace(text, string.Empty);

        // Place manually entered text last if 'After' is selected
        if (manualTextType == ManualTextType.After)
            _ttsString += ManualStringContent;

        return _ttsString;
    }


    /// <summary>
    /// Split the text content into sections that are <= 280 chars long to respect the TTS service's char limit
    /// </summary>
    /// <param name="input"></param>
    private string[] GenerateSplitTextContent(string input)
    {
        List<string> strings = new();
        // split string at each '.' char while respecting '!' and '?'. special characters are removed to prevent text-to-speech from saying weird things
        foreach(string s in input.Split('.'))
        {
            string cleanedString = Regex.Replace(s.Trim(), "<\\/?[bi]>", string.Empty);
            if (!string.IsNullOrEmpty(cleanedString))
            {
                char lastChar = cleanedString.Last();
                strings.Add(cleanedString + (char.IsLetterOrDigit(lastChar) ? "." : string.Empty) + " "); // place a dot at end of sentence if last character is letter or digit
            }
        }

        List<string> stringsFinal = new();
        string tmp = string.Empty;
        // make each element of 'stringsFinal' as long as possible while <= 280 chars
        for (int i = 0; i < strings.Count; i++)
        {
            if ((tmp + strings[i]).Length > 280)
            {
                stringsFinal.Add(tmp);
                tmp = string.Empty + strings[i];
            }
            else
                tmp += strings[i];
                
            // ensure the the last string is added
            if (i == strings.Count - 1)
                stringsFinal.Add(tmp);
        }
        return stringsFinal.ToArray();
    }

    /// <summary>
    /// This method swaps the speaker button icon every half second to animate sound waves while the text-to-speech is talking.
    /// </summary>
    private int step = 0;
    private IEnumerator AnimateSpeaker()
    {
        while (true)
        {
            if (_speaker.IsActive)
            {
                SetCurrentSpeakerIcon(step);
                step = (step + 1) % 3;
            }
            else
            {
                step = 0;
                SetCurrentSpeakerIcon(SpeakerIcons.Count - 1); // reset to standard speaker icon with two sound waves ("full" icon)
                StopCoroutine(nameof(AnimateSpeaker));
            }

            yield return new WaitForSecondsRealtime(.5f);
        }
    }

    private void SetCurrentSpeakerIcon(int step)
    {
        for (int i = 0; i < SpeakerIcons.Count; i++)
        {
            if (i == step)
                SpeakerIcons[i].enabled = true;
            else
                SpeakerIcons[i].enabled = false;
        }
    }
}
