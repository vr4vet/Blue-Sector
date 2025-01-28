using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvokeEventOnNPCAnswerButton : MonoBehaviour
{
    [SerializeField] private List<string> ValidButtonAnswers = new();

    public UnityEvent m_OnValidAnswerClicked;
    public UnityEvent m_OnAdvancedClicked;
    public UnityEvent m_OnConditionFactorClicked;
    public UnityEvent m_OnPlanktonSampleClicked;
    public UnityEvent m_OnDissectingFishClicked;
    void Start()
    {
        if (m_OnValidAnswerClicked == null)
            m_OnValidAnswerClicked = new UnityEvent();

        if (m_OnAdvancedClicked == null)
            m_OnAdvancedClicked = new UnityEvent();

        if (m_OnConditionFactorClicked == null)
            m_OnConditionFactorClicked = new UnityEvent();

        if (m_OnPlanktonSampleClicked == null)
            m_OnPlanktonSampleClicked = new UnityEvent();

        if (m_OnDissectingFishClicked == null)
            m_OnDissectingFishClicked = new UnityEvent();

        ButtonSpawner.OnAnswer += ButtonSpawner_OnAnswer;
    }

    private void ButtonSpawner_OnAnswer(string answer)
    {
        bool validAnswerFound = false;
        foreach (string validAnswer in ValidButtonAnswers)
        {
            if (answer.Equals(validAnswer))
                validAnswerFound = true;
        }

        if (validAnswerFound)
        {
            m_OnValidAnswerClicked.Invoke();
            
            if (answer.Equals("Advanced"))
                m_OnAdvancedClicked.Invoke();
            if (answer.Equals("Calculating condition factor"))
                m_OnConditionFactorClicked.Invoke();
            if (answer.Equals("Analysing plankton samples"))
                m_OnPlanktonSampleClicked.Invoke();
            if (answer.Equals("Dissecting the fish (Still in development)"))
                m_OnDissectingFishClicked.Invoke();
        }
    }

    private void OnDestroy()
    {
        ButtonSpawner.OnAnswer -= ButtonSpawner_OnAnswer;
    }
}
