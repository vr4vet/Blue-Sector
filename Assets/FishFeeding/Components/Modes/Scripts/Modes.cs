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

    public event EventHandler<Mode> OnModeChanged;

    void Start() { modeLoader = FindObjectOfType<ModeLoader>(); }

    void Update()
    {
        if (!modeLoader.finishedLoading || modesList.Count() > 0) return;
        modesList = modeLoader.modesList;
        mode = modesList[0];
    }

    public void ChangeTo(int i)
    {
        var newMode = modesList[i];
        if (newMode != mode)
        {
            OnModeChanged?.Invoke(this, newMode);
            mode = newMode;
        }
    }
}
