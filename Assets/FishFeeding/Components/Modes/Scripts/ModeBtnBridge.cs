using UnityEngine;

public class ModeBtnBridge : MonoBehaviour
{
    private Game game;

    [HideInInspector]
    public enum ButtonType
    {
        L1,
        L2
    }

    [Tooltip("Which mode should the button switch to?")]
    public ButtonType buttonType;

    private void Awake()
    {
        game = FindObjectOfType<Game>();
    }

    public void OnButton()
    {
        if (game.startGame) return; // Disable button if game is runnig.
        game.modesClass.ChangeTo(((int)buttonType));
    }
}