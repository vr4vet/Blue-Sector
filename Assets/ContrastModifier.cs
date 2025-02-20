using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ContrastModifier : MonoBehaviour
{
    private Color _defaultDarkBlue = new Color32(0x1C, 0x45, 0x6E, 0xFF);
    private Color _defaultLightBlue = new Color32(0x00, 0x82, 0xD6, 0xFF);
    private Color _defaultMiddleBlue = new Color32(0x5C, 0x80, 0xAD, 0xFF);
    private Color _defaultBackground = new Color32(0xF2, 0xF7, 0xFF, 0xFF);
    private Color _defaultPositive = new Color32(0x1C, 0xA3, 0x8C, 0xFF);
    private Color _defaultBrandPurple = new Color32(0x8A, 0x99, 0xFA, 0xFF);
    private Color _fullyWhite = Color.white;
    private Color _contrastDarkBlue;
    private Color _contrastLightBlue;
    private Color _contrastMiddleBlue;
    private Color _contrastBackground;
    private Color _contrastPositive;
    private Color _contrastBrandPurple;
    private Color _contrastWhite;

    private bool _highContrastMode = false;

    // Start is called before the first frame update
    void Start()
    {
        _contrastDarkBlue = _defaultDarkBlue * .5f;
        _contrastDarkBlue.a = 0xFF;

        _contrastLightBlue = _defaultLightBlue * .5f;
        _contrastLightBlue.a = 0xFF;

        _contrastMiddleBlue = _defaultMiddleBlue * .5f;
        _contrastMiddleBlue.a = 0xFF;

        _contrastBackground = Color.white;

        _contrastPositive = _defaultPositive * .5f;
        _contrastPositive.a = 0xFF;

        _contrastBrandPurple = _defaultBrandPurple * .5f;
        _contrastBrandPurple.a = 0xFF;

        _contrastWhite = _fullyWhite * .5f;
        _contrastWhite.a = 0xFF;

        WatchManager.Instance.UIChanged.AddListener(OnUIChanged);
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
        _highContrastMode = contrast;
        if (_highContrastMode)
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
                    image.color = _contrastBackground;

                if (image.color == _defaultPositive)
                    image.color = _contrastPositive;

                if (image.color == _defaultBrandPurple)
                    image.color = _contrastBrandPurple;
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

                if (image.color == _contrastBrandPurple)
                    image.color = _defaultBrandPurple;
            }
        }
    }

    private void OnUIChanged()
    {
        Debug.Log("ContrastModifier noticed");
        ToggleContrast(_highContrastMode);
    }
}
