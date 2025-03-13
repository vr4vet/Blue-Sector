using UnityEngine;

public class StartGameToolTip : MonoBehaviour
{
    [SerializeField] private GameObject _controller;
    [SerializeField] private GameObject _arrow;
    [SerializeField] private Game _gameScript;
    [SerializeField] private GameObject _canvas;

    // Update is called once per frame
    void Update()
    {
        bool visible = _gameScript.CanStartGame && !_gameScript.startGame;
        if (_canvas.activeSelf != visible)
        {
            _canvas.SetActive(visible);
            _controller.SetActive(visible);
            _arrow.SetActive(visible);
        }
    }
}
