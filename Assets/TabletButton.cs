using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabletButton : MonoBehaviour
{
    private UnityEngine.UI.Button button;

    void Start()
    {
        button = gameObject.GetComponent<UnityEngine.UI.Button>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<RemoteGrabber>() == null && other.GetComponent<UITrigger>() == null && LayerMask.LayerToName(other.gameObject.layer).Equals("Hand"))
        {
            Debug.Log(other.name);
            button.onClick.Invoke();
        }
    }

}
