using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour
{
    public TutorialEntry[] Items = Array.Empty<TutorialEntry>();
    public GameObject PopupHint;

    /// <summary>
    /// Gets an event which is fired when all the tutorial entires have been completed.
    /// </summary>
    public UnityEvent OnCompleted;

    /// <summary>
    /// Gets an event which is fired when the tutorial is triggered.
    /// </summary>
    public UnityEvent OnTriggered;

    private int indexOfCurrentItem = -1;
    private bool triggered;
    private bool dismissed;

    public TutorialEntry Current
        => IndexOfCurrentItem >= 0 && IndexOfCurrentItem < Items.Length
        ? Items[IndexOfCurrentItem]
        : null;

    private int IndexOfCurrentItem
    {
        get => indexOfCurrentItem;
        set
        {
            if (indexOfCurrentItem == value)
                return;

            if (Current != null)
            {
                Current.IsActive = false;
            }

            indexOfCurrentItem = value;

            if (Current != null)
            {
                Current.IsActive = true;
            }
        }
    }

    public bool Triggered
    {
        get => triggered;
        internal set
        {
            if (triggered == value)
                return;

            triggered = value;
            if (value && !dismissed)
            {
                IndexOfCurrentItem = 0;
            }
        }
    }

    /// <summary>
    /// Starts the tutorial if it has not already been started.
    /// </summary>
    public void Trigger()
    {
        Triggered = true;
    }

    /// <summary>
    /// Dismisses the tutorial, removing all UI elements from the scene.
    /// </summary>
    public void Dismiss()
    {
        IndexOfCurrentItem = -1;
    }

    /// <summary>
    /// Advances to the next tutorial entry, or completes the tutorial if all entries are enumerated.
    /// </summary>
    /// <returns><see langword="true"/> if there are more entires in the tutorial.</returns>
    public bool MoveNext()
    {
        IndexOfCurrentItem++;
        Debug.Log("index: " + indexOfCurrentItem);
        if (IndexOfCurrentItem == Items.Length && Items.Length > 0)
        {
            Debug.Log("true");
            OnCompleted.Invoke();
        }

        return IndexOfCurrentItem < Items.Length;
    }

    /// <summary>
    /// Restores the previous tutorial item.
    /// </summary>
    /// <returns><see langword="true"/> if the tutorial is still active.</returns>
    public bool MoveBack()
    {
        IndexOfCurrentItem = Math.Max(IndexOfCurrentItem, 0) - 1;

        return IndexOfCurrentItem >= 0;
    }

    // Start is called before the first frame update
    private void Start()
    {
        foreach (var entry in Items)
        {
            entry.Tutorial = this;
        }
    }
}