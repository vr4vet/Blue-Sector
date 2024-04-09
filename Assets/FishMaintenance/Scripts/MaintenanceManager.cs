using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class MaintenanceManager : MonoBehaviour
{
    public static MaintenanceManager Instance;
    [SerializeField] private Task.TaskHolder taskHolder;
    [SerializeField] private Tablet.TaskListLoader1 taskListLoader;
    [SerializeField] private AudioClip success;
    [SerializeField] private AudioClip equipmentPickup;
    private AddInstructionsToWatch watch;
    private FeedbackManager feedbackManager;
    private Task.Task task;
    [HideInInspector] public int stepCount;

    public UnityEvent<Task.Subtask?> SubtaskChanged { get; } = new();
    public UnityEvent<Task.Step?> BadgeChanged { get; } = new();
    public UnityEvent<Task.Skill?> SkillCompleted { get; } = new();

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
        task = taskHolder.GetTask("Vedlikehold");

        // Reset subtsk and step progress on each play, and skill and badge progress
        foreach (Task.Subtask sub in task.Subtasks)
        {
            foreach (Task.Step step in sub.StepList)
            {
                step.Reset();
            }
        }
        foreach (Task.Skill skill in taskHolder.skillList)
        {
            skill.Reset();
        }

    }


    public void CompleteStep(string subtaskName, string stepName)
    {

        Task.Subtask sub = task.GetSubtask(subtaskName);
        Task.Step step = sub.GetStep(stepName);
        step.CompleateRep();
        taskListLoader.SubTaskPageLoader(sub);
        taskListLoader.TaskPageLoader(task);

        // Task.Skill skill = taskHolder.GetSkill("Kommunikasjon");
        if (step.IsCompeleted())
        {
            PlayAudio(success);
            stepCount += 1;
            feedbackManager.emptyInstructions();
            BadgeChanged.Invoke(step);
        }
        if (sub.Compleated())
        {
            SubtaskChanged.Invoke(sub);
            // SkillCompleted.Invoke(skill);
        }
    }



    public Task.Subtask GetSubtask(string subtaskName)
    {

        return task.GetSubtask(subtaskName);
    }

    public Task.Step GetStep(string subtaskName, string stepName)
    {

        Task.Subtask sub = task.GetSubtask(subtaskName);

        Task.Step step = sub.GetStep(stepName);
        return step;
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

}
