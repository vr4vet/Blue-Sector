using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FactoryFishState;
using static FishSortingButton;

public class CheckIfCut : MonoBehaviour
{


    private GameObject _errorController;

    private void Start()
    {
        _errorController = GameObject.Find("ErrorController");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.gameObject && other.name == "Head")
        {
            GameObject fish = other.transform.parent.transform.parent.gameObject;
            Debug.Log(fish.tag + " is the tag of " + other.name + "'s parent object");
            if (fish.tag == "Fish")
            {
                Debug.Log("hehe");
                Tier fishTier = fish.GetComponent<FactoryFishState>().fishTier;
                bool Stunned = fish.GetComponent<FactoryFishState>().Stunned;
                bool correctlyBled = fish.GetComponent<FactoryFishState>().correctlyBled;

                if (fishTier == Tier.BadQuality)
                {
                    _errorController.GetComponent<CutFishWrong>().CutBadFish();
                    return;
                }
                if (!Stunned && !correctlyBled)
                {
                    _errorController.GetComponent<CutFishWrong>().ForgotToCutFish();
                    return;
                }
                if (Stunned && !correctlyBled)
                {
                    _errorController.GetComponent<CutFishWrong>().ForgotToCutFish();
                    return;
                }
            }

        }
    }
}
