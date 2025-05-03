using BNG;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class BreakTask : MonoBehaviour
{
    [HideInInspector] private NPCSpawner _npcSpawner;
    [HideInInspector] private GameObject _npc;
    [SerializeField] private DialogueTree dialogueTree;
    [SerializeField] private DialogueTree deadFishDialogue;
    [SerializeField] private DialogueTree deadFishTaskDialogue;

    [SerializeField] private Task.Step talkToLailaStep;
    [SerializeField] private Task.Step getInfoFromLailaStep;
    [SerializeField] private Task.Step watchVideoStep;
    [SerializeField] private Task.Skill communicationSkill;
    [SerializeField] private Task.Skill observantSkill;
    [SerializeField] private GameObject deadFishPumpVideo;
    [SerializeField] private GameObject deadFishCountVideo;
    [SerializeField] private GameObject breakAnchor;

    private DialogueBoxController dialogueController;
    private bool breakDialoguePlayed = false;

    private int answersAsked = 0;


    // Start is called before the first frame update
    void Start()
    {
        _npcSpawner = FindObjectOfType<NPCSpawner>();
        if (_npcSpawner == null)
        {
            Debug.LogError("No NPCSpawner found");
            return;
        }
        _npc = _npcSpawner.NpcInstances[0];
        dialogueController = _npc.GetComponent<DialogueBoxController>();

        ButtonSpawner.OnAnswer += ButtonSpawner_OnAnswer;
    }


    private void ButtonSpawner_OnAnswer(string answer, string question, string npcName)
    {
        if (dialogueController.DialogueTreeRestart == dialogueTree)
        {
            if (answer == "I'm ready!") // move on to dead fish set up dialogue if player answered "I'm ready!"
            {
                dialogueController.StartDialogue(deadFishDialogue, 0, "Boss", 0);
                deadFishPumpVideo.SetActive(true);
            }
            else if (answer != "No") // all answers that are not "I'm ready!" or "No" are (at time of writing) questions, thus increase counter
                answersAsked++;

            if (answersAsked >= 3) // invoke skill when 3 questions have beeen asked
                WatchManager.Instance.invokeBadge(communicationSkill);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!breakDialoguePlayed && other.CompareTag("Player"))
        {
            dialogueController.StartDialogue(dialogueTree, 0, "Boss", 0);
            WatchManager.Instance.CompleteStep(talkToLailaStep);
            breakDialoguePlayed = true;
        }
    }

    public void OnPumpVideoWatched()
    {

        StartCoroutine(DisableVideoObject(deadFishPumpVideo.GetComponent<VideoObject>()));
        WatchManager.Instance.CompleteStep(watchVideoStep);

        deadFishPumpVideo.GetComponent<DeadfishTask>().EnableEquipment();
        dialogueController.StartDialogue(deadFishTaskDialogue, 0, "Boss", 0);
        WatchManager.Instance.CompleteStep(getInfoFromLailaStep);
        deadFishCountVideo.SetActive(true);
    }

    public void OnDeadFishCountVideoWatched()
    {
        StartCoroutine(DisableVideoObject(deadFishCountVideo.GetComponent<VideoObject>()));
        WatchManager.Instance.invokeBadge(observantSkill);
        WatchManager.Instance.CompleteStep(watchVideoStep);
    }

    private IEnumerator DisableVideoObject(VideoObject videoObject)
    {
        while (videoObject.GetComponent<Grabbable>().BeingHeld)
            yield return null;

        videoObject.HideVideoPlayer();
    }


    private void OnDestroy()
    {
        ButtonSpawner.OnAnswer -= ButtonSpawner_OnAnswer;
    }
}
