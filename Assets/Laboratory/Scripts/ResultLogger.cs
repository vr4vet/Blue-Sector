using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ResultLogger : MonoBehaviour
{
    // ----------------- Editor Variables -----------------
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
    [SerializeField]
    public AudioSource audio2;
    [SerializeField]
    public NpcTriggerDialogue npcTriggerDialogue;
    [SerializeField]
    public Scale scale;

    // ----------------- Private Variables -----------------
    private bool UsingDecimals;
    private decimal currentValue = 0;
    private int InputDecimals = 0;
    private TMP_Text currentText;
    private float weightAnswer;
    private float lengthAnswer;
    private float conditionAnswer;
    private bool wasLoggedCorectly = false;

    // Struct to store logged fish
    private struct LoggedAnswers {
    public GameObject fishObject;
    public float fishWeight;
    public float fishLength;
    public float fishConditionFactor; 
    public bool fishWasLoggedCorectly;
    }

    private LoggedAnswers[] loggedAnswers = new LoggedAnswers[6];

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
        && System.Math.Round(conditionAnswer, 5) == plate.conditionRight
        && scale.tubWasUsed == true)
        {
            wasLoggedCorectly = true;
            npcTriggerDialogue.response1();
            audio.Play();
        }
        else if (weightAnswer == plate.fishWeight 
        && lengthAnswer == plate.fishLength 
        && System.Math.Round(conditionAnswer, 5) == plate.conditionRight)
        {
            npcTriggerDialogue.response2();
            audio2.Play();
        }
        else if (weightAnswer == plate.fishWeight 
        && lengthAnswer == plate.fishLength 
        && System.Math.Round(conditionAnswer, 5) != plate.conditionRight)
        {
            npcTriggerDialogue.response3();
            audio2.Play();
        }
        else if (weightAnswer == plate.fishWeight 
        && lengthAnswer != plate.fishLength )
        {
            npcTriggerDialogue.response4();
            audio2.Play();
        }
        else if (weightAnswer != plate.fishWeight 
        && lengthAnswer == plate.fishLength )
        {
            npcTriggerDialogue.response5();
            audio2.Play();
        }
        else if (weightAnswer != plate.fishWeight 
        && lengthAnswer != plate.fishLength 
        && System.Math.Round(conditionAnswer, 5) != System.Math.Round(plate.conditionRight, 5))
        {
            npcTriggerDialogue.response6();
            audio2.Play();
        }

        logAnswer(plate.fishObject, weightAnswer, lengthAnswer, conditionAnswer, wasLoggedCorectly);
        wasLoggedCorectly = false;
    }

    private void logAnswer(GameObject fish, float weight, float length, float condition, bool wasFishLoggedCorectly)
    {
        for (int i = 0; i < loggedAnswers.Length; i++)
        {
            if (loggedAnswers[i].fishObject == fish)
            {
                loggedAnswers[i].fishWeight = weight;
                loggedAnswers[i].fishLength = length;
                loggedAnswers[i].fishConditionFactor = condition;
                loggedAnswers[i].fishWasLoggedCorectly = wasFishLoggedCorectly;
                return;
            }
        }
        LoggedAnswers newAnswer;
        newAnswer.fishObject = fish;
        newAnswer.fishWeight = weight;
        newAnswer.fishLength = length;
        newAnswer.fishConditionFactor = condition;
        newAnswer.fishWasLoggedCorectly = wasFishLoggedCorectly;
        loggedAnswers.Append(newAnswer);
        Debug.Log(loggedAnswers);
    }

    // ----------------- Unity event methods -----------------
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
