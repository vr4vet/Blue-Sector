using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatTutorialPoints : MonoBehaviour
{
    public HideShowBointQuest quests;

    public GameObject area2;
    public GameObject area3;
    public GameObject area4;
    public GameObject area5;

    private void Start()
    {
        if (gameObject.name != "boatPointCube")
        {
            gameObject.SetActive(false);
        }


    }
    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.tag == "BoatPointArea")
        {
            quests.counter += 1;
            Destroy(gameObject);

            switch (quests.counter)
            {
                case 1:
                    area2.SetActive(true);
                    break;
                case 2:
                    area3.SetActive(true);
                    break;
                case 3:
                    area4.SetActive(true);
                    break;
                case 4:
                    area5.SetActive(true);
                    break;



            }
        }
    }
}


