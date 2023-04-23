using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameToolTip : MonoBehaviour
{
    Game script;
    GameObject startGameToolTip;
    GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        script = FindObjectOfType<Game>();
        startGameToolTip = GameObject.Find("StartGameToolTip");
        canvas = GameObject.Find("StartGameToolTip").transform.GetChild(0).gameObject;
        canvas.GetComponent<Canvas>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (script.CanStartGame && !script.startGame)
        {
            canvas.GetComponent<Canvas>().enabled = true;
        }
        else
        {
            canvas.GetComponent<Canvas>().enabled = false;
        }
    }

    public void GiveStartGameToolTip()
    {
        script.InActivatedArea = true;
        canvas.GetComponent<Canvas>().enabled = true;
        /*startGameToolTip.SetActive(true);*/
    }
}
