using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Task;

public class WatchManager : MonoBehaviour
{
    public static WatchManager Instance;
    [SerializeField] public Task.TaskHolder taskHolder;
    [SerializeField] private AudioClip success;
    [SerializeField] public Subtask FirstSubTask;
    [SerializeField] public FeedbackManager feedbackManager;
    [SerializeField] private GameObject[] arrows;
    
    private Task.Task task;
    private int teleportationAnchorCount;
    [HideInInspector] public int stepCount;

    public Task.Task Task { get => task; set => task = value; }
    public UnityEvent<Task.Skill> BadgeChanged { get; } = new();
    public UnityEvent<Task.Skill> SkillCompleted { get; } = new();
    public UnityEvent<Task.Subtask> SubtaskChanged { get; } = new();
    public UnityEvent<Task.Task> TaskCompleted { get; } = new();
    public UnityEvent<Task.Subtask> CurrentSubtask { get; } = new();

    void Start()
    {
        task = taskHolder.taskList[0];

        // Set up tasks, subtasks, and steps. Reset subtask and step progress on each play, and skill and badge progress. Also set step number to one on feedback loop task.
        foreach (Task.Task task in taskHolder.taskList)
        {
            foreach (Task.Subtask sub in task.Subtasks)
            {
                sub.ParentTask = task;
                foreach (Task.Step step in sub.StepList)
                {
                    step.Reset();
                    step.CurrentStep = false;
                    step.ParentSubtask = sub;
                    step.setStepNumber(sub.StepList.IndexOf(step) + 1);
                }
            }
        }

        foreach (Task.Skill skill in taskHolder.skillList)
        {
            skill.Lock();
        }
        UpdateCurrentSubtask(FirstSubTask);
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
        if (step.IsCompeleted())
        {
            return;
        }
        Subtask sub = step.ParentSubtask;
        step.CompleateRep();
        UpdateCurrentSubtask(sub);
        if (step.IsCompeleted())
        {
            SubtaskChanged.Invoke(sub);
            PlayAudio(success);
            stepCount += 1;
            if (feedbackManager)
            {
                feedbackManager.emptyInstructions();
            }
            Task.Step nextStep = sub.StepList.FirstOrDefault(element => !element.IsCompeleted());
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
