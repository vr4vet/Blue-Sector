using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoring : MonoBehaviour
{
    private int score = 0;
    private GameObject[] fishes;
    private GameObject[] merds;
    private bool startGame = false;
    [SerializeField]
    private float time = 60.0f;

    // Start is called before the first frame update
    void Start()
    {
        fishes = GameObject.FindGameObjectsWithTag("Fish");
        merds = GameObject.FindGameObjectsWithTag("Fish System");
    }

    /* Update is called once per frame. If the key 's' is pressed and the game hasn't started, start the game and
     * the coroutine GiveScore and update the score every second. */
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && !startGame)
        {
            score = 0;
            startGame = true;
            Debug.Log("Started the game");
            Debug.Log("Score: " + score);
            StartCoroutine(GiveScore());
            InvokeRepeating(nameof(UpdateScore), 1.0f, 1.0f);
        }
    }

     /* Gives the score after a certain amount of time. */
    IEnumerator GiveScore()
    {
        yield return new WaitForSeconds(time);
        CancelInvoke("UpdateScore");
        startGame = false;
        Debug.Log("End of game");
        Debug.Log("Score: " + score);
    }

    /* Goes through every fish and checks if it is full, hungry or dead. Based on the status to the fish add or 
     * subtract from the total score of the game. */
    void UpdateScore()
    {
        foreach (GameObject i in fishes)
        {
            FishScript script = i.GetComponent<FishScript>();
            if (script.status == FishScript.Status.Full)
            {
                score += 1;
            } else if (script.status == FishScript.Status.Hungry)
            {
                score -= 1;
            } else
            {
                score -= 2;
            }

        }

        foreach (GameObject i in merds)
        {
            Debug.Log("Score before food waste: " + score);
            FishSystemScript script = i.GetComponent<FishSystemScript>();
            float wastedFoodPercentage = script.foodWasted / 10;
            Debug.Log("Wasted food: " + script.foodWasted + "wastedFoodPercentage: " + wastedFoodPercentage);
            score -= (int)(wastedFoodPercentage + 0.5f); // Rounds wastedFoodPercentage to the nearest int.
            script.foodWasted = 0;
            Debug.Log("Score after food waste: " + score);
        }
    }

}
