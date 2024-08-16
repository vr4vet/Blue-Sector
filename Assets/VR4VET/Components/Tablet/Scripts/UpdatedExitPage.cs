using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatedExitPage : MonoBehaviour
{
    public GameObject page;

    private void OnTriggerEnter(Collider other) {
        page.SetActive(false);
    }
}
