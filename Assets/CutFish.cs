using System.Globalization;
using System.Linq;
using Task;
using UnityEngine;

public class CutFish : MonoBehaviour
{
    [Tooltip("The fish state script attached to the main fish object")]
    [SerializeField]
    protected FactoryFishState _fishState;

    [Tooltip("The knife script attached to the knife object")]
    protected KnifeState _knifeState;

    public GameObject[] knifes;

    private int numbersCut = 0;

    private bool state;

    private WatchManager watchManager;
    private TaskHolder taskHolder;


    void Start()
    {
        //Find all knifes in scene and choose the correct knife state
        knifes = GameObject.FindGameObjectsWithTag("Knife");
        if (knifes.Length >=1 && knifes[0])
        {
            foreach (GameObject knife in knifes)
            {
                if (knife.name == "FishKnife")
                {
                    _knifeState = knife.GetComponent<KnifeState>();
                }
                else
                {
                    _knifeState = knifes[0].GetComponent<KnifeState>();
                }
            }
        }
    }
    
    protected void cutEvent(string tag, bool isGills = false)
    {
        if (tag != "Knife")
            return;
        if (isGills)
        {
            numbersCut++;
            _fishState.CutFishGills();
            if (numbersCut == 20)
            {
                watchManager = GameObject.FindObjectOfType<WatchManager>();
                taskHolder = GameObject.FindObjectOfType<TaskHolder>();

                Task.Skill skill = taskHolder.GetSkill("Adept with the knife");
                watchManager.BadgeChanged.Invoke(skill);
            }
        }
        else
        {
            _fishState.CutFishBody();
        }
        if (_knifeState)
        {
            state = _knifeState.DecrementDurabilityCount();
        }
        if (state)
        {
            _fishState.PlaceMetalInFish();
        }
    }
}
