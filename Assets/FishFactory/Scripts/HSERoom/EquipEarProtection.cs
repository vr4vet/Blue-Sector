using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipEarProtection : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.EarProtectionOn = true;
            GameManager.instance.PlaySound("correct");
            Destroy(gameObject);
        }
    }
}
