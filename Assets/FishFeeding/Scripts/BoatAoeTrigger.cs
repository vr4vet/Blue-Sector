using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider))]
public class BoatAoeTrigger : MonoBehaviour
{
    //public bool MaintenanceBoat;
    
    /// <summary>
    /// Gets an event that is fired when the player enters the box collider.
    /// </summary>
    public UnityEvent OnPlayerEnter;

    /// <summary>
    /// Gets an event that is fired when the player exits the box collider.
    /// </summary>
    public UnityEvent OnPlayerExit;

    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other))
        {
/*            if (MaintenanceBoat)
            {
                SceneManager.LoadScene("FishFeeding");
                return;
            }*/
            OnPlayerEnter.Invoke();
        }

        if (other.CompareTag("Growler"))    // anesthetic bottle ("growler")
        {
            other.GetComponent<Respawner>().Respawn();
        }
        else if (other.CompareTag("Bone"))  // fish
        {
            other.GetComponentInParent<Respawner>().Respawn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other))
        {
            OnPlayerExit.Invoke();
        }
    }

    private static bool IsPlayer(Collider collider) => collider.gameObject.tag == "Player" || collider.gameObject.name == "Grabber";
}