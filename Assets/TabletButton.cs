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

        Debug.Log("triggerEntered");
        if (other.CompareTag("Hand"))
        {
            button.onClick.Invoke();
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collisionEntered");
        Debug.Log(collision.collider.tag);
        button.onClick.Invoke();
    }

}
