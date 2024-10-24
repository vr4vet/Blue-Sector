using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MicroscopeNumPad : MonoBehaviour
{
    private TMP_InputField currentInputField;
    public TMP_InputField CurrentInputField 
    {
        get { return currentInputField; }
        set { currentInputField = value; }
    } 

    public void EnterDigit(int digit)
    {
        currentInputField.text += digit.ToString();
    }

    public void ClearInputField()
    {
        currentInputField.text = string.Empty;
    }
    
}
