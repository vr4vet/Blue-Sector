using System;
using System.Collections.Generic;


namespace ProgressDTO
{
    /// <summary>
    /// Represents the progress of a single step in a subtask.
    /// </summary>
    [Serializable]
    public class StepProgressDTO
    {
        public string stepName;
        public int repetitionNumber;
        public bool completed;
    }

    /// <summary>
    /// Represents the progress of a subtask, including its steps.
    /// </summary>
    [Serializable]
    public class SubtaskProgressDTO
    {
        public string subtaskName;
        public string description;
        public bool completed;
        public List<StepProgressDTO> stepProgress;
    }

    /// <summary>
    /// Represents the progress of a task, including its subtasks.
    /// </summary>
    [Serializable]
    public class ProgressDataDTO
    {
        public string taskName;
        public string description;
        public string status;
        public List<SubtaskProgressDTO> subtaskProgress;
    }

    /// <summary>
    /// A collection of progress data for multiple tasks.
    /// </summary>
    [Serializable]
    public class ProgressDataCollection
    {
        public List<ProgressDataDTO> items;
    }
}