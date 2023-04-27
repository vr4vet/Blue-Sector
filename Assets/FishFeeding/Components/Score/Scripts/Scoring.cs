using System.Collections;
using System.Collections.Generic;
using TMPro;
using BNG;
using UnityEngine;
using UnityEngine.UI;

public class Scoring : MonoBehaviour
{
    private int score = 0;
    public int Score => score;
    private GameObject[] merds;
    private int deadFish;
    public int DeadFish => deadFish;
    private float foodWasted;
    public float FoodWasted => foodWasted;
    private float foodWastedPercentage;
    public float FoodWastedPercentage => foodWastedPercentage;
    private MerdCameraController merdCameraController;
    [SerializeField]
    private GameObject MerdCameraHost;


    // Start is called before the first frame update
    void Start()
    {
        merds = GameObject.FindGameObjectsWithTag("Fish System");
        merdCameraController = MerdCameraHost.GetComponent<MerdCameraController>();
    }

    public void StartScoring()
    {
        score = 0;
        InvokeRepeating(nameof(UpdateScore), 1.0f, 1.0f);
        Debug.Log("Score: " + score);
    }

    public void StopScoring()
    {
        CancelInvoke("UpdateScore");
    }

    /* Goes through every merd and checks if it is full, hungry or dead. Based on the status to the merd 
     * add to the total score of the game. */
    void UpdateScore()
    {
        foreach (GameObject i in merds)
        {
            FishSystemScript script = i.GetComponent<FishSystemScript>();
            if (script.state == FishSystemScript.FishState.Full)
            {
                if (script.feedingIntensity == FishSystemScript.FeedingIntensity.Low)
                {
                    score += 10;
                }
                else if (script.feedingIntensity == FishSystemScript.FeedingIntensity.Medium)
                {
                    score += 5;
                }
                Debug.Log("State Full, score: " + score);
            }
            else if (script.state == FishSystemScript.FishState.Hungry)
            {
                score += 2;
                Debug.Log("State Hungry, score: " + score);
            }

            Debug.Log("FishSystemScript status: " + script.state);
            Debug.Log("Score: " + score);
        }

        UpdateFoodWasteAndDeadFish();
    }

    /* Updates the food waste, food waste percentage and the amount of dead fish depending on which merd is being shown on the screen. */
    void UpdateFoodWasteAndDeadFish()
    {
        foreach (GameObject i in merds)
        {
            FishSystemScript script = i.GetComponent<FishSystemScript>();
            if (merdCameraController.SelectedFishSystem == script)
            {
                foodWasted = script.foodWasted;
                foodWastedPercentage = script.foodWasted / (script.foodBase * 5 / 3);
                deadFish = script.fishKilled;
            }
        }
    }

}
