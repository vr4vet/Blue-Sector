using TMPro;
using UnityEngine;

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
        if (currentInputField.text.Length < 8)
            currentInputField.text += digit.ToString();
    }

    public void ClearInputField()
    {
        currentInputField.text = string.Empty;
    }
    
}
