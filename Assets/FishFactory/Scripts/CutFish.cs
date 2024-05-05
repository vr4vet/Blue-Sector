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

    private void OnTriggerEnter(Collider collider)
    {
        cutEvent(collider.tag, true);
    }

    /// <summary>
    /// When the knife collides with the fish body, the player makes a wrong cut
    /// </summary>
    /// <param name="collision"> The knife object </param>
    private void OnCollisionEnter(Collision collisionObject)
    {
        cutEvent(collisionObject.collider.tag, false);
    }

    private void cutEvent(string tag, bool isGills)
    {
        if (tag != "Knife")
            return;
        if (isGills)
        {
            _fishState.CutFishGills();
        }
        else
        {
            _fishState.CutFishBody();
        }
        _knifeState.DecrementDurabilityCount();
    }
}
