using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ResultLogger : MonoBehaviour
{
    // ----------------- Editor Variables -----------------
    public RegisterFish plate;
    public NpcTriggerDialogue npcTriggerDialogue;
    public Scale scale;
    public TMP_Text weight;
    public TMP_Text length;
    public TMP_Text conditionFactor;
    public TMP_Text correctlyLoggedFish;
    public TMP_Text activeFishText;
    public AudioSource Audio;
    public AudioSource audio2;
    public UnityEvent m_OnWeightRegistered;
    public UnityEvent m_OnLengthRegistered;
    public UnityEvent m_OnConditionFactorRegistered;
    public UnityEvent m_OnConditionFactorCorrectFirstTry;

    // ----------------- Private Variables -----------------
    private bool UsingDecimals;
    private decimal currentValue = 0;
    private int InputDecimals = 0;
    private TMP_Text currentText;
    private float weightAnswer;
    private float lengthAnswer;
    private float conditionAnswer;
    private bool wasLoggedCorectly = false;
    private int tries = 0;

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

    private DialogueBoxController dialogueBoxController;

    private Attributes _currentAttribute; // used to keep track of which attribute the player is currently registering

    private enum Attributes
    {
        Weight, Length, ConditionFactor
    }


    void Start()
    {
        m_OnLengthRegistered ??= new UnityEvent();
        m_OnLengthRegistered ??= new UnityEvent();
        m_OnConditionFactorRegistered ??= new UnityEvent();
        m_OnConditionFactorCorrectFirstTry ??= new UnityEvent();

        currentText = weight;
        dialogueBoxController = FindObjectOfType<DialogueBoxController>();

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
        if (_currentAttribute != Attributes.ConditionFactor)
        {
            // skip dialogue ahead when player registers weight or length
            if (_currentAttribute == Attributes.Weight)
            {
                if (dialogueBoxController.dialogueTreeRestart.name == "LarsDialogue" && dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[1].dialogue[4])
                {
                    dialogueBoxController.SkipLine();
                    m_OnWeightRegistered.Invoke();
                }
            }
            else if (_currentAttribute == Attributes.Length)
            {
                if (dialogueBoxController.dialogueTreeRestart.name == "LarsDialogue" && dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[3].dialogue[1])
                {
                    dialogueBoxController.SkipLine();
                    m_OnLengthRegistered.Invoke();
                }
            }

            return;
        }

        tries++;

        weightAnswer = float.Parse(weight.text);
        lengthAnswer = float.Parse(length.text);
        conditionAnswer = float.Parse(conditionFactor.text);

        if (weightAnswer == plate.fishWeight 
        && lengthAnswer == plate.fishLength 
        && (float)System.Math.Round(conditionAnswer, 2) == plate.conditionRight
        && scale.tubWasUsed == true)
        {
            wasLoggedCorectly = true;
            npcTriggerDialogue.Response1();
            Audio.Play();
            m_OnConditionFactorRegistered.Invoke();
            if (tries < 2)
                m_OnConditionFactorCorrectFirstTry.Invoke();
        }
        else if (weightAnswer == plate.fishWeight 
        && lengthAnswer == plate.fishLength 
        && (float)System.Math.Round(conditionAnswer, 2) == plate.conditionRight)
        {
            npcTriggerDialogue.Response2();
            audio2.Play();
        }
        else if (weightAnswer == plate.fishWeight 
        && lengthAnswer == plate.fishLength 
        && (float)System.Math.Round(conditionAnswer, 2) != plate.conditionRight)
        {
            npcTriggerDialogue.Response3();
            audio2.Play();
        }
        else if (weightAnswer == plate.fishWeight 
        && lengthAnswer != plate.fishLength )
        {
            npcTriggerDialogue.Response4();
            audio2.Play();
        }
        else if (weightAnswer != plate.fishWeight 
        && lengthAnswer == plate.fishLength )
        {
            npcTriggerDialogue.Response5();
            audio2.Play();
        }
        else if (weightAnswer != plate.fishWeight 
        && lengthAnswer != plate.fishLength 
        && System.Math.Round(conditionAnswer, 2) != plate.conditionRight)
        {
            npcTriggerDialogue.Response6();
            audio2.Play();
        }
        logAnswer(activeFish.fishObject, weightAnswer, lengthAnswer, conditionAnswer, wasLoggedCorectly);
        wasLoggedCorectly = false;
    }

    private void logAnswer(GameObject fish, float weight, float length, float condition, bool wasFishLoggedCorectly)
    {
        if (fish == null)
        {
            audio2.Play();
            return;
        }
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
        _currentAttribute = Attributes.Weight;

        currentText = weight;
        UsingDecimals = false;
        currentValue = 0;
        currentText.SetText(currentValue.ToString());
    }

    public void SwitchToLength()
    {
        _currentAttribute= Attributes.Length;

        currentText = length;
        UsingDecimals = false;
        currentValue = 0;
        currentText.SetText(currentValue.ToString());
    }

    public void SwitchToCondition()
    {
        _currentAttribute = Attributes.ConditionFactor;

        currentText = conditionFactor;
        UsingDecimals = false;
        currentValue = 0;
        currentText.SetText(currentValue.ToString());
    }


    public void SetActiveFish(int fishNumber)
    {
        if (loggedAnswers.Count >= fishNumber)
        {
            activeFish = loggedAnswers[fishNumber-1];
            activeFishText.SetText("Fish "+ fishNumber.ToString());
            plate.activeFishChanged(activeFish.fishObject);
        }
    }
}
