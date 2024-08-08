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
                StopAllCoroutines();
                scale.displayText.SetText("000.0");
                scale.audio.Play();
                scale.scaleOn = true;
                break;
            case ButtonType.Off:
                StopAllCoroutines();
                scale.displayText.SetText("");
                scale.audio.Play();
                scale.scaleOn = true;
                break;
            case ButtonType.Reset:
                StopAllCoroutines();
                scale.totalWeight = 0;
                scale.displayText.SetText("000.0");
                scale.audio.Play();
                break;
        }
    }
}
