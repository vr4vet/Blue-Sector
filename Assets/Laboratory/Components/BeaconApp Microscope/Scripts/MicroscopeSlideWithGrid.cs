using UnityEngine;

public class MicroscopeSlideWithGrid : MonoBehaviour
{
    [SerializeField] private GameObject Grid;
    private MicroscopeGrid GridScript;

    [SerializeField] private MicroscopeMonitor MicroscopeMonitor;
    [SerializeField] private MicroscopeInfoSubmitUI MicroscopeInfoSubmitUI;

    private void Start()
    {
        GridScript = Grid.GetComponentInChildren<MicroscopeGrid>();    
    }

    public void SetMicroscopeSlide()
    {
        MicroscopeMonitor.SetCurrentSlideWithGrid(this);
        MicroscopeInfoSubmitUI.SetCurrentSlideWithGrid(this);
    }

    public void SetMicroscopeGrid()
    {
        MicroscopeMonitor.SetGrid(Grid);
    }

    public int GetTotalAmountOfPlankton()
    { return GridScript.GetTotalAmountOfPlankton(); }

    public int GetTotalAmountOfChaetoceros()
    { return GridScript.GetTotalAmountOfChaetoceros(); }

    public int GetTotalAmountOfPseudoNitzschia()
    { return GridScript.GetTotalAmountOfPseudoNitzschia(); }

    public int GetTotalAmountOfSkeletonema()
    { return GridScript.GetTotalAmountOfSkeletonema(); }
}
