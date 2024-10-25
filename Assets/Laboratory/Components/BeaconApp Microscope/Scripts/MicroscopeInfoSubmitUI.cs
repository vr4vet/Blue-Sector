using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MicroscopeInfoSubmitUI : MonoBehaviour
{
    private MicroscopeSlideWithGrid Slide;
    private List<TMP_InputField> InputFields = new List<TMP_InputField>();
    private List<Image> InputFieldHighlights = new List<Image>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (TMP_InputField InputField in GetComponentsInChildren<TMP_InputField>())
        {
            InputFields.Add(InputField);

            // making highlights showing if input field answer is correct/incorrect transparent (invisible) before adding to list
            Image HighLightImage = InputField.transform.parent.Find("InputFieldHighlight").GetComponent<Image>();
            HighLightImage.color = Color.clear;
            InputFieldHighlights.Add(HighLightImage);
        }

        if (InputFields.Count != 3 || InputFieldHighlights.Count != 3)
            Debug.LogError("The amount of input fields are not 3!");
    }

    public void SetCurrentSlideWithGrid(MicroscopeSlideWithGrid slide)
        { Slide = slide; }

    public void Submit()
    {
        if (Slide == null)
            return;

        for (int i = 0; i < InputFields.Count; i++)
            VerifyAnswer(i);
    }

    private void VerifyAnswer(int specimen)
    {
        // fetching the actual amount of individuals of the specimen
        int correctAnswer;
        if (specimen == 0)
            correctAnswer = Slide.GetTotalAmountOfSkeletonema();
        else if (specimen == 1)
            correctAnswer = Slide.GetTotalAmountOfChaetoceros();
        else if (specimen == 2)
            correctAnswer = Slide.GetTotalAmountOfPseudoNitzschia();
        else
        {
            Debug.LogError(specimen + " is not a valid index");
            return;
        }

        Debug.Log("Specimen " + specimen + ": " + correctAnswer);

        // checking if the player has provided an answer in the input field
        string inputString = InputFields[specimen].text;
        if (inputString == string.Empty)
        {
            MarkIncorrect(specimen);
            return;
        }

        // comparing the provided answer with the actual amount. an inaccuracy of 5 is accepted
        int submittedAnswer;
        if (int.TryParse(inputString, out submittedAnswer))
        {
            if (Mathf.Abs(correctAnswer - submittedAnswer) <= 5)
                MarkCorrect(specimen);
            else
                MarkIncorrect(specimen);
        }
        else
            Debug.LogError($"Could not parse '{inputString}' to an int");
    }

    private void MarkCorrect(int specimen)
    {
        // green colour
        InputFieldHighlights[specimen].color = new Color32(0x09, 0xB9, 0x00, 255);
    }

    private void MarkIncorrect(int specimen)
    {
        // red colour
        InputFieldHighlights[specimen].color = new Color32(0xFF, 0x00, 0x00, 255);
    }
}
