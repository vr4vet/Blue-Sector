using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameToolTip : MonoBehaviour
{
    Game script;
    GameObject startGameToolTip;

    // Start is called before the first frame update
    void Start()
    {
        script = FindObjectOfType<Game>();
        startGameToolTip = GameObject.Find("StartGameToolTip");
        startGameToolTip.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (script.inActivatedArea && !script.startGame)
        {
            startGameToolTip.SetActive(true);
        }
        else
        {
            startGameToolTip.SetActive(false);
        }
    }

    public void GiveStartGameToolTip()
    {
        script.inActivatedArea = true;
        startGameToolTip.SetActive(true);
    }
}
