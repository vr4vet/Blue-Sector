using UnityEngine;

public class MicroscopeSlideWithGrid : MonoBehaviour
{
    [SerializeField] private GameObject Grid;
    [SerializeField] private MicroscopeMonitor MicroscopeMonitor;

    public void SetMicroscopeSlide()
    {
        MicroscopeMonitor.SetCurrentSlideWithGrid(this);
    }

    public void SetMicroscopeGrid()
    {
        MicroscopeMonitor.SetGrid(Grid);
    }
}
