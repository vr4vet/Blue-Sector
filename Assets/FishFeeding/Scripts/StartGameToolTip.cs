using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameToolTip : MonoBehaviour
{
    public GameObject Controller;
    Game script;
    GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        script = FindObjectOfType<Game>();
        canvas = GameObject.Find("StartGameToolTip").transform.GetChild(0).gameObject;
        canvas.GetComponent<Canvas>().enabled = false;
        foreach (var renderer in Controller.GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var canvasComponent = canvas.GetComponent<Canvas>();
        bool visible = script.CanStartGame && !script.startGame;
        if (canvasComponent.enabled != visible)
        {
            canvasComponent.enabled = visible;
            foreach (var renderer in Controller.GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = false;
            }
        }
    }

}
