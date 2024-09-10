using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class MicroscopeSlideWithGrid : MonoBehaviour
{
    //[SerializeField] private List<MicroscopeSlideCell> Cells = new List<MicroscopeSlideCell>();
    [SerializeField] private GameObject Grid;
    [SerializeField] private MicroscopeMonitor MicroscopeMonitor;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(Cells[0].GetAmountOfPlankton());   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMicroscopeSlide()
    {
        MicroscopeMonitor.SetCurrentSlideWithGrid(this);
    }

    public void SetMicroscopeGrid()
    {
        MicroscopeMonitor.SetGrid(Grid);
    }
}
