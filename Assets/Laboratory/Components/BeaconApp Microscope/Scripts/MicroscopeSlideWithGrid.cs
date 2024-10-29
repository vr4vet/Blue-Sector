using UnityEngine;

public class MicroscopeSlideWithGrid : MonoBehaviour
{
    [SerializeField] private GameObject Grid;
    private MicroscopeGrid GridScript;

    [SerializeField] private MicroscopeMonitor MicroscopeMonitor;
    [SerializeField] private MicroscopeInfoSubmitUI MicroscopeInfoSubmitUI;
    [SerializeField] private NPCAnswerAssessment NPCAnswerAssessment;

    private void Start()
    {
        GridScript = Grid.GetComponentInChildren<MicroscopeGrid>();    
    }

    public void SetMicroscopeSlide()
    {
        MicroscopeMonitor.SetCurrentSlideWithGrid(this);
        MicroscopeInfoSubmitUI.SetCurrentSlideWithGrid(this);
        NPCAnswerAssessment.SetCurrentSlideWithGrid(this);
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
