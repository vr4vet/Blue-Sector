using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleButtons : MonoBehaviour
{
    public Scale scale;
    public enum ButtonType
    {
        On,
        Off,
        Reset,
    }

    [SerializeField]
    public ButtonType buttonType;

    private void OnTriggerEnter(Collider collisionObject)
    {
        switch (buttonType)
        {
            case ButtonType.On:
                Debug.Log("on");
                break;
            case ButtonType.Off:
                Debug.Log("off");
                break;
            case ButtonType.Reset:
                scale.totalWeight = 0;
                scale.displayText.SetText("0.000");
                Debug.Log("Weight reset");
                break;
        }
    }
}
