using UnityEngine;
using Task;
public class CutFishGill : MonoBehaviour
{
    [Tooltip("The fish state script attached to the main fish object")]
    [SerializeField]
    private FactoryFishState fishState;

    [SerializeField]
    private GameObject TaskHolder; // Reference to the TaskHolder GameObject

    /// <summary>
    /// When the knife collides with the fish gill, cut the gill
    /// </summary>
    /// <param name="collider"> The knife collider </param>
    private void OnTriggerEnter(Collider collider)
    {
        if (!collider.CompareTag("Knife"))
        {
            return;
        }

        // Cut the fish gills
        fishState.CutFishGills();

        // Update the task step
        UpdateTaskStep();
    }

    private void UpdateTaskStep()
    {
        // Access the TaskHolder component from the TaskHolder object
        TaskHolder taskHolder = TaskHolder.GetComponent<TaskHolder>();

        // Access the specific step associated with cutting fish gills
        Step step = taskHolder.GetTask("Euthanize fish").GetSubtask("Bleed fish").GetStep("Cut fish gills");

        // Complete a repetition of the step
        step.CompleateRep();
    }
}