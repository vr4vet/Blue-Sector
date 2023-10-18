using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public class Levels : MonoBehaviour
{
    public LocalizedStringTable stringTable = new LocalizedStringTable { TableReference = "TextFishFeeding" };
    private Modes modes;
    private TextMeshProUGUI currentLevelText;
    private int currentLevel = 1;
    private string translatedCurrentLevel;
    private string translatedBasic;
    private string translatedAdvanced;

    void Awake()
    {
        Debug.Log("Test");
        currentLevelText = GetComponentInChildren<TextMeshProUGUI>();
        modes = FindObjectOfType<Modes>();
        modes.OnModeChanged += Levels_OnModeChanged;
    }

    public void ChangeToLevel1()
    {
        if (currentLevel != 1)
        {
            if (!LocalizationSettings.InitializationOperation.IsDone)
            {
                return;
            }
            currentLevel = 1;
            UpdateLevelText(translatedBasic);
        }
    }

    public void ChangeToLevel2()
    {
        if (currentLevel != 2)
        {
            //Debug.Log("level2");
            if (!LocalizationSettings.InitializationOperation.IsDone)
            {
                return;
            }
            //Debug.Log("become lv2");
            currentLevel = 2;
            UpdateLevelText(translatedAdvanced);
        }
    }

    void OnEnable()
    {
        stringTable.TableChanged += LoadStrings;
    }

    void OnDisable()
    {
        stringTable.TableChanged -= LoadStrings;
    }

    void LoadStrings(StringTable stringTable)
    {
        translatedCurrentLevel = stringTable.GetEntry("CurrentLevel").GetLocalizedString();
        translatedBasic = stringTable.GetEntry("Basic").GetLocalizedString();
        translatedAdvanced = stringTable.GetEntry("Advanced").GetLocalizedString();
        /*Debug.Log("currentlevel:" + translatedCurrentLevel);
        Debug.Log("basic:" + translatedBasic);*/
        if (currentLevel == 1)
        {
            UpdateLevelText(translatedBasic);
        } else
        {
            UpdateLevelText(translatedAdvanced);
        }
    }

    private void UpdateLevelText(string translatedLevel)
    {
        /*Debug.Log("Updated text");*/
        currentLevelText.text = translatedCurrentLevel + " " + translatedLevel.ToUpper();
    }


    private void Levels_OnModeChanged(object sender, Mode e)
    {
        switch (e.name)
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
