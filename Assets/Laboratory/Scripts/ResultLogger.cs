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
    public NpcTriggerDialogue npcTriggerDialogue;
    [SerializeField]
    public Scale scale;
    [SerializeField]
    public TMP_Text weight;
    [SerializeField]
    public TMP_Text length;
    [SerializeField]
    public TMP_Text conditionFactor;
    [SerializeField]
    public TMP_Text correctlyLoggedFish;
    [SerializeField]
    public TMP_Text activeFishText;
    [SerializeField]
    public AudioSource audio;
    [SerializeField]
    public AudioSource audio2;

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

    private List<LoggedAnswers> loggedAnswers = new List<LoggedAnswers>();

    private LoggedAnswers activeFish;

    void Start()
    {
        currentText = weight;

        GameObject[] fishInScene = GameObject.FindGameObjectsWithTag("Fish");
        foreach (GameObject fish in fishInScene)
        {
            logAnswer(fish, 0, 0, 0, false);
        }
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
        && (float)System.Math.Round(conditionAnswer, 2) == plate.conditionRight
        && scale.tubWasUsed == true)
        {
            wasLoggedCorectly = true;
            npcTriggerDialogue.response1();
            audio.Play();
        }
        else if (weightAnswer == plate.fishWeight 
        && lengthAnswer == plate.fishLength 
        && (float)System.Math.Round(conditionAnswer, 2) == plate.conditionRight)
        {
            npcTriggerDialogue.response2();
            audio2.Play();
        }
        else if (weightAnswer == plate.fishWeight 
        && lengthAnswer == plate.fishLength 
        && (float)System.Math.Round(conditionAnswer, 2) != plate.conditionRight)
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
        && System.Math.Round(conditionAnswer, 2) != plate.conditionRight)
        {
            npcTriggerDialogue.response6();
            audio2.Play();
        }
        logAnswer(activeFish.fishObject, weightAnswer, lengthAnswer, conditionAnswer, wasLoggedCorectly);
        wasLoggedCorectly = false;
    }

    private void logAnswer(GameObject fish, float weight, float length, float condition, bool wasFishLoggedCorectly)
    {
        for (int i = 0; i < loggedAnswers.Count; i++)
        {
            if (loggedAnswers[i].fishObject == fish)
            {
                LoggedAnswers updatedAnswer = new LoggedAnswers
                {
                    fishObject = fish,
                    fishWeight = weight,
                    fishLength = length,
                    fishConditionFactor = condition,
                    fishWasLoggedCorectly = wasFishLoggedCorectly
                };
                loggedAnswers[i] = updatedAnswer;
                setMeasuredFishText();
                return;
            }
        }
        LoggedAnswers newAnswer;
        newAnswer.fishObject = fish;
        newAnswer.fishWeight = weight;
        newAnswer.fishLength = length;
        newAnswer.fishConditionFactor = condition;
        newAnswer.fishWasLoggedCorectly = wasFishLoggedCorectly;
        loggedAnswers.Add(newAnswer);
        setMeasuredFishText();
    }

    private void setMeasuredFishText()
    {
        int fishCorrect = 0;
        int fishLeft = loggedAnswers.Count;
        for (int i = 0; i < loggedAnswers.Count; i++)
        {
            if (loggedAnswers[i].fishWasLoggedCorectly == true)
            {
                fishCorrect++;
            }
        }
        correctlyLoggedFish.SetText(fishCorrect.ToString()+"/"+fishLeft.ToString());
    }

    public int getFishNumber(GameObject fish)
    {
        for (int i = 0; i < loggedAnswers.Count; i++)
        {
            if (loggedAnswers[i].fishObject == fish)
            {
                return i+1;
            }
        }
        return 0;
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

    public void setActiveFish(int fishNumber)
    {
        if (loggedAnswers.Count >= fishNumber)
        {
            activeFish = loggedAnswers[fishNumber-1];
            activeFishText.SetText("Fish "+ fishNumber.ToString());
            plate.activeFishChanged(activeFish.fishObject);
        }
    }
}
