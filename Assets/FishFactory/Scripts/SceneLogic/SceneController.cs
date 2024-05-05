using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [Header("Scene Controller")]
    [Tooltip("The name of the scene to change to")]
    [SerializeField]
    private string sceneName;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// When the player collides with the scene controller, change the scene
    /// </summary>
    /// <param name="collisionObject">The player collider</param>
    private void OnTriggerEnter(Collider collisionObject)
    {
        if (collisionObject.gameObject.name == "Grabber")
        {
            ChangeScene(sceneName);
        }
    }

    /// <summary>
    /// Change the scene. If the player is currently in the HSE room the requirements has to be fulfilled first.
    /// </summary>
    /// <param name="scene">The name of the scene to change to</param>
    private void ChangeScene(string scene)
    {
        // Check if the player is in the HSE room and if the requirements are fulfilled. Player should only be able to progress after the HSE room is completed.
        if (
            !GameManager.Instance.HSERoomCompleted
            && SceneManager.GetActiveScene().name == "HSERoom"
        )
        {
            GameManager.Instance.PlaySound("incorrect");
            return;
        }

        // Set the target location (door) the player should be placed by after the scene change
        SetPlayerTargetLocation(scene);

        // Adds a delegate to trigger when the scene is loaded
        // The delegate will move the player to the correct location
        SceneManager.sceneLoaded += OnSceneLoaded;

        GameManager.Instance.PlaySound("door");
        SceneManager.LoadScene(scene);
    }

    /// <summary>
    /// Set the target location the player should be placed by after the scene change
    /// </summary>
    /// <param name="scene">The name of the scene to change to</param>
    private void SetPlayerTargetLocation(string scene)
    {
        // get scene name and destination scene
        string targetSpawnLocation = SceneManager.GetActiveScene().name + "To" + scene;

        // switch (targetSpawnLocation)
        // {
        //     case "HSERoomToBleedingStation":
        //         GameManager.Instance.NewPlayerPosition = new Vector3(
        //             4.88999987f,
        //             1.06599998f,
        //             12.4700003f
        //         );
        //         break;
        //     case "HSERoomToQAStation":
        //         GameManager.Instance.NewPlayerPosition = new Vector3(
        //             -10.191f,
        //             1.06599998f,
        //             -10.0489998f
        //         );
        //         break;
        //     case "BleedingStationToHSERoom":
        //         GameManager.Instance.NewPlayerPosition = new Vector3(
        //             0.829999983f,
        //             1.00600004f,
        //             -5.25f
        //         );
        //         break;
        //     case "BleedingStationToQAStation":
        //         GameManager.Instance.NewPlayerPosition = new Vector3(
        //             -8.76399994f,
        //             1.06599998f,
        //             -20.8099995f
        //         );
        //         break;
        //     case "QAStationToHSERoom":
        //         GameManager.Instance.NewPlayerPosition = new Vector3(
        //             -4.0079999f,
        //             1.06599998f,
        //             -3.95799994f
        //         );
        //         break;
        //     case "QAStationToBleedingStation":
        //         GameManager.Instance.NewPlayerPosition = new Vector3(
        //             -13.7299995f,
        //             1.06599998f,
        //             12.8199997f
        //         );
        //         break;
        // }

        // Debug.Log(GameManager.Instance.NewPlayerPosition);

        SceneManager.sceneLoaded += OnSceneLoaded;
        // Debug.Log(SceneManager.sceneLoaded.GetInvocationList());

        if (
            !GameManager.Instance.HSERoomCompleted
            && SceneManager.GetActiveScene().name == "HSERoom"
        )
        {
            GameManager.Instance.PlaySound("incorrect");
            return;
        }

        GameManager.Instance.PlaySound("door");
        SceneManager.LoadScene(scene);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.Find("XR Rig Advanced VR4VET");
        player.transform.position = GameManager.Instance.NextScenePlayerPosition;
        player.transform.eulerAngles = GameManager.Instance.NextScenePlayerRotation;

        // Reset the new player position
        GameManager.Instance.NextScenePlayerPosition = new Vector3(0, 0, 0);
        GameManager.Instance.NextScenePlayerRotation = new Vector3(0, 0, 0);

        // Remove the delegate to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
