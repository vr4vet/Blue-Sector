using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ContrastModifier : MonoBehaviour
{
    private Color _defaultDarkBlue = new Color32(0x1C, 0x45, 0x6E, 0xFF);
    private Color _defaultLightBlue = new Color32(0x00, 0x82, 0xD6, 0xFF);//0082D6
    private Color _defaultMiddleBlue = new Color32(0x5C, 0x80, 0xAD, 0xFF);
    private Color _defaultBackground = new Color32(0xF2, 0xF7, 0xFF, 0xFF);
    private Color _defaultPositive = new Color32(0x1C, 0xA3, 0x8C, 0xFF); // 1CA38C
    private Color _contrastDarkBlue;
    private Color _contrastLightBlue;
    private Color _contrastMiddleBlue;
    private Color _contrastBackground;
    private Color _contrastPositive;

    // Start is called before the first frame update
    void Start()
    {
        _contrastDarkBlue = _defaultDarkBlue * .5f;
        _contrastDarkBlue.a = 0xFF;
        _contrastLightBlue = _defaultLightBlue * .5f;
        _contrastLightBlue.a = 0xFF;
        _contrastMiddleBlue = _defaultMiddleBlue * .5f;
        _contrastMiddleBlue.a = 0xFF;
        _contrastBackground = _defaultBackground * 1.5f;
        _contrastBackground.a = 0xFF;
        //Debug.Log(_contrastBackground.ToHexString());
        _contrastBackground = Color.white;
        _contrastPositive = _defaultPositive * .5f;
        _contrastPositive.a = 0xFF;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            ToggleContrast(true);

        if (Input.GetKeyDown(KeyCode.DownArrow))
            ToggleContrast(false);
    }

    public void ToggleContrast(bool contrast)
    {
        if (contrast)
        {
            foreach (Image image in transform.GetComponentsInChildren<Image>(true))
            {
                if (image.color == _defaultDarkBlue)
                    image.color = _contrastDarkBlue;

                if (image.color == _defaultLightBlue)
                    image.color = _contrastLightBlue;

                if (image.color == _defaultMiddleBlue)
                    image.color = _contrastMiddleBlue;

                if (image.color == _defaultBackground)
                {
                    image.color = _contrastBackground;
                    //Debug.Log(image.color.ToHexString());
                }

                if (image.color == _defaultPositive)
                    image.color = _contrastPositive;
            }
        }
        else
        {
            foreach (Image image in transform.GetComponentsInChildren<Image>(true))
            {
                if (image.color == _contrastDarkBlue)
                    image.color = _defaultDarkBlue;

                if (image.color == _contrastLightBlue)
                    image.color = _defaultLightBlue;

                if (image.color == _contrastMiddleBlue)
                    image.color = _defaultMiddleBlue;

                if (image.color == _contrastBackground)
                    image.color = _defaultBackground;

                if (image.color == _contrastPositive)
                    image.color = _defaultPositive;
            }
        }
    }
}
