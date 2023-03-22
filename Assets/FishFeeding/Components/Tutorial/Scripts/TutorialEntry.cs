using BNG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TutorialEntry : MonoBehaviour
{
    public string Text = string.Empty;
    private bool isActive;
    private GameObject currentHint;
    private Popup currentPopup;

    public bool IsActive
    {
        get => isActive;
        internal set
        {
            if (isActive != value)
            {
                isActive = value;
                if (value)
                {
                    currentHint = Instantiate(Tutorial.PopupHint);
                    var tmp = currentHint.GetComponent<TextMeshPro>();
                    tmp.text = Text;
                    currentPopup = currentHint.GetComponent<Popup>();
                    currentPopup.transform.position = transform.position;
                    currentPopup.Show();
                }
                else
                {
                    currentPopup.Hide();
                    currentHint = null;
                }
            }
        }
    }

    internal Tutorial Tutorial { get; set; }

    public void SetCompleted()
    {
        if (IsActive
            && Tutorial != null
            && Tutorial.Current == this)
        {
            Tutorial.MoveNext();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
