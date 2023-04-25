using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using Unity.XR.Oculus.Input;
using System;
using UnityEngine.Events;

public class Modes : MonoBehaviour
{
    private ModeLoader modeLoader;
    public List<Mode> modesList;
    public Mode mode;
    private int modeIndex;

    public event EventHandler<Mode> OnModeChanged;

    void Start() { modeLoader = FindObjectOfType<ModeLoader>(); }

    void Update()
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
