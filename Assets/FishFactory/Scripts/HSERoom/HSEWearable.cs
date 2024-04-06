using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HSEWearable : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.EarProtectionOn = true;
            //TODO: Could add points here
            Destroy(gameObject);
        }
    }
}
