using UnityEngine;

public class MicroscopeSlideWithGrid : MonoBehaviour
{
    [SerializeField] private GameObject Grid;
    private MicroscopeGrid GridScript;

    [SerializeField] private MicroscopeMonitor MicroscopeMonitor;
    [SerializeField] private MicroscopeInfoSubmitUI MicroscopeInfoSubmitUI;
    [SerializeField] private NPCMicroscopeTask NPCMicroscopeTask;

    private void Start()
    {
        GridScript = Grid.GetComponentInChildren<MicroscopeGrid>();    
    }

    public void SetMicroscopeSlide()
    {
        MicroscopeMonitor.SetCurrentSlideWithGrid(this);
        MicroscopeInfoSubmitUI.SetCurrentSlideWithGrid(this);
        NPCMicroscopeTask.SetCurrentSlideWithGrid(this);
    }

    public void SetMicroscopeGrid()
    {
        MicroscopeMonitor.SetGrid(Grid);
    }

    public int GetTotalAmountOfPlankton()
    { return MicroscopeMonitor.GetGrid().GetComponent<MicroscopeGrid>().GetTotalAmountOfPlankton(); }

    public int GetTotalAmountOfChaetoceros()
    { return MicroscopeMonitor.GetGrid().GetComponent<MicroscopeGrid>().GetTotalAmountOfChaetoceros(); }

    public int GetTotalAmountOfPseudoNitzschia()
    { return MicroscopeMonitor.GetGrid().GetComponent<MicroscopeGrid>().GetTotalAmountOfPseudoNitzschia(); }

    public int GetTotalAmountOfSkeletonema()
    { return MicroscopeMonitor.GetGrid().GetComponent<MicroscopeGrid>().GetTotalAmountOfSkeletonema(); }
}
