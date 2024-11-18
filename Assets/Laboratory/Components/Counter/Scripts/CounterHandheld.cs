using System;
using System.Collections.Generic;
using UnityEngine;

public class CounterHandheld : MonoBehaviour
{
    [SerializeField] private List<GameObject> NumberedWheels = new List<GameObject>();
    private int Count = 0; 
    private DialogueBoxController dialogueBoxController;

    // Start is called before the first frame update
    void Start()
    {
        dialogueBoxController = FindObjectOfType<DialogueBoxController>();
        Count = UnityEngine.Random.Range(0, 9999);  // start with random value so the player is required to reset the counter
        SetNumberedWheels();
    }

    public void Increment()
    {
        if (dialogueBoxController != null && dialogueBoxController.dialogueTreeRestart.name == "LarsDialogue")
        {
            if (dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[13].dialogue[4])
            {
                dialogueBoxController.SkipLine();
            }
        }
        Count = (Count + 1) % 10000;
        SetNumberedWheels();
    }

    public void ResetCounter()
    {
        if (dialogueBoxController != null && dialogueBoxController.dialogueTreeRestart.name == "LarsDialogue")
        {
            if (dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[13].dialogue[5])
            {
                dialogueBoxController.SkipLine();
            }
        }
        Count = 0;
        SetNumberedWheels();
    }

    /// <summary>
    /// Here I'm using the Normalize() method to translate the digits into degrees. 
    /// That way, the digit value, which is 0 - 9, is translated into degrees, 0 - 324.
    /// The reason for using 324 degrees is because each number is placed at ((360 / 10) * digit) degrees, where 324 represents the digit 9. 
    /// </summary>
    private void SetNumberedWheels()
    {
        string CountString = Count.ToString();
        if (Count < 10)
        {
            NumberedWheels[0].transform.localEulerAngles = new Vector3(Normalize((float)Char.GetNumericValue(CountString[0]), 0, 9, 0, 360 - 36), 0, 0);
            NumberedWheels[1].transform.localEulerAngles = Vector3.zero;
            NumberedWheels[2].transform.localEulerAngles = Vector3.zero;
            NumberedWheels[3].transform.localEulerAngles = Vector3.zero;
        }
        else if (Count < 100)
        {
            NumberedWheels[0].transform.localEulerAngles = new Vector3(Normalize((float)Char.GetNumericValue(CountString[^1]), 0, 9, 0, 360 - 36), 0, 0);
            NumberedWheels[1].transform.localEulerAngles = new Vector3(Normalize((float)Char.GetNumericValue(CountString[^2]), 0, 9, 0, 360 - 36), 0, 0);
            NumberedWheels[2].transform.localEulerAngles = Vector3.zero;
            NumberedWheels[3].transform.localEulerAngles = Vector3.zero;
        }
        else if (Count < 1000)
        {
            NumberedWheels[0].transform.localEulerAngles = new Vector3(Normalize((float)Char.GetNumericValue(CountString[^1]), 0, 9, 0, 360 - 36), 0, 0);
            NumberedWheels[1].transform.localEulerAngles = new Vector3(Normalize((float)Char.GetNumericValue(CountString[^2]), 0, 9, 0, 360 - 36), 0, 0);
            NumberedWheels[2].transform.localEulerAngles = new Vector3(Normalize((float)Char.GetNumericValue(CountString[^3]), 0, 9, 0, 360 - 36), 0, 0);
            NumberedWheels[3].transform.localEulerAngles = Vector3.zero;
        }
        else if (Count < 10000)
        {
            NumberedWheels[0].transform.localEulerAngles = new Vector3(Normalize((float)Char.GetNumericValue(CountString[^1]), 0, 9, 0, 360 - 36), 0, 0);
            NumberedWheels[1].transform.localEulerAngles = new Vector3(Normalize((float)Char.GetNumericValue(CountString[^2]), 0, 9, 0, 360 - 36), 0, 0);
            NumberedWheels[2].transform.localEulerAngles = new Vector3(Normalize((float)Char.GetNumericValue(CountString[^3]), 0, 9, 0, 360 - 36), 0, 0);
            NumberedWheels[3].transform.localEulerAngles = new Vector3(Normalize((float)Char.GetNumericValue(CountString[^4]), 0, 9, 0, 360 - 36), 0, 0);
        }
    }

    // found this function at https://stackoverflow.com/questions/51161098/normalize-range-100-to-100-to-0-to-3
    private float Normalize(float val, float valmin, float valmax, float min, float max)
    {
        return (((val - valmin) / (valmax - valmin)) * (max - min)) + min;
    }
}
