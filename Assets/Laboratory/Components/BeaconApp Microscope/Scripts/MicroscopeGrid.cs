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

    public int GetTotalAmountOfPlankton()
    {
        return TotalAmountOfPlankton;
    }
}
