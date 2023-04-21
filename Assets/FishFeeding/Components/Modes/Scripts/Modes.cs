using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class Modes : MonoBehaviour
{
    private ModeLoader modeLoader;
    [HideInInspector]
    public List<Mode> modesList;
    [HideInInspector]
    public Mode mode;

    void Start() { modeLoader = FindObjectOfType<ModeLoader>(); }

    void Update()
    {
        if (!modeLoader.finishedLoading || modesList != null) return;

        modesList = modeLoader.modesList;
        mode = modesList[0];
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
    }
}
