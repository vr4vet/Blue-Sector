using System;
using UnityEngine;
using UnityEngine.Events;

public class MicroscopeFloatingUIToggle : MonoBehaviour
{
    private RectTransform rectTransform;
    private bool isScaling, scalingUp = false;
    private Canvas canvas;
    private Vector3 openPosition, openScale;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        openPosition = rectTransform.localPosition;
        openScale = rectTransform.localScale;
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;

        // scale down and hide behind monitor/UI element to its left
        rectTransform.localPosition = new Vector3(rectTransform.localPosition.x - 0.2f, rectTransform.localPosition.y, rectTransform.localPosition.z + 0.2f);
        rectTransform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // keyboard input for debugging and testing purposes
        if (Input.GetKeyDown(KeyCode.O))
            EnableAndScaleUp();
        if (Input.GetKeyDown(KeyCode.P))
            DisableAndScaleDown();
        if (Input.GetKeyDown(KeyCode.U))
            ToggleOnOff();

        // perform the opening/closing of canvas
        if (isScaling)
        {
            if (scalingUp)
            {
                if ((float)Math.Truncate(rectTransform.localScale.x * 1000) / 1000 == openScale.x - 0.001) // check first 3 decimals to prevent excessive precision/time spent scaling. subtraction prevents infinite interpolation
                    isScaling = false;

                float newScale = Mathf.Lerp(rectTransform.localScale.x, openScale.x, 5f * Time.deltaTime);
                float newPositionX = Mathf.Lerp(rectTransform.localPosition.x, openPosition.x, 5f * Time.deltaTime);
                float newPositionZ = Mathf.Lerp(rectTransform.localPosition.z, openPosition.z, 5f * Time.deltaTime);
                rectTransform.localScale = Vector3.one * newScale;
                rectTransform.localPosition = new Vector3(newPositionX, openPosition.y, newPositionZ);
            }
            if (!scalingUp)
            {
                if (rectTransform.localScale.x <= openScale.x / 6 /*(float)Math.Truncate(rectTransform.localScale.x * 100) / 100 == 0*/)
                {
                    isScaling = false;
                    canvas.enabled = false;
                }

                float newScale = Mathf.Lerp(rectTransform.localScale.x, 0, 6f * Time.deltaTime);
                float newPositionX = Mathf.Lerp(rectTransform.localPosition.x, openPosition.x - 0.2f, 6f * Time.deltaTime);
                float newPositionZ = Mathf.Lerp(rectTransform.localPosition.z, openPosition.z + 0.2f, 6f * Time.deltaTime);
                rectTransform.localScale = Vector3.one * newScale;
                rectTransform.localPosition = new Vector3(newPositionX, openPosition.y, newPositionZ);
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
        if (canvas.enabled)
            return;

        canvas.enabled = true;
        scalingUp = true;
        isScaling = true;
    }

    public void DisableAndScaleDown()
    {
        if (!canvas.enabled)
            return;

        scalingUp = false;
        isScaling = true;
    }

    public bool IsEnabled()
    {
        return canvas.enabled;
    }

    /// <summary>
    /// Disable the target UI canvas before this. Useful for nested UIs, for example when closing the microscope's info-submit UI without closing the numpad UI first.
    /// </summary>
    /// <param name="target"></param>
    public void DisableAndScaleDownTargetThenThis(MicroscopeFloatingUIToggle target)
    {
        // disable target UI and then this after a short wait if target is enabled. otherwise simply disable this as ususal
        if (target.IsEnabled())
        {
            target.DisableAndScaleDown();
            Invoke("DisableAndScaleDown", .25f);
        }
        else
            DisableAndScaleDown();
    }
}
