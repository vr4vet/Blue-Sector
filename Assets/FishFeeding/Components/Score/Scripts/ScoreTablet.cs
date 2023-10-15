using System.Collections;
using System.Collections.Generic;
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
    [SerializeField]
    private FishSystemScript selectedFishSystem;
    private Dictionary<FishSystemScript, FishSystemScript.FeedingIntensity> prevFeedingIntensities = new(); 
    private FishSystemScript.FeedingIntensity prevFeedingIntensity;
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

        prevFeedingIntensity = selectedFishSystem.feedingIntensity;

        totalFoodwaste = 0;
        totalHungryTime = 0;

        foreach (GameObject merd in merds) {
            FishSystemScript fishSystemScript = merd.GetComponent<FishSystemScript>();
            prevFeedingIntensities.Add(fishSystemScript, fishSystemScript.feedingIntensity);
        }

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

    void UpdateScore() {


    }

    private void FishSystemChanged(FishSystemScript fishSystemScript) {
        if (!game.startGame) {
            return;
        }
        selectedFishSystem = fishSystemScript;
        Subtask firstSubTask = fishFeedTask.GetSubtask("Switch into cage");
        firstSubTask.SetCompleated(true);
        Debug.Log("firstsubtask completed");

        foreach (Subtask subTask in fishFeedTask.Subtasks) {
            if (subTask != firstSubTask)
                subTask.SetCompleated(false);
        }

    }

    public void JoystickGrabbed() {
        if (!game.startGame) {
            return;
        }
        fishFeedTask.GetSubtask("Steer the camera").SetCompleated(true);
        fishFeedTask.GetSubtask("Observe the situation").SetCompleated(true);
    }

    public void FeedingIntensityChanged() {
        if (!game.startGame) {
            return;
        }
        if (prevFeedingIntensities[selectedFishSystem] != selectedFishSystem.feedingIntensity) {
            fishFeedTask.GetSubtask("Adjust feeding intensity").SetCompleated(true);
        }
    }

}
