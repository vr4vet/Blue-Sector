using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroscopeGrid : MonoBehaviour
{
    private int TotalAmountOfPlankton = 0;
    // Start is called before the first frame update
    void Start()
    {
        foreach (MicroscopeSlideCell cell in transform.Find("Panel").GetComponentsInChildren<MicroscopeSlideCell>())
        {
            TotalAmountOfPlankton += cell.GetAmountOfPlankton();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetTotalAmountOfPlankton()
    {
        return TotalAmountOfPlankton;
    }
}
