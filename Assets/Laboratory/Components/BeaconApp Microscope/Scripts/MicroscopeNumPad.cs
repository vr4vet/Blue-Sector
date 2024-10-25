using Meta.WitAi;
using TMPro;
using UnityEngine;

public class MicroscopeNumPad : MonoBehaviour
{
    private TMP_InputField currentInputField;
    public TMP_InputField CurrentInputField 
    {
        get { return currentInputField; }
        set 
        {
            // stop indicator cycle to prevent doubling cycles
            StopIndicatorCycle();

            // ensure input field does not contain indicator symbols before moving on to the new input field selected by player
            if (currentInputField != null)
                currentInputField.text = currentInputField.text.Replace(" ", "").Replace("|", "");

            currentInputField = value;
            StartIndicatorCycle();
        }
    }

    private bool IndicatedLast = false;
    private int IndicatorPos;

    public void EnterDigit(int digit)
    {
        if (currentInputField.text.Length < 8)
        {
            // stop cycle before entering digit to prevent indicator symbols from being placed between digits
            StopIndicatorCycle();

            RemoveIndicator();
            currentInputField.text = currentInputField.text.Replace(" ", "") + digit.ToString();

            StartIndicatorCycle();
        }     
    }

    public void ClearInputField()
    {
        currentInputField.text = string.Empty;
    }

    public void RemoveCurrentInputField()
    {
        // stop cycle and remove all indicator symbols to stop content from changing in inactive input fields
        StopIndicatorCycle();
        RemoveIndicator();
        currentInputField = null; 
    }

    /// <summary>
    /// make the last character oscilate between " " and '|'.
    /// " " prevents placeholder text from appearing when string is empty.
    /// </summary>
    public void IndicateCurrentInputField()
    {
        if (currentInputField == null)
            return;

        if (!IndicatedLast)
            currentInputField.text = currentInputField.text.Replace(" ", "") + "|";
        else
            RemoveIndicator();

        IndicatedLast = !IndicatedLast;
    }

    private void RemoveIndicator()
    {
        currentInputField.text = currentInputField.text.Replace("|", " ");
    }

    private void StopIndicatorCycle()
    {
        if (IsInvoking("IndicateCurrentInputField"))
            CancelInvoke("IndicateCurrentInputField");
    }

    private void StartIndicatorCycle()
    {
        InvokeRepeating("IndicateCurrentInputField", 0, 0.5f);
    }
}
