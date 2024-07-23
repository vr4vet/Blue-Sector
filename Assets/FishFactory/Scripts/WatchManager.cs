using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class WatchManager : MonoBehaviour
{
    public static WatchManager Instance;
    [SerializeField] public Task.TaskHolder taskHolder;
    [SerializeField] private AudioClip success;
    [SerializeField] private GameObject[] arrows;
    [SerializeField] private GameObject pauseAnchor;
    private AddInstructionsToWatch watch;
    private FeedbackManager feedbackManager;
    private Task.Task task => taskHolder.GetTask("Get ready");
    private int teleportationAnchorCount;


    [HideInInspector] public int stepCount;

    public Task.Task Task { get => task; }
    public UnityEvent<Task.Skill?> BadgeChanged { get; } = new();
    public UnityEvent<Task.Skill?> SkillCompleted { get; } = new();
    public UnityEvent<Task.Subtask?> SubtaskChanged { get; } = new();
    public UnityEvent<Task.Task> TaskCompleted { get; } = new();
    public UnityEvent<Task.Subtask?> CurrentSubtask { get; } = new();
    // Start is called before the first frame update
    private void Awake()
    {
       
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        feedbackManager = this.gameObject.GetComponent<FeedbackManager>();
        watch = this.gameObject.GetComponent<AddInstructionsToWatch>();
        UpdateCurrentSubtask(task.GetSubtask("Wash hands"));

        // Reset subtsk and step progress on each play, and skill and badge progress. Also set step number to one on feedback loop task.
        foreach (Task.Subtask sub in task.Subtasks)
        {
            foreach (Task.Step step in sub.StepList)
            {
                step.Reset();
                step.CurrentStep = false;
            }
        }
        foreach (Task.Skill skill in taskHolder.skillList)
        {
            skill.Lock();
        }

    }

    public void invokeBadge(Task.Skill badge)
    {
        BadgeChanged.Invoke(badge);
    }

    public void CompleteSubTask(Task.Subtask subtask)
    {

        subtask.SetCompleated(true);
    }

    public void CompleteStep(Task.Step step)
    {

        Task.Subtask sub = step.ParentSubtask;
        step.CompleateRep();
        UpdateCurrentSubtask(sub);
        // Task.Skill skill = taskHolder.GetSkill("Kommunikasjon");
        if (step.IsCompeleted())
        {

            SubtaskChanged.Invoke(sub);

            PlayAudio(success);
            stepCount += 1;
            feedbackManager.emptyInstructions();

            Task.Step nextStep = sub.StepList.FirstOrDefault(element => (!element.IsCompeleted()));
            if (nextStep != null)
            {
                nextStep.CurrentStep = true;
                step.CurrentStep = false;
                UpdateCurrentSubtask(sub);

            }


        }
      
        if (sub.Compleated())
        {
            string subtaskName = sub.SubtaskName;
            SubtaskChanged.Invoke(sub);
            if (task.Compleated())
            {
                TaskCompleted.Invoke(task);
            }
            if (sub.SubtaskName == "Runde På Ring")
            {
                Task.Skill skill = taskHolder.GetSkill("Problemløser");
                BadgeChanged.Invoke(skill);
            }

            Task.Subtask nextSubtask = task.Subtasks.FirstOrDefault(element => (!element.Compleated()));

            if (nextSubtask != null)
            {
                Task.Step nextStep = nextSubtask.StepList.FirstOrDefault(element => (!element.IsCompeleted()));
                step.CurrentStep = false;
                nextStep.CurrentStep = true;
                UpdateCurrentSubtask(nextSubtask);
            }

        }
    }


    public void UpdateCurrentSubtask(Task.Subtask subtask)
    {
        CurrentSubtask.Invoke(subtask);

    }


    public Task.Step GetStep(string subtaskName, string stepName)
    {
        Task.Subtask sub = task.GetSubtask(subtaskName);

        Task.Step step = sub.GetStep(stepName);
        return step;
    }

    public Task.Subtask GetSubtask(string subtaskName)
    {
        Task.Subtask sub = task.GetSubtask(subtaskName);
        return sub;
    }



    public void PlayAudio(AudioClip audio)
    {
        //if the gameobject has audiosource
        if (TryGetComponent<AudioSource>(out AudioSource audioSource))
        {
            audioSource.Stop();
            audioSource.PlayOneShot(audio);
            return;
        }

        //otherwise create audiosource
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.PlayOneShot(audio);
    }

    public void incrementTeleportationAnchorCount()
    {
        teleportationAnchorCount += 1;
    }

    public int getTeleportationAnchorCount()
    {
        return teleportationAnchorCount;
    }

}
