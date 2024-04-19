using Task;
using UnityEngine;

public class CutFishGill : MonoBehaviour
{
    [Tooltip("The fish state script attached to the main fish object")]
    [SerializeField] 
    private FactoryFishState fishState;
            
    [Tooltip("Reference to the TaskHolder component")]
    public TaskHolder taskHolder;
            
    private void Start()
    { 
        // Ensure the TaskHolder component is assigned
        if (taskHolder == null)
        { 
            Debug.LogError("TaskHolder component not assigned to CutFishGill script!");
        }
    }
    
    /// <summary>
    /// When the knife collides with the fish gill, cut the gill
    /// </summary>
    /// <param name="collider"> The knife collider </param>
    private void OnTriggerEnter(Collider collider) 
    { 
        if (collider.CompareTag("Knife")) 
        { 
            // Cut the fish gills
            fishState.CutFishGills();
            // Update the task step
            UpdateTaskStep();
        }
        else
        {
            return;
        }
    }
    
    /// <summary>
    /// Update the task step after cutting the fish gills
    /// </summary>
    private void UpdateTaskStep()
    {
        // Get the task, subtask, and step
        Task.Task task = taskHolder.GetTask("euthanize fish");
        Subtask subtask = task.GetSubtask("bleed fish");
        Step step = subtask.GetStep("cut gills");

        // Complete a repetition of the step
        step.CompleateRep();
    }
}
