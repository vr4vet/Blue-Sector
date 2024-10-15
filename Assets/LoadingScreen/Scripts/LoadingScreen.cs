using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject LoadingBar;

/*    private void FixedUpdate()
    {
        LoadingBar.GetComponent<Image>().fillAmount = .5f;
        
    }*/

    public void BarFillAmount(float amount)
    {
        LoadingBar.GetComponent<Image>().fillAmount = amount;
    }
}
