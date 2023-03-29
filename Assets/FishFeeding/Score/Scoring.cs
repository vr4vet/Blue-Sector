using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scoring : MonoBehaviour
{
    private int score = 0;
    private GameObject[] merds;
    private bool startGame = false;
    [SerializeField]
    private int time = 60;
    private float gameTimeLeft;
    private int deadFish;
    private float foodWasted;
    private float foodWastedPercentage;
    private TextMeshProUGUI endScoreText;
    private TextMeshProUGUI timeLeft;
    private TextMeshProUGUI currentScore;
    private TextMeshProUGUI deadFishText;
    private TextMeshProUGUI foodWasteText;
    private Slider foodWasteSlider;


    // Start is called before the first frame update
    void Start()
    {
        merds = GameObject.FindGameObjectsWithTag("Fish System");
        endScoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<TextMeshProUGUI>();
        GameObject canvas = GameObject.FindGameObjectWithTag("MonitorMerd").transform.GetChild(1).gameObject;
        timeLeft = canvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        currentScore = canvas.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        deadFishText = canvas.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        foodWasteText = canvas.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        foodWasteSlider = canvas.transform.GetChild(4).gameObject.GetComponent<Slider>();
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
            InvokeRepeating(nameof(UpdateScore), 1.0f, 1.0f);
            StartCoroutine(GiveScore());
        }
        if (startGame)
        {
            UpdateScreenStats();
        }
    }

     /* Gives the score after a certain amount of time. */
    IEnumerator GiveScore()
    {
        for (gameTimeLeft = time; gameTimeLeft > 0; gameTimeLeft -= Time.deltaTime)
        {
            yield return null;
        }
        CancelInvoke("UpdateScore");
        startGame = false;
        endScoreText.text = "YOUR SCORE:\n" + score;
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

            float wastedFoodPoints = script.foodWasted / 10;
            score -= (int)(wastedFoodPoints + 0.5f); // Rounds wastedFoodPoints to the nearest int.
            Debug.Log("Score after food waste: " + score);

            foodWasted = script.foodWasted;
            foodWastedPercentage = script.foodWasted / (script.foodBase * 5 / 3);

        }
        Debug.Log("Time left: " + gameTimeLeft);
    }

    /* Updates the timer, score, food waste and the amount of dead fish on the merd screen. */
    void UpdateScreenStats()
    {
        timeLeft.text = "Time left: " + Mathf.FloorToInt(gameTimeLeft / 60) + ":" +
            Mathf.FloorToInt(gameTimeLeft % 60).ToString("00");
        currentScore.text = "Score: " + score;
        foodWasteText.text = "Food wastage: " + foodWasted + " / Sec.";
        foodWasteSlider.value = foodWastedPercentage;
        deadFishText.text = "Dead fish: " + deadFish;
    }

}
