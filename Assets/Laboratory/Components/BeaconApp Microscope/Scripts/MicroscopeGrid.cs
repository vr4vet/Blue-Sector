using UnityEngine;
using System.Collections.Generic;

public class MicroscopeGrid : MonoBehaviour
{
    private int TotalPlanktonAmount, TotalChaetocerosAmount, TotalPseudoNitzschiaAmount, TotalSkeletonemaAmount = 0;

    /// <summary>
    /// Go through all cells and add plankton together.
    /// Seperated into its own method to prevent MicroscopeScreenSpaceOverlay.cs from also doing this as its computationally expensive.
    /// </summary>
    public void FetchPlanktonCount()
    {
        foreach (MicroscopeSlideCell cell in transform.Find("Grid").GetComponentsInChildren<MicroscopeSlideCell>())
        {
            TotalPlanktonAmount += cell.GetTotalPlanktonCount();
            TotalChaetocerosAmount += cell.GetChaetocerosCount();
            TotalPseudoNitzschiaAmount += cell.GetPseudoNitzschiaCount();
            TotalSkeletonemaAmount += cell.GetSkeletonemaCount();
        }
    }

    /// <summary>
    /// Randomly rotate all cells randomly and return a list containing the generated rotations.
    /// </summary>
    /// <returns></returns>
    public List<int> RandomlyRotateCells()
    {
        List<int> cellRotations = new List<int>();
        foreach (MicroscopeSlideCell cell in transform.Find("Grid").GetComponentsInChildren<MicroscopeSlideCell>())
        {
            int randomRotation = 90 * (int)(Random.Range(0, 0.4f) * 10);
            cell.gameObject.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, randomRotation);
            cellRotations.Add(randomRotation);
        }

        return cellRotations;
    }

    /// <summary>
    /// Rotate all cells using a list of rotations. Ment to be used in tandem with RandomlyRotateCells().
    /// </summary>
    /// <param name="cellRotations"></param>
    public void RotateCells(List<int> cellRotations)
    {
        int i = 0;
        foreach (MicroscopeSlideCell cell in transform.Find("Grid").GetComponentsInChildren<MicroscopeSlideCell>())
        {
            cell.gameObject.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, cellRotations[i]);
            i++;
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
