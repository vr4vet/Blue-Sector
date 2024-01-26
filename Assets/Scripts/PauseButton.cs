using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    private Valve.VR.InteractionSystem.Hand hand;
    public GameObject pausemenu;
    public GameObject pauseText;
    public IsPaused paused;

    void Start()
    {
        hand = gameObject.GetComponent<Valve.VR.InteractionSystem.Hand>();
        
    }

    void Update()
    {
        if (hand.controller == null) return;
        // Opens pausemenu on menu-button click
        if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            PauseMenu();
            
        }
    }
    // Opens pausemenu and slows down time
    public void PauseMenu()
    {
        if (!paused.pauseMenu)
        {
            pausemenu.SetActive(true);
            pauseText.SetActive(false);
            Time.timeScale = 0.1f;
            paused.pauseMenu = true;
        }
        else
        {
            pausemenu.SetActive(false);
            pauseText.SetActive(false);
            Time.timeScale = 1;
            paused.pauseMenu = false;
            paused.isPaused = false;
        }
    }
}