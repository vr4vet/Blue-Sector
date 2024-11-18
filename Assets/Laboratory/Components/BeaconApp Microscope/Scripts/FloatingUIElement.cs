using System;
using UnityEngine;
using UnityEngine.Events;

public class FloatingUIElement : MonoBehaviour
{
    private RectTransform rectTransform;
    private bool isScaling, scalingUp = false;
    private Canvas canvas;
    [SerializeField] private Vector3 closedPosition, openPosition;
    [SerializeField] private float closedScale, openScale;
    [SerializeField] private StartState startState;
    [SerializeField] private bool hideWhenClosed = false;
    [SerializeField] private Canvas canvasToHideWhenClosed;
    private enum StartState
    {
        Closed,
        Open
    }

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Debug.Log(this.name + " " + rectTransform.localPosition);

        if (hideWhenClosed)
            canvasToHideWhenClosed.enabled = false;

        // initiate in closed or open position
        if (startState == StartState.Closed)
        {
            rectTransform.localScale = Vector3.one * closedScale;
            //rectTransform.localPosition = closedPosition;
            rectTransform.anchoredPosition = closedPosition;
        }
        else
        {
            rectTransform.localScale = Vector3.one * openScale;
            rectTransform.anchoredPosition = openPosition;

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
        if (isScaling)
        {
            if (scalingUp)
            {
                if ((float)Math.Truncate(rectTransform.localScale.x * 1000) / 1000 == openScale - 0.001) // check first 3 decimals to prevent excessive precision/time spent scaling. subtraction prevents infinite interpolation
                    isScaling = false;

                float newScale = Mathf.Lerp(rectTransform.localScale.x, openScale, 6f * Time.deltaTime);
                float newPositionX = Mathf.Lerp(rectTransform.anchoredPosition.x, openPosition.x, 6f * Time.deltaTime);
                float newPositionY = Mathf.Lerp(rectTransform.anchoredPosition.y, openPosition.y, 6f * Time.deltaTime);
                float newPositionZ = Mathf.Lerp(rectTransform.localPosition.z, openPosition.z, 6f * Time.deltaTime);
                rectTransform.localScale = Vector3.one * newScale;
                rectTransform.anchoredPosition = new Vector3(newPositionX, newPositionY);
                rectTransform.localPosition += Vector3.forward * newPositionZ;
            }
            if (!scalingUp)
            {
                if ((float)Math.Truncate(rectTransform.localScale.x * 1000) / 1000 ==  closedScale + 0.001 /*(float)Math.Truncate(rectTransform.localScale.x * 100) / 100 == 0*/)
                {
                    isScaling = false;
                    
                    if (hideWhenClosed)
                        canvasToHideWhenClosed.enabled = false; 
                }

                float newScale = Mathf.Lerp(rectTransform.localScale.x, closedScale, 6f * Time.deltaTime);
                float newPositionX = Mathf.Lerp(rectTransform.anchoredPosition.x, closedPosition.x, 6f * Time.deltaTime);
                float newPositionY = Mathf.Lerp(rectTransform.anchoredPosition.y, closedPosition.y, 6f * Time.deltaTime);
                float newPositionZ = Mathf.Lerp(rectTransform.localPosition.z, closedPosition.z, 6f * Time.deltaTime);
                rectTransform.localScale = Vector3.one * newScale;
                rectTransform.anchoredPosition = new Vector3(newPositionX, newPositionY);
                rectTransform.localPosition += Vector3.forward * newPositionZ;
            }
        }
    }

    public void ToggleOnOff()
    {
        if (scalingUp)
            DisableAndScaleDown();
        else
            EnableAndScaleUp();
    }

    public void EnableAndScaleUp()
    {
        if (hideWhenClosed)
            canvasToHideWhenClosed.enabled = true;
        
        scalingUp = true;
        isScaling = true;
    }

    public void DisableAndScaleDown()
    {
        scalingUp = false;
        isScaling = true;
    }

    public bool IsEnabled()
    {
        return canvasToHideWhenClosed.enabled;
    }
}
