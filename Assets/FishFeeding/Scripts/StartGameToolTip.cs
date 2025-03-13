using UnityEngine;

public class StartGameToolTip : MonoBehaviour
{
    [SerializeField] private GameObject _controller;
    [SerializeField] private GameObject _arrow;
    [SerializeField] private Game _gameScript;
    [SerializeField] private GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        _gameScript = FindObjectOfType<Game>();
        canvas = transform.GetChild(0).gameObject;
        canvas.GetComponent<Canvas>().enabled = false;
        _controller.SetActive(false);
        _arrow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        var canvasComponent = canvas.GetComponent<Canvas>();
        bool visible = _gameScript.CanStartGame && !_gameScript.startGame;
        if (canvasComponent.enabled != visible)
        {
            canvasComponent.enabled = visible;
            _controller.SetActive(visible);
            _arrow.SetActive(visible);
        }
    }
}
