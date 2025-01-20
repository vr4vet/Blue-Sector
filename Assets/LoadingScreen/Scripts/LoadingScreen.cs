using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject LoadingBar;

    private void Awake()
    {
        // Apply the player camera as target camera for the camera space canvas. Make UI layer the only rendered layer to only show loading screen.
        Camera PlayerCamera = Camera.main;
        PlayerCamera.cullingMask = LayerMask.GetMask("LoadingScreen");
        GetComponentInChildren<Canvas>().worldCamera = PlayerCamera;
    }

    /// <summary>
    /// Method to fill the bar to show the player the loading progress.
    /// </summary>
    /// <param name="amount"></param>
    public void SetFillAmount(float amount)
    {
        LoadingBar.GetComponent<Image>().fillAmount = amount;
    }
}
