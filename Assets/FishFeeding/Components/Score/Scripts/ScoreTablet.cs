using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Task;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreTablet : MonoBehaviour
{
    private TaskHolder taskHolder;
    private Task.Task fishFeedTask;
    private Game game;
    private GameObject[] merds;
    private MerdCameraController cameraController;
    //[SerializeField]
    private FishSystemScript selectedFishSystem;
    private Dictionary<FishSystemScript, FishSystemScript.FeedingIntensity> prevFeedingIntensities = new();
    private Dictionary<FishSystemScript, Camera> cameras = new();
    private FishSystemScript.FeedingIntensity prevFeedingIntensity;
    private Dictionary<string, float> scoreCount = new();
    private int totalFoodwaste;
    private float totalHungryTime;


    // Start is called before the first frame update
    void Start() {
        taskHolder = GetComponent<TaskHolder>();
        fishFeedTask = taskHolder.GetTask("Feed the fish");

        game = FindObjectOfType<Game>();

        merds = GameObject.FindGameObjectsWithTag("Fish System");

        cameraController = FindObjectOfType<MerdCameraController>();
        cameraController.SelectedFishSystemChanged.AddListener(FishSystemChanged);

        selectedFishSystem = cameraController.SelectedFishSystem;

        prevFeedingIntensity = selectedFishSystem.feedingIntensity;

        totalFoodwaste = 0;
        totalHungryTime = 0;

        foreach (GameObject merd in merds) {
            FishSystemScript fishSystemScript = merd.GetComponent<FishSystemScript>();
            prevFeedingIntensities.Add(fishSystemScript, fishSystemScript.feedingIntensity);
            fishSystemScript.FishStateChanged.AddListener(UpdateScore);
            cameras.Add(fishSystemScript, fishSystemScript.GetComponentInChildren<Camera>());
        }
        /*foreach (KeyValuePair<FishSystemScript, FishSystemScript.FeedingIntensity> ele in prevFeedingIntensities) {
            Debug.Log(ele.Key);
            Debug.Log(ele.Value);
        }*/

        scoreCount = new Dictionary<string, float>(){
            {"steerCameraCorrect", 0}, {"steerCameraCount", 0},
            {"adjustFeedingCorrect", 0}, {"adjustFeedingCount", 0},
            {"didNotCheckMerdCount", 0}, {"checkMerdCount", 0} };
            //{"merd1SelectedTime", 0}, {"merd2SelectedTime", 0}, {"merd3SelectedTime", 0}};

        foreach (Subtask subTask in fishFeedTask.Subtasks) {
             subTask.SetCompleated(false);
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void StartScoring() {
        /*score = 0;*/
        InvokeRepeating(nameof(UpdateScore), 1.0f, 1.0f);
        /*Debug.Log("Score: " + score);*/
    }

    public void StopScoring() {
        CancelInvoke("UpdateScore");
    }

    public void GiveFinalTabletScore() {
        fishFeedTask.GetSubtask("Steer the camera").Points = 0;
        fishFeedTask.GetSubtask("Adjust feeding intensity").Points = 0;
        fishFeedTask.GetSubtask("Switch to another cage").Points = 0;
        Debug.Log("Steer camera: " + fishFeedTask.GetSubtask("Steer the camera").Points);
        Debug.Log("Adjust feeding intensity: " + fishFeedTask.GetSubtask("Adjust feeding intensity").Points);
        Debug.Log("Switch to another cage: " + fishFeedTask.GetSubtask("Switch to another cage").Points);
        foreach (KeyValuePair<string, float> ele in scoreCount) {
            Debug.Log(ele.Key + ": " + ele.Value);
        }
        float steerCameraScore = 0;
        if (scoreCount["steerCameraCount"] != 0) {
            steerCameraScore = (scoreCount["steerCameraCorrect"] / scoreCount["steerCameraCount"]) / 3;
        }
        Debug.Log("steerScore: " + steerCameraScore);
        fishFeedTask.GetSubtask("Steer the camera").AddPoints((int)Math.Round(steerCameraScore * 100));

        float adjustFeedingScore = 0;
        if (scoreCount["adjustFeedingCount"] != 0) {
            adjustFeedingScore = (scoreCount["adjustFeedingCorrect"] / scoreCount["adjustFeedingCount"]) / 3;
        }
        Debug.Log("adjustFeedingScorew: " + adjustFeedingScore);
        fishFeedTask.GetSubtask("Adjust feeding intensity").AddPoints((int)Math.Round(adjustFeedingScore * 100));

        float checkMerdScore = 0;
        if (scoreCount["checkMerdCount"] != 0) {
            checkMerdScore = ((scoreCount["checkMerdCount"] - scoreCount["didNotCheckMerdCount"]) / scoreCount["checkMerdCount"]) / 3;
        }
        Debug.Log("checkMerdScore: " + checkMerdScore);
        fishFeedTask.GetSubtask("Switch to another cage").AddPoints(33);

        Debug.Log("check" + Math.Round(0f * 100));

        Debug.Log("Steer camera: " + fishFeedTask.GetSubtask("Steer the camera").Points);
        Debug.Log("Adjust feeding intensity: " + fishFeedTask.GetSubtask("Adjust feeding intensity").Points);
        Debug.Log("Switch to another cage: " + fishFeedTask.GetSubtask("Switch to another cage").Points);

        foreach (string key in scoreCount.Keys.ToList()) {
            scoreCount[key] = 0;
        }

    }

    private void UpdateScore(FishSystemScript fishSystemScript) {
        // Checks each time a state of a fish cage changes
        // Will then check the position of the camera - either correct or not and saves this value
        // Is the feeding intensity correct for the situation? - 
        // Is the merd currently selected?
        // If it is selected but the feeding intensity is wrong then the merd is not checked, get minus for overview or checking cages
        if (IsCameraPositionCorrect()) {
            scoreCount["steerCameraCorrect"] += 1;
        }
        scoreCount["steerCameraCount"] += 1;

        float feedingIntensityScore = IsFeedingIntensityCorrect(fishSystemScript);
        scoreCount["adjustFeedingCorrect"] += feedingIntensityScore;
        scoreCount["adjustFeedingCount"] += 1;

        if (fishSystemScript != selectedFishSystem && feedingIntensityScore == 0) {
            scoreCount["didNotCheckMerdCount"] += 1;
        }
        scoreCount["checkMerdCount"] += 1;


    }

    private bool IsCameraPositionCorrect() {
        return true;
    }

    private float IsFeedingIntensityCorrect(FishSystemScript fishSystemScript) {
        if ((fishSystemScript.state == FishSystemScript.FishState.Full && 
            fishSystemScript.feedingIntensity == FishSystemScript.FeedingIntensity.Low) ||
            (fishSystemScript.state == FishSystemScript.FishState.Hungry &&
            fishSystemScript.feedingIntensity == FishSystemScript.FeedingIntensity.High) ||
            (fishSystemScript.state == FishSystemScript.FishState.Dying &&
            fishSystemScript.feedingIntensity == FishSystemScript.FeedingIntensity.High)) {
            return 1;
        } else if (fishSystemScript.state == FishSystemScript.FishState.Full &&
            fishSystemScript.feedingIntensity == FishSystemScript.FeedingIntensity.Medium) {
            return 0.5f;
        } else {
            return 0;
        }
    }



    private void FishSystemChanged(FishSystemScript fishSystemScript) {
        if (!game.startGame) {
            return;
        }
        selectedFishSystem = fishSystemScript;
        Subtask switchSubTask = fishFeedTask.GetSubtask("Switch to another cage");
        switchSubTask.SetCompleated(true);
        Debug.Log("firstsubtask completed");

        foreach (Subtask subTask in fishFeedTask.Subtasks) {
            if (subTask != switchSubTask)
                subTask.SetCompleated(false);
        }

    }

    public void JoystickGrabbed() {
        if (!game.startGame) {
            return;
        }
        fishFeedTask.GetSubtask("Steer the camera").SetCompleated(true);
    }

    public void FeedingIntensityChanged() {
        if (!game.startGame) {
            return;
        }
        if (prevFeedingIntensities[selectedFishSystem] != selectedFishSystem.feedingIntensity) {
            Debug.Log(prevFeedingIntensities[selectedFishSystem]);
            Debug.Log(selectedFishSystem.feedingIntensity);
            fishFeedTask.GetSubtask("Adjust feeding intensity").SetCompleated(true);
            Debug.Log("secondsubtask completed");
        }
        foreach (FishSystemScript fishSystemScript in prevFeedingIntensities.Keys.ToList()) {
            prevFeedingIntensities[fishSystemScript] = fishSystemScript.feedingIntensity;
        }
    }

}
