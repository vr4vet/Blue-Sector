using Unity.VisualScripting;
using UnityEngine;

public class MicroscopeGrid : MonoBehaviour
{
    private int TotalPlanktonAmount, TotalChaetocerosAmount, TotalPseudoNitzschiaAmount, TotalSkeletonemaAmount = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (MicroscopeSlideCell cell in transform.Find("Grid").GetComponentsInChildren<MicroscopeSlideCell>())
        {
            TotalPlanktonAmount += cell.GetTotalPlanktonCount();
            TotalChaetocerosAmount += cell.GetChaetocerosCount();
            TotalPseudoNitzschiaAmount += cell.GetPseudoNitzschiaCount();
            TotalSkeletonemaAmount += cell.GetSkeletonemaCount();
            //cell.gameObject.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 90 * (int)(Random.Range(0, 0.4f) * 10));
            //Debug.Log(90 * (int)(Random.Range(0, 0.4f) * 10));
        }
        
    }

    public int GetTotalAmountOfPlankton()
        { return TotalPlanktonAmount; }

    public int GetTotalAmountOfChaetoceros()
        { return TotalChaetocerosAmount; }

    public int GetTotalAmountOfPseudoNitzschia()
        { return TotalPseudoNitzschiaAmount; }

    public int GetTotalAmountOfSkeletonema()
        { return TotalSkeletonemaAmount; }
}
