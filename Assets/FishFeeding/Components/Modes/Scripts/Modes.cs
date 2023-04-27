using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Modes : MonoBehaviour
{
    [HideInInspector]
    public List<Mode> modesList;

    [HideInInspector]
    public Mode mode;

    [HideInInspector]
    public event EventHandler<Mode> OnModeChanged;

    private ModeLoader modeLoader;
    private int modeIndex;

    private void Start()
    {
        modeLoader = FindObjectOfType<ModeLoader>();
    }

    private void Update()
    {
        if (!modeLoader.finishedLoading || modesList.Count() > 0) return;

        modesList = modeLoader.modesList;
        mode = modesList[modeIndex];
    }

    public void ChangeTo(int i)
    {
        modeIndex = i;
        if (!modeLoader.finishedLoading)
        {
            return;
        }

        var newMode = modesList[i];
        if (newMode != mode)
        {
            OnModeChanged?.Invoke(this, newMode);
            mode = newMode;
        }
    }
}