using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class Modes : MonoBehaviour
{
    // [field: SerializeField]
    // public GameObject ModeLoaderParent { get; set; }
    private ModeLoader modeLoader;
    private TextMeshProUGUI currentModeText;
    public List<Mode> modesList;
    public Mode mode;

    void Start()
    {
        currentModeText = GetComponent<TextMeshProUGUI>();    
        modeLoader = FindObjectOfType<ModeLoader>();
    }

    void Update()
    {
        if (modeLoader.finishedLoading == true && modesList == null)
        {
            modesList = modeLoader.modesList;
            mode = modesList[0];
            updateText();
        }
    }

    public void ChangeToNextMode()
    {
        int length = modesList.Count() - 1;
        int index  = modesList.FindIndex(a => a.name == mode.name);

        if (index >= length)
        {
            mode = modesList[0];
        } else {
            mode = modesList[index + 1];
        }
        updateText();
    }

    public void ChangeToPreviousMode()
    {
        int length = modesList.Count() - 1;
        int index = modesList.FindIndex(a => a.name == mode.name);
        if (index <= 0)
        {
            mode = modesList[length];
        } else {
            mode = modesList[index - 1];
        }
        updateText();
    }

    private void updateText()
    {
        string text  = "Current mode: " + mode.name;
        // currentModeText.text = text;
        Debug.Log(text);
    }
}
