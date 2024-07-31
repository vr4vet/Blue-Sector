using BNG;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class TutorialEntry : MonoBehaviour
{
    public UnityEvent oncomplete;
    internal Tutorial Tutorial { get; set; }
    public void SetCompleted()
    {
        if (gameObject.activeSelf
            && Tutorial != null
            && Tutorial.Current == this)
        {
            oncomplete.Invoke();
            Tutorial.MoveNext();
        }
    }
}
