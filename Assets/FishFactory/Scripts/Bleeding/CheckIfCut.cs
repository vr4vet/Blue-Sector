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
            if (fish.tag == "Fish")
            {
                Tier fishTier = fish.GetComponent<FactoryFishState>().fishTier;
                bool Stunned = fish.GetComponent<FactoryFishState>().Stunned;
                bool correctlyBled = fish.GetComponent<FactoryFishState>().correctlyBled;
                bool containsMetal = fish.GetComponent<FactoryFishState>().ContainsMetal;

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
                if (containsMetal)
                {
                    _errorController.GetComponent<CutFishWrong>().MetalInFish();
                    return;
                }
            }

        }
    }
}
