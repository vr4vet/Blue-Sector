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
        L1,
        L2
    }
    public ButtonType buttonType;

    void Start()
    {
        game = FindObjectOfType<Game>();
    }

    public void OnButton()
    {
        if (game.startGame) return; // Disable button if game is runnig.
        game.modesClass.ChangeTo(((int)buttonType));
    }
}
