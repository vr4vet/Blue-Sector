using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scoring : MonoBehaviour
{
    private int score = 0;
    private GameObject[] fishes;
    private GameObject[] merds;
    private bool startGame = false;
    [SerializeField]
    private float time = 60.0f;
    private TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        fishes = GameObject.FindGameObjectsWithTag("Fish");
        merds = GameObject.FindGameObjectsWithTag("Fish System");
        scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<TextMeshProUGUI>();
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
        scoreText.text = "YOUR SCORE:\n" + score;
        Debug.Log("End of game");
        Debug.Log("Score: " + score);
    }

    /* Goes through every merd and checks if it is full, hungry or dead. Based on the status to the merd 
     * add or subtract from the total score of the game. */
    void UpdateScore()
    {
        foreach (GameObject i in merds)
        {
            FishSystemScript script = i.GetComponent<FishSystemScript>();
            if (script.state == FishSystemScript.FishState.Full)
            {
                score += 1 * script.amountOfFish;
            }
            else if (script.state == FishSystemScript.FishState.Hungry)
            {
                score -= (int)(0.5 * script.amountOfFish);
            }
            else
            {
                score -= 1 * script.fishKilled;
            }

            /*Debug.Log("Score before food waste: " + score);*/
            float wastedFoodPoints = script.foodWasted / 10;
            score -= (int)(wastedFoodPoints + 0.5f); // Rounds wastedFoodPoints to the nearest int.
            /*script.foodWasted = 0;*/
            Debug.Log("Score after food waste: " + score);
        }
    }

}
