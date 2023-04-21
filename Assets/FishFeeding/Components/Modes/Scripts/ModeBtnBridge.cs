using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class ModeBtnBridge : MonoBehaviour
{  
    private Game game;
    public enum ButtonType
    {
        Next,
        Prev
    }
    public ButtonType buttonType;

    void Start()
    {
        game = FindObjectOfType<Game>();
    }

    public void OnButton()
    {
        if (game.startGame) return; // Disable button if game is runnig.

        switch(buttonType)
        {
            case ButtonType.Next:
                game.modesClass.ChangeToNextMode();
                game.currentMode = game.modesClass.mode;
            break;
            case ButtonType.Prev:
                game.modesClass.ChangeToPreviousMode();
                game.currentMode = game.modesClass.mode;
            break;
            default:
                Debug.LogError("Invalid button-type for modebtn");
            break;
        }
    }
}
