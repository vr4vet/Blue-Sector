using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

public class LocalizeToolTip : MonoBehaviour
{

    //public LocalizedStringTable stringTable;
    private LocalizedStringTable stringTable = new LocalizedStringTable { TableReference = "TextFishFeeding" };
    private string translatedString;
    [SerializeField] private string entryKey;

    // Start is called before the first frame update
    /*void Start()
    {
        stringTable.TableChanged += LoadStrings;
    }*/

    // Update is called once per frame
    void Update()
    {
    }

    void OnEnable() {
        stringTable.TableChanged += LoadStrings;
    }

    void OnDisable() {
        stringTable.TableChanged -= LoadStrings;
    }

    void LoadStrings(StringTable stringTable) {
        translatedString = stringTable.GetEntry(entryKey).GetLocalizedString();
        /*Debug.Log("currentlevel:" + translatedCurrentLevel);
        Debug.Log("basic:" + translatedBasic);*/
        GetComponent<Text>().text = translatedString;
    }

    /*void OnDestroy() {
        stringTable.TableChanged -= LoadStrings;
    }*/
}
