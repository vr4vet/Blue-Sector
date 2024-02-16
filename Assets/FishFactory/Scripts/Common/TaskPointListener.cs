using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TaskPointListener
{
    /// <summary>
    /// Adds points to the task for the players "skill".
    /// </summary>
    /// <param name="taskId">Task to add points to</param>
    /// <param name="skill">Skill used by the player</param>
    /// <param name="points">Amount of points to give</param>
    public void AddPoints(string taskId, string skill, int points);
}
