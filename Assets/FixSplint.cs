using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixSplint : MonoBehaviour
{
    public GameObject splint;
    public GameObject splintGuide;
    public GameObject handSplint;

    void OnTriggerEnter(Collider other)
    {
        splint.SetActive(true);
        splintGuide.SetActive(false);
        handSplint.SetActive(false);
        
    }
}
