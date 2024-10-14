using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{
    // ----------------- Editor Fields -----------------

    [Header("Scene Controller")]
    [Tooltip("The name of the scene to change to")]
    [SerializeField]
    private string sceneName;
    public string SceneName
    {
        set { sceneName = value; }
        get { return sceneName; }
    }

    [Tooltip("Whether the trigger is a door and should play a door sound")]
    [SerializeField]
    private MeansOfTransportation meansOfTransportation = MeansOfTransportation.Door;
    private enum MeansOfTransportation
    {
        Door,
        Boat
    }

    public UnityEvent OnChangeScene;

    // ----------------- Unity Functions -----------------

    /// <summary>
    /// When the player collides with the scene controller, change the scene
    /// </summary>
    /// <param name="collisionObject">The player collider</param>
    private void OnTriggerEnter(Collider collisionObject)
    {
        if (sceneName != null)
        {
            if (
                meansOfTransportation == MeansOfTransportation.Door && collisionObject.gameObject.name == "Grabber" 
                || 
                meansOfTransportation == MeansOfTransportation.Boat && collisionObject.gameObject.CompareTag("Player")
                )
            {
                OnChangeScene.Invoke();
                ChangeScene(sceneName);
                Debug.Log("Player entered");
            }
        }
    }

    // ----------------- Private Functions -----------------

    /// <summary>
    /// Change the scene. If the player is currently in the HSE room the requirements has to be fulfilled first.
    /// </summary>
    /// <param name="scene">The name of the scene to change to</param>
    private void ChangeScene(string scene)
    {
        /*        if (scene == "FishFeeding")
                {
                    GameManager.Instance.PlaySound("door");
                    SceneManager.LoadScene(scene);
                    return;
                }*/

        // Check if the player is in the HSE room and if the requirements are fulfilled. Player should only be able to progress after the HSE room is completed or if going to a non-factory scene.
        if (
            !GameManager.Instance.HSERoomCompleted
            && SceneManager.GetActiveScene().name == "HSERoom"
            && IsFactoryScene(scene)
        )
        {
            GameManager.Instance.PlaySound("incorrect");
            return;
        }

        // Set the target location (exit door) the player should be placed by after the scene change
        SetPlayerTargetLocation(scene);

        // Adds a delegate to trigger when the scene is loaded
        // The delegate will move the player to the correct location
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Play the appropriate transportation sound
        if (meansOfTransportation == MeansOfTransportation.Door)
            GameManager.Instance.PlaySound("door");
        if (meansOfTransportation == MeansOfTransportation.Boat)
            GameManager.Instance.PlaySound("MotorBoatDriving");

        SceneManager.LoadScene(scene);
    }

    /// <summary>
    /// Set the target location the player should be placed by after the scene change
    /// </summary>
    /// <param name="scene">The name of the scene to change to</param>
    private void SetPlayerTargetLocation(string scene)
    {
        // Get scene name and destination scene in the format "CurrentSceneNameToDestinationScene"
        string targetSpawnLocation = SceneManager.GetActiveScene().name + "To" + scene;
        switch (targetSpawnLocation)
        {
            case "FishFeedingToHSERoom":
                GameManager.Instance.NextScenePlayerPosition = new Vector3(
                    -4.114f,
                    1.014f,//0.5f,
                    -12.793f
                );
                GameManager.Instance.NextScenePlayerRotation = new Vector3(0f, -270f, 0f);
                break;
            case "FishFeedingToReceptionOutdoor":
                GameManager.Instance.NextScenePlayerPosition = new Vector3(
                    5.6f,
                    1.03299999f,//0.5f,
                    78.5f
                );
                GameManager.Instance.NextScenePlayerRotation = new Vector3(0f, 0f, 0f);
                break;
            case "ReceptionOutdoorToHSERoom":
                GameManager.Instance.NextScenePlayerPosition = new Vector3(
                    -4.114f,
                    1.014f,//0.5f,
                    -12.793f
                );
                GameManager.Instance.NextScenePlayerRotation = new Vector3(0f, -270f, 0f);
                break;
            case "ReceptionOutdoorToLaboratory":
                GameManager.Instance.NextScenePlayerPosition = new Vector3(
                    2.212f,
                    1.025f,//0.5f,
                    -0.225f
                );
                GameManager.Instance.NextScenePlayerRotation = new Vector3(0f, -47.564f, 0f);
                break;
            case "HSERoomToReceptionOutdoor":
                GameManager.Instance.NextScenePlayerPosition = new Vector3(
                    5.6f,
                    1.03299999f,//0.5f,
                    78.5f
                );
                GameManager.Instance.NextScenePlayerRotation = new Vector3(0f, 0f, 0f);
                break;
            case "HSERoomToBleedingStation":
                GameManager.Instance.NextScenePlayerPosition = new Vector3(
                    11.1490002f,
                    1.03299999f,
                    -6.76000023f
                );
                GameManager.Instance.NextScenePlayerRotation = new Vector3(0f, 270f, 0f);
                break;
            case "HSERoomToQAStation":
                GameManager.Instance.NextScenePlayerPosition = new Vector3(
                    -10.191f,
                    1.06599998f,
                    -10.0489998f
                );
                GameManager.Instance.NextScenePlayerRotation = new Vector3(0f, 90f, 0f);
                break;
            case "BleedingStationToHSERoom":
                GameManager.Instance.NextScenePlayerPosition = new Vector3(
                    0.907000005f,
                    1.01400006f,
                    -5.0000000f
                );
                GameManager.Instance.NextScenePlayerRotation = new Vector3(0f, 270f, 0f);
                break;
            case "BleedingStationToQAStation":
                GameManager.Instance.NextScenePlayerPosition = new Vector3(
                    -8.9989996f,
                    1.06599998f,
                    -20.7859993f
                );
                GameManager.Instance.NextScenePlayerRotation = new Vector3(0f, 0f, 0f);
                break;
            case "QAStationToHSERoom":
                GameManager.Instance.NextScenePlayerPosition = new Vector3(
                    -4.11399984f,
                    1.01400006f,
                    -3.93300009f
                );
                GameManager.Instance.NextScenePlayerRotation = new Vector3(0f, 0f, 0f);
                break;
            case "QAStationToBleedingStation":
                GameManager.Instance.NextScenePlayerPosition = new Vector3(
                    -7.92999983f,
                    1.03299999f,
                    -6.76000023f
                );
                GameManager.Instance.NextScenePlayerRotation = new Vector3(0f, 90f, 0f);
                break;
        }
    }

    /// <summary>
    /// Move the player to the correct location after the scene has been loaded
    /// </summary>
    /// <param name="scene"> The scene that has been loaded</param>
    /// <param name="mode"> The mode the scene has been loaded in</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // The player should be placed at the initial spawn point if the new player position is not set. This is to prevent the player from spawning at 0,0,0.
        if (GameManager.Instance.NextScenePlayerPosition == new Vector3(0, 0, 0))
        {
            Debug.LogError("Location was not set correctly. Spawning player at initial location.");
            SceneManager.sceneLoaded -= OnSceneLoaded;
            return;
        }

        // Moving the player will not work if the XR Rig prefab does not have the Player tag!
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = GameManager.Instance.NextScenePlayerPosition;
        player.transform.eulerAngles = GameManager.Instance.NextScenePlayerRotation;

        // Reset the new player position
        GameManager.Instance.NextScenePlayerPosition = new Vector3(0, 0, 0);
        GameManager.Instance.NextScenePlayerRotation = new Vector3(0, 0, 0);

        GameManager.Instance.IsTaskOn = false;
        GameManager.Instance.IsSecondaryTaskOn = false;

/*        // Tell GameManager if the current scene is a factory scene
        GameManager.Instance.IsFactoryScene = IsFactoryScene(scene.name);*/

        // Load gloves if one of the factory scenes
        if (IsFactoryScene(scene.name))
            GameManager.Instance.LoadGlovesMaterials();

        // Remove the delegate to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private bool IsFactoryScene(string scene) { return scene == "BleedingStation" || scene == "HSERoom" || scene == "QAStation"; }
}
