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

    [HideInInspector]
    public event EventHandler<Mode> OnFinishedLoading;

    private ModeLoader modeLoader;
    private int modeIndex;

    private void Start()
    {
        modeLoader = FindObjectOfType<ModeLoader>();
        modeLoader.OnFinishedLoading += InitializeMode;
    }

    private void InitializeMode(object sender, List<Mode> e)
    {
        modesList = e;
        mode = modesList[modeIndex];
        OnFinishedLoading?.Invoke(this, mode);
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