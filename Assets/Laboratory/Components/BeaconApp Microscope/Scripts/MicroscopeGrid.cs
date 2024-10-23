using Unity.VisualScripting;
using UnityEngine;

public class MicroscopeGrid : MonoBehaviour
{
    private int TotalAmountOfPlankton = 0;
    // Start is called before the first frame update
    void Start()
    {
        foreach (MicroscopeSlideCell cell in transform.Find("Grid").GetComponentsInChildren<MicroscopeSlideCell>())
        {
            TotalAmountOfPlankton += cell.GetTotalPlanktonCount();
            //cell.gameObject.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 90 * (int)(Random.Range(0, 0.4f) * 10));
            //Debug.Log(90 * (int)(Random.Range(0, 0.4f) * 10));
        }
        
    }

    public int GetTotalAmountOfPlankton()
    {
        return TotalAmountOfPlankton;
    }
}
