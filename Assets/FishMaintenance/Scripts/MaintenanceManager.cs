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
    private Task.Task task;
    [HideInInspector] public int stepCount;

    public UnityEvent<Task.Subtask?> SubtaskChanged { get; } = new();

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
        task = taskHolder.GetTask("Vedlikehold");

        // Reset subtsk and step progress on each play
        foreach (Task.Subtask sub in task.Subtasks)
        {
            foreach (Task.Step step in sub.StepList)
            {
                step.Reset();
            }
        }

    }


    public void CompleteStep(string subtaskName, string stepName)
    {

        Task.Subtask sub = task.GetSubtask(subtaskName);
        Task.Step step = sub.GetStep(stepName);
        step.CompleateRep();
        taskListLoader.SubTaskPageLoader(sub);
        taskListLoader.TaskPageLoader(task);
        taskListLoader.LoadSkillsPage();
        if (step.IsCompeleted())
        {
            PlaySuccess();
            stepCount += 1;
        }
        if (sub.Compleated())
        {

            SubtaskChanged.Invoke(sub);
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

    public void PlaySuccess()
    {
        PlayAudio(success);
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
