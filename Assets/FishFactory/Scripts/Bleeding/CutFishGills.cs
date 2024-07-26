using Task;
using UnityEngine;
using UnityEngine.Events;

public class CutFishGills : CutFish
{
    private WatchManager watchManager;

    void Start()
    {
        watchManager = FindObjectOfType<WatchManager>();
    }
    /// <summary>
    /// When the knife collides with the fish gills, the player makes a succesful cut
    /// </summary>
    /// <param name="collision"> The knife object </param>
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Knife" && !_fishState.correctlyBled)
        {
            Step step = watchManager.GetStep("Get used to working at this station","Correctly bleed 15 fish");
            if (!step.IsCompeleted())
            {
                watchManager.CompleteStep(step);
            }

            cutEvent(collider.tag, true);
        }
    }
}
