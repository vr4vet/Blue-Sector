using BNG;
using UnityEngine;

public class TransitionSceneNPC : MonoBehaviour
{
    [SerializeField] private AudioSource motorSound;
    [SerializeField] private SceneLoader sceneLoader;

    // Start is called before the first frame update
    void Start()
    {
        ButtonSpawner.OnAnswer += ButtonSpawner_OnAnswer;
        sceneLoader.ScreenFadeTime = 4f;
    }

    private void ButtonSpawner_OnAnswer(string answer)
    {
        if (answer == "Welfare station")
        {
            ChangeScene("FishWelfare");
        } else if (answer == "Fish factory")
        {
            ChangeScene("HSERoom");
        }
    }

    private void ChangeScene(string sceneName)
    {
        motorSound.Play();
        sceneLoader.LoadScene(sceneName);
    }

    private void OnDestroy()
    {
        ButtonSpawner.OnAnswer -= ButtonSpawner_OnAnswer;
    }
}