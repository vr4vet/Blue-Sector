using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvokeEventOnNPCAnswerButton : MonoBehaviour
{
    [SerializeField] private List<string> ValidButtonAnswers = new();

    public UnityEvent m_OnValidAnswerClicked;
    public UnityEvent m_OnAdvancedClicked;
    void Start()
    {
        if (m_OnValidAnswerClicked == null)
            m_OnValidAnswerClicked = new UnityEvent();

        if (m_OnAdvancedClicked == null)
            m_OnAdvancedClicked = new UnityEvent();

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
        }
    }

    private void OnDestroy()
    {
        ButtonSpawner.OnAnswer -= ButtonSpawner_OnAnswer;
    }
}
