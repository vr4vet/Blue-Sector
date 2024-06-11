using UnityEngine;

public class CutFish : MonoBehaviour
{
    [Tooltip("The fish state script attached to the main fish object")]
    [SerializeField]
    protected FactoryFishState _fishState;

    [Tooltip("The knife script attached to the knife object")]
    protected KnifeState _knifeState;

    void Start()
    {
        GameObject knife = GameObject.Find("FishKnife");
        if (knife)
            _knifeState = knife.GetComponent<KnifeState>();
    }

    protected void cutEvent(string tag, bool isGills = false)
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
