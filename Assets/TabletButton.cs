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
        // xr rig index finger tip capsule colliders
        if (other.name.Equals("hands_coll:b_r_index3") || other.name.Equals("hands_coll:b_r_index3 (1)")/*other.GetComponent<RemoteGrabber>() == null && other.GetComponent<UITrigger>() == null && LayerMask.LayerToName(other.gameObject.layer).Equals("Hand")*/)
            button.onClick.Invoke();
    }
}
