using System;
using UnityEngine;
using UnityEngine.UI;

public class FloatingUIElement : MonoBehaviour
{
    private RectTransform _rectTransform;
    private bool _isScaling, _scalingUp = false;
    private float _enableTime;
    private Button _button;
    [SerializeField] private Vector3 closedPosition, openPosition;
    [SerializeField] private float closedScale, openScale;
    [SerializeField] private StartState startState;
    [SerializeField] private float closeAutomaticallyAfterSeconds;
    [SerializeField] private Canvas canvasToHideWhenClosed;
    private enum StartState
    {
        Closed,
        Open
    }

    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _button = GetComponent<Button>();

        // initiate in closed or open position
        if (startState == StartState.Closed)
        {
            _rectTransform.localScale = Vector3.one * closedScale;
            _rectTransform.anchoredPosition = closedPosition;

            if (canvasToHideWhenClosed)
                canvasToHideWhenClosed.enabled = false;
        }
        else
        {
            _rectTransform.localScale = Vector3.one * openScale;
            _rectTransform.anchoredPosition = openPosition;

            if (canvasToHideWhenClosed)
                canvasToHideWhenClosed.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // keyboard input for debugging and testing purposes
/*        if (Input.GetKeyDown(KeyCode.O))
            EnableAndScaleUp();
        if (Input.GetKeyDown(KeyCode.P))
            DisableAndScaleDown();
        if (Input.GetKeyDown(KeyCode.U))
            ToggleOnOff();*/

        // perform the opening/closing of canvas
        if (_isScaling)
        {
            if (_scalingUp)
            {
                if ((float)Math.Truncate((openScale - _rectTransform.localScale.x) * 1000) / 1000 <= 0.001) // check first 3 decimals to prevent excessive precision/time spent scaling. subtraction prevents infinite interpolation
                {
                    _isScaling = false;
                    _button.interactable = false;
                    _button.interactable = true;
                }

                float newScale = Mathf.Lerp(_rectTransform.localScale.x, openScale, 6f * Time.deltaTime);
                float newPositionX = Mathf.Lerp(_rectTransform.anchoredPosition.x, openPosition.x, 6f * Time.deltaTime);
                float newPositionY = Mathf.Lerp(_rectTransform.anchoredPosition.y, openPosition.y, 6f * Time.deltaTime);
                float newPositionZ = Mathf.Lerp(_rectTransform.localPosition.z, openPosition.z, 6f * Time.deltaTime);
                _rectTransform.localScale = Vector3.one * newScale;
                _rectTransform.anchoredPosition = new Vector3(newPositionX, newPositionY);
                _rectTransform.localPosition += Vector3.forward * newPositionZ;
            }
            if (!_scalingUp)
            {
                if ((float)Math.Truncate((_rectTransform.localScale.x - closedScale) * 1000) / 1000 <= 0.001)
                {
                    _isScaling = false;
                    _button.interactable = false;
                    _button.interactable = true;
                    
                    if (canvasToHideWhenClosed)
                        canvasToHideWhenClosed.enabled = false; 
                }

                float newScale = Mathf.Lerp(_rectTransform.localScale.x, closedScale, 6f * Time.deltaTime);
                float newPositionX = Mathf.Lerp(_rectTransform.anchoredPosition.x, closedPosition.x, 6f * Time.deltaTime);
                float newPositionY = Mathf.Lerp(_rectTransform.anchoredPosition.y, closedPosition.y, 6f * Time.deltaTime);
                float newPositionZ = Mathf.Lerp(_rectTransform.localPosition.z, closedPosition.z, 6f * Time.deltaTime);
                _rectTransform.localScale = Vector3.one * newScale;
                _rectTransform.anchoredPosition = new Vector3(newPositionX, newPositionY);
                _rectTransform.localPosition += Vector3.forward * newPositionZ;
            }
        }

        // automatically close after provided time in seconds has passed
        if (closeAutomaticallyAfterSeconds > 0 && _scalingUp)
        {
            if (Time.time - _enableTime > closeAutomaticallyAfterSeconds)
                DisableAndScaleDown();
        }
    }

    public void ToggleOnOff()
    {
        if (_scalingUp)
            DisableAndScaleDown();
        else
            EnableAndScaleUp();
    }

    public void EnableAndScaleUp()
    {
        _enableTime = Time.time;

        if (canvasToHideWhenClosed)
            canvasToHideWhenClosed.enabled = true;
        
        _scalingUp = true;
        _isScaling = true;
    }

    public void DisableAndScaleDown()
    {
        _scalingUp = false;
        _isScaling = true;
    }

    public bool IsEnabled()
    {
        return canvasToHideWhenClosed.enabled;
    }
}
