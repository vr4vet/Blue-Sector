using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPaused : MonoBehaviour {

    public bool isPaused;
    public bool pauseMenu;

    // Checks if game is paused
    void ChangePause()
    {
        if(isPaused)
        {
            isPaused = false;
        }
        else
        {
            isPaused = true;
        }
    }
}
