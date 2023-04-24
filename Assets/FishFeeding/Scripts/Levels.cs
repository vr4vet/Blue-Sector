using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Levels : MonoBehaviour
{
    /*private GameObject currentLevelObject;*/
    private Modes modes;
    private TextMeshProUGUI currentLevelText;
    private int currentLevel = 1;

    void Start()
    {
        currentLevelText = GetComponentInChildren<TextMeshProUGUI>();
        modes = FindObjectOfType<Modes>();
        modes.OnModeChanged += Levels_OnModeChanged;
    }

    public void ChangeToLevel1()
    {
        if (currentLevel != 1)
        {
            currentLevel = 1;
            currentLevelText.text = "Nåværende nivå: GRUNNLEGGENDE";
        }
    }

    public void ChangeToLevel2()
    {
        if (currentLevel != 2)
        {
            currentLevel = 2;
            currentLevelText.text = "Nåværende nivå: AVANSERT";
        }
    }

    private void Levels_OnModeChanged(object sender, Mode e)
    {
        switch(e.name)
        {
            case "arcade":
                ChangeToLevel1();
                break;
            case "realism":
                ChangeToLevel2();
                break;
        }
    }
}
