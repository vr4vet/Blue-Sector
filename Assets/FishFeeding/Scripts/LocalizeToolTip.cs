using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

public class LocalizeToolTip : MonoBehaviour
{

    [SerializeField] private LocalizedStringTable stringTable;
    private string translatedString;
    [SerializeField] private string entryKey;


    void OnEnable() {
        stringTable.TableChanged += LoadStrings;
        UpdateString();
    }

    void OnDisable() {
        stringTable.TableChanged -= LoadStrings;
    }

    void LoadStrings(StringTable stringTable) {
        translatedString = stringTable.GetEntry(entryKey).GetLocalizedString();
        UpdateString();
    }

    private void UpdateString() {
        GetComponent<TooltipScript>().TextContent = translatedString;
    }

}
