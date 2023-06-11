using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsLight : MonoBehaviour
{
    public MeshRenderer[] innerButtons = Array.Empty<MeshRenderer>();
    private Color emissionColor;

    // Start is called before the first frame update
    void Start()
    {
        emissionColor = innerButtons[0].material.GetColor("_EmissionColor");
    }

    public void TurnOnLight(int btnNr)
    {
        for (int i = 0; i < innerButtons.Length; i++)
        {
            if (i == btnNr - 1)
            {
                innerButtons[i].material.EnableKeyword("_EMISSION");
                innerButtons[i].material.SetColor("_EmissionColor", emissionColor);
            }
            else
            {
                innerButtons[i].material.SetColor("_EmissionColor", Color.black);
            }
        }
    }
}
