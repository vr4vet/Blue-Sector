using UnityEngine;

public class CutFish : MonoBehaviour
{
    [Tooltip("The fish state script attached to the main fish object")]
    [SerializeField]
    private FactoryFishState _fishState;

    [Tooltip("The knife script attached to the knife object")]
    [SerializeField]
    private KnifeState _knifeState;

    void Start()
    {
        GameObject knife = GameObject.Find("FishKnife");
        _knifeState = knife.GetComponent<KnifeState>();
    }

    /// <summary>
    /// When the knife collides with the fish body, the player makes a wrong cut
    /// </summary>
    /// <param name="collision"> The knife object </param>
    private void OnCollisionEnter(Collision collisionObject)
    {
        cutEvent(collisionObject.collider.tag, "body");
    }
    
    private void OnTriggerEnter(Collider collider)
    {
        cutEvent(collider.tag, "gill");
    }

    private void cutEvent (string tag, string fishPart)
    {
        tag != "Knife" && return;
        fishPart == "gill" fishState.CutFishGills() : fishState.CutFishBody();
        knifeState.DecrementDurabilityCount();
    }
}
