using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Task;
using UnityEngine;

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
    //private Dictionary<string, float> scoreCount = new();
    private float steerCameraCorrect;
    private float adjustFeedingCorrect;
    private float didNotCheckMerdCount;
    private float totalChecks;


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

        steerCameraCorrect = 0;
        adjustFeedingCorrect = 0;
        didNotCheckMerdCount = 0;
        totalChecks = 0;

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

        /*scoreCount = new Dictionary<string, float>(){
            {"steerCameraCorrect", 0}, {"adjustFeedingCorrect", 0}, {"didNotCheckMerdCount", 0}, {"totalChecks", 0}};*/
            //{"merd1SelectedTime", 0}, {"merd2SelectedTime", 0}, {"merd3SelectedTime", 0}};
        
        foreach (Subtask subtask in fishFeedTask.Subtasks) {
            subtask.Points = 0;
        }

        foreach (Subtask subTask in fishFeedTask.Subtasks) {
             subTask.SetCompleated(false);
        }
        fishFeedTask.Compleated(false);
        /*Debug.Log("Steer camera: " + fishFeedTask.GetSubtask("Steer the camera").Points);
        Debug.Log("Adjust feeding intensity: " + fishFeedTask.GetSubtask("Adjust feeding intensity").Points);
        Debug.Log("Switch to another cage: " + fishFeedTask.GetSubtask("Switch to another cage").Points);

        Debug.Log("Accuracy: " + taskHolder.GetSkill("Accuracy").GetArchivedPoints());
        Debug.Log("Camera adjustment: " + taskHolder.GetSkill("Camera adjustment").GetArchivedPoints());
        Debug.Log("Overview of multiple cages: " + taskHolder.GetSkill("Overview of multiple cages").GetArchivedPoints());*/
    }

    // Update is called once per frame
    void Update() {

    }

    /*public void StartScoring() {
        *//*score = 0;*//*
        InvokeRepeating(nameof(UpdateScore), 1.0f, 1.0f);
        *//*Debug.Log("Score: " + score);*//*
    }

    public void StopScoring() {
        CancelInvoke("UpdateScore");
    }*/

    public void GiveFinalTabletScore() {
        /*foreach (KeyValuePair<string, float> ele in scoreCount) {
            Debug.Log(ele.Key + ": " + ele.Value);
        }*/

        float steerCameraScore = 0;
        float adjustFeedingScore = 0;
        float checkMerdScore = 0;

        /*if (scoreCount["totalChecks"] != 0) {
            steerCameraScore = scoreCount["steerCameraCorrect"] / scoreCount["totalChecks"];
            adjustFeedingScore = scoreCount["adjustFeedingCorrect"] / scoreCount["totalChecks"];
            checkMerdScore = (scoreCount["totalChecks"] - scoreCount["didNotCheckMerdCount"]) / scoreCount["totalChecks"];
        }*/
        if (totalChecks != 0) {
            steerCameraScore = steerCameraCorrect / totalChecks;
            adjustFeedingScore = adjustFeedingCorrect / totalChecks;
            checkMerdScore = (totalChecks - didNotCheckMerdCount) / totalChecks;
        }

        Debug.Log("steerScore: " + steerCameraScore);
        Debug.Log("adjustFeedingScorew: " + adjustFeedingScore);
        Debug.Log("checkMerdScore: " + checkMerdScore);

        fishFeedTask.GetSubtask("Steer the camera").AddPoints((int)Math.Round(steerCameraScore * 100));
        fishFeedTask.GetSubtask("Adjust feeding intensity").AddPoints((int)Math.Round(adjustFeedingScore * 100));
        fishFeedTask.GetSubtask("Switch to another cage").AddPoints((int)Math.Round(checkMerdScore * 100));

        fishFeedTask.Compleated(true);
        Debug.Log("task completed: " + fishFeedTask.Compleated());


        Debug.Log("Steer camera: " + fishFeedTask.GetSubtask("Steer the camera").Points);
        Debug.Log("Adjust feeding intensity: " + fishFeedTask.GetSubtask("Adjust feeding intensity").Points);
        Debug.Log("Switch to another cage: " + fishFeedTask.GetSubtask("Switch to another cage").Points);

        Debug.Log("Accuracy: " + taskHolder.GetSkill("Accuracy").GetArchivedPoints());
        Debug.Log("Camera adjustment: " + taskHolder.GetSkill("Camera adjustment").GetArchivedPoints());
        Debug.Log("Overview of multiple cages: " + taskHolder.GetSkill("Overview of multiple cages").GetArchivedPoints());

        /*foreach (string key in scoreCount.Keys.ToList()) {
            scoreCount[key] = 0;
        }*/
        steerCameraCorrect = 0;
        adjustFeedingCorrect = 0;
        didNotCheckMerdCount = 0;
        totalChecks = 0;

        foreach (Subtask subtask in fishFeedTask.Subtasks) {
            subtask.Points = 0;
        }

    }

    /// <summary>
    /// Updates the score based on the fish cage's current camera position, feeding intensity and if the cage is checked.
    /// </summary>
    /// <param name="fishSystemScript">The fishsystemscript for a fish cage.</param>
    private void UpdateScore(FishSystemScript fishSystemScript) {
        if (IsCameraPositionCorrect(fishSystemScript)) {
            steerCameraCorrect += 1;
        }

        float feedingIntensityScore = GiveFeedingIntensityScore(fishSystemScript);
        adjustFeedingCorrect += feedingIntensityScore;

        if (fishSystemScript != selectedFishSystem && feedingIntensityScore == 0) {
            didNotCheckMerdCount += 1;
        }

        totalChecks += 1;

    }

    /// <summary>
    /// Checks if the camera is facing outwards of the fish cage and at the outer edge of the cage, or if the camera is at the start 
    /// position.
    /// </summary>
    private bool IsCameraPositionCorrect(FishSystemScript fishSystemScript) {
        Camera camera = cameras[fishSystemScript];
        Vector3 zeroVector = new Vector3(0, 0, 0); // start position of the cameras
        Vector3 targetDir = zeroVector - camera.transform.localPosition;
        float sqrLen = targetDir.sqrMagnitude;
        //float angle = sqrLen;
        //Debug.Log("fishsystem" + fishSystemScript.merdNr + " BallAngle: " + Vector3.Angle(camera.transform.forward, sphere.gameObject.transform.localPosition - camera.transform.localPosition));
        Debug.Log("fishsystem" + fishSystemScript.merdNr + " ZeroVectorAngle: " + Vector3.Angle(camera.transform.forward, targetDir));
        //Debug.Log("fishsystem"+ fishSystemScript.merdNr + " Angle: " + Vector3.Angle(camera.transform.forward, fishSystemScript.gameObject.transform.localPosition - camera.transform.localPosition));
        Debug.Log("Distance: " + sqrLen);
        if ((Vector3.Angle(camera.transform.forward, targetDir) > 90 && sqrLen > 7) || camera.transform.localPosition == zeroVector) {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Gives the score for feeding intensity based on the status of the cage and the current feeding intensity.
    /// </summary>
    /// <returns> 1 point if intensity is correct, 0.5 if it's partially correct and 0 otherwise.</returns>
    private float GiveFeedingIntensityScore(FishSystemScript fishSystemScript) {
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

        UpdateScore(selectedFishSystem);

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
