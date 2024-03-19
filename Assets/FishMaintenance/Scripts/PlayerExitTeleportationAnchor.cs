using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExitTeleportationAnchor : MonoBehaviour
{
    public GameObject cylinder;
    public GameObject cylinderGlow;
    public MaintenanceManager manager;
    [SerializeField] private string subTask;
    [SerializeField] private string step;
    [SerializeField] private AddInstructionsToWatch watch;
    [SerializeField] private GameObject floatingToast;


    private BoxCollider boxCollider;
    private Vector3 originalSize;

    private bool subtaskComplete = false;

    void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider>();
        originalSize = boxCollider.size;
        manager.SubtaskChanged.AddListener(OnSubtaskCompleted);
        // activeSubtask = manager.GetSubtask(subTask);

    }

    void OnSubtaskCompleted(Task.Subtask subtask)
    {
        if (subtask.SubtaskName == subTask)
        {
            floatingToast.SetActive(true);
            subtaskComplete = true;
        };
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            cylinder.SetActive(false);
            cylinderGlow.SetActive(false);
            boxCollider.size = new Vector3(1.55380726f, 2.72183228f, 2.2479949f);
        }
    }
    public void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            cylinder.SetActive(true);
            cylinderGlow.SetActive(true);
            watch.emptyInstructions();
            boxCollider.size = originalSize;
            // if (string.IsNullOrEmpty(step) && manager.GetSubtask(subTask).Compleated())
            // {
            //     gameObject.SetActive(false);
            //     return;

            // }
            try
            {
                if (subtaskComplete)
                {
                    gameObject.SetActive(false);
                    return;
                }
                else if (manager.GetStep(subTask, step).IsCompeleted())
                {
                    gameObject.SetActive(false);
                    return;
                }
            }
            catch
            {

            }

        }


    }


}




