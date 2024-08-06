using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ResultLogger : MonoBehaviour
{
    [SerializeField]
    public RegisterFish plate;
    [SerializeField]
    public TMP_Text weight;
    [SerializeField]
    public TMP_Text length;
    [SerializeField]
    public TMP_Text conditionFactor;
    [SerializeField]
    public AudioSource audio;
    public AudioSource audio2;
    public UnityEvent OnClick;

    private bool UsingDecimals;
    private decimal currentValue = 0;
    private int InputDecimals = 0;
    private TMP_Text currentText;

    private float weightAnswer;
    private float lengthAnswer;
    private float conditionAnswer;

    void Start()
    {
        currentText = weight;
    }

    public void OnPressedNumber(int number)
    {
        if (!UsingDecimals)
        {
            currentValue *= 10;
            currentValue += number;
        }
        else
        {
            currentValue += (decimal)number / (decimal)Mathf.Pow(10, ++InputDecimals);
        }
        currentText.SetText(currentValue.ToString());
    }

    public void OnPressedClearEntry()
    {
        currentValue = 0;
        UsingDecimals = false;
        currentText.SetText(currentValue.ToString());
    }

    public void OnPressedDecimal()
    {
        UsingDecimals = true;
        InputDecimals = 0;
    }

    public void OnPressedOk()
    {
        weightAnswer = float.Parse(weight.text);
        lengthAnswer = float.Parse(length.text);
        conditionAnswer = float.Parse(conditionFactor.text);

        if (weightAnswer == plate.fishWeight 
        && lengthAnswer == plate.fishLength 
        && System.Math.Round(conditionAnswer, 5) == System.Math.Round(plate.conditionRight, 5))
        {
            audio.Play();
        }
        else
        {
            audio2.Play();
        }
    }

    public void SwitchToWeight()
    {
        currentText = weight;
        UsingDecimals = false;
        currentValue = 0;
        currentText.SetText(currentValue.ToString());
    }

    public void SwitchToLength()
    {
        currentText = length;
        UsingDecimals = false;
        currentValue = 0;
        currentText.SetText(currentValue.ToString());
    }

    public void SwitchToCondition()
    {
        currentText = conditionFactor;
        UsingDecimals = false;
        currentValue = 0;
        currentText.SetText(currentValue.ToString());
    }
}
