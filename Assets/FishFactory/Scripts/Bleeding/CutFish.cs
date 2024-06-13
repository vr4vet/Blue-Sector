using UnityEngine;

public class CutFish : MonoBehaviour
{
    [Tooltip("The fish state script attached to the main fish object")]
    [SerializeField]
    protected FactoryFishState _fishState;

    [Tooltip("The knife script attached to the knife object")]
    protected KnifeState _knifeState;

    public GameObject[] knife;

    void Start()
    {
        knife = GameObject.FindGameObjectsWithTag("Knife");
        if (knife.Length >=1 && knife[0])
            _knifeState = knife[0].GetComponent<KnifeState>();
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
        
        bool state = _knifeState.DecrementDurabilityCount();
        if (state)
        {
            _fishState.PlaceMetalInFish();
        }
    }
}
