using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Levels : MonoBehaviour
{
    /*private GameObject currentLevelObject;*/
    private TextMeshProUGUI currentLevelText;
    private int currentLevel = 1;

    void Start()
    {
        currentLevelText = GetComponent<TextMeshProUGUI>();
    }

    public void ChangeToLevel1()
    {
        if (currentLevel != 1)
        {
            currentLevel = 1;
            currentLevelText.text = "Current level: Level 1";
        }
    }

    public void ChangeToLevel2()
    {
        if (currentLevel != 2)
        {
            currentLevel = 2;
            currentLevelText.text = "Current level: Level 2";
        }
    }

}
