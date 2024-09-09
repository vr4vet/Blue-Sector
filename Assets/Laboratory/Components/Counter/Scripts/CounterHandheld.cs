using System;
using System.Collections.Generic;
using UnityEngine;

public class CounterHandheld : MonoBehaviour
{
    [SerializeField] private List<GameObject> NumberedWheels = new List<GameObject>();
    private int Count = 0;

    // Start is called before the first frame update
    void Start()
    {
        //NumberedWheels[3].transform.Rotate(36.5f, 0, 0);
        //InvokeRepeating("Increment", 0f, 0.1f);
    }

    public void Increment()
    {
        if (Count < 9999)
            Count++;
        else
        {
            Count = 0;
            NumberedWheels[0].transform.localEulerAngles = Vector3.zero;
            NumberedWheels[1].transform.localEulerAngles = Vector3.zero;
            NumberedWheels[2].transform.localEulerAngles = Vector3.zero;
            NumberedWheels[3].transform.localEulerAngles = Vector3.zero;
        }

        string CountString = Count.ToString();
        if (Count < 10)
        {
            NumberedWheels[0].transform.localEulerAngles = new Vector3(Normalize((float)Char.GetNumericValue(CountString[0]), 0, 9, 0, 360 - 36), 0, 0);
        }
        else if (Count < 100)
        {
            NumberedWheels[0].transform.localEulerAngles = new Vector3(Normalize((float)Char.GetNumericValue(CountString[^1]), 0, 9, 0, 360 - 36), 0, 0);
            NumberedWheels[1].transform.localEulerAngles = new Vector3(Normalize((float)Char.GetNumericValue(CountString[^2]), 0, 9, 0, 360 - 36), 0, 0);
        }
        else if (Count < 1000)
        {
            NumberedWheels[0].transform.localEulerAngles = new Vector3(Normalize((float)Char.GetNumericValue(CountString[^1]), 0, 9, 0, 360 - 36), 0, 0);
            NumberedWheels[1].transform.localEulerAngles = new Vector3(Normalize((float)Char.GetNumericValue(CountString[^2]), 0, 9, 0, 360 - 36), 0, 0);
            NumberedWheels[2].transform.localEulerAngles = new Vector3(Normalize((float)Char.GetNumericValue(CountString[^3]), 0, 9, 0, 360 - 36), 0, 0);
        }
        else if (Count < 10000)
        {
            NumberedWheels[0].transform.localEulerAngles = new Vector3(Normalize((float)Char.GetNumericValue(CountString[^1]), 0, 9, 0, 360 - 36), 0, 0);
            NumberedWheels[1].transform.localEulerAngles = new Vector3(Normalize((float)Char.GetNumericValue(CountString[^2]), 0, 9, 0, 360 - 36), 0, 0);
            NumberedWheels[2].transform.localEulerAngles = new Vector3(Normalize((float)Char.GetNumericValue(CountString[^3]), 0, 9, 0, 360 - 36), 0, 0);
            NumberedWheels[3].transform.localEulerAngles = new Vector3(Normalize((float)Char.GetNumericValue(CountString[^4]), 0, 9, 0, 360 - 36), 0, 0);
        }
    }

    public void ResetCounter()
    {
        Count = 9999;
        Increment();
    }

    // found this function at https://stackoverflow.com/questions/51161098/normalize-range-100-to-100-to-0-to-3
    private float Normalize(float val, float valmin, float valmax, float min, float max)
    {
        return (((val - valmin) / (valmax - valmin)) * (max - min)) + min;
    }
}
