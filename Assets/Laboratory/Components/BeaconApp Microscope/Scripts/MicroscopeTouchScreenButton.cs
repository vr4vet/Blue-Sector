using UnityEngine;
using UnityEngine.UI;

public class MicroscopeTouchScreenButton : MonoBehaviour
{
    private bool IsTouched = false;
    [SerializeField] private Input input;
    private Button _button;
    private bool _screenControlsStepCompleted = false;
    private enum Input
    {
        Up, Down, Left, Right, Magnify, Minimize, Faster, Slower, InfoSubmit
    }
    private MicroscopeMonitor MicroscopeMonitor;

    private void Start()
    {
        MicroscopeMonitor = transform.root.GetComponentInChildren<MicroscopeMonitor>();
        _button = GetComponent<Button>();
    }

    private void FixedUpdate()
    {
        if (IsTouched)
        {
            switch(input)
            {
                case Input.Left:
                    MicroscopeMonitor.ScrollLeft();
                    break;
                case Input.Right:
                    MicroscopeMonitor.ScrollRight();
                    break;
                case Input.Up:
                    MicroscopeMonitor.ScrollUp();
                    break;
                case Input.Down:
                    MicroscopeMonitor.ScrollDown();
                    break;
                default:
                    break;
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // check if player finger tip
        if (other.name.Equals("hands_coll:b_r_index3") || other.name.Equals("hands_coll:b_r_index3 (1)")/*other.name == "tip_collider_i"*/)
        {

            CompleteScreenControlsStep();

            IsTouched = true;
                
            switch (input)
            {
                case Input.Magnify:
                    if (!MicroscopeMonitor.RevolvingNosePiece.IsRotating())
                        MicroscopeMonitor.Magnify();
                    break;
                case Input.Minimize:
                    if (!MicroscopeMonitor.RevolvingNosePiece.IsRotating())
                        MicroscopeMonitor.Minimize();
                    break;
                case Input.Faster:
                    CompleteScreenControlsStep();
                    MicroscopeMonitor.IncreaseScrollSpeed();
                    break;
                case Input.Slower:
                    CompleteScreenControlsStep();
                    MicroscopeMonitor.DecreaseScrollSpeed();
                    break;
                default:
                    break;
            }
        }
    }

    private void CompleteScreenControlsStep()
    {
        if (!_screenControlsStepCompleted)
        {
            _button.onClick.Invoke();
            _screenControlsStepCompleted = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // check if player hand
        if (other.name == "tip_collider_i")
        {
            IsTouched = false;
        }
    }

    
}
