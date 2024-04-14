using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSortingButton : MonoBehaviour
{
    public static FishSortingButton fishSortingButton;

    public enum FishTier
    {
        Tier1,
        Tier2, 
        Tier3
    }

    public FishTier currentFishTier;

    // list of sorted fish
    public List<GameObject> sortedFish = new List<GameObject>();

    // three set tier methods for each button and tier
    public void setTier1()
    {
        currentFishTier = FishTier.Tier1;
        Debug.Log(currentFishTier);
    }
    public void setTier2()
    {
        currentFishTier = FishTier.Tier2;
        Debug.Log(currentFishTier);
    }
    public void setTier3()
    {
        currentFishTier = FishTier.Tier3;
        Debug.Log(currentFishTier);
    }
    void Awake()
    {
        // Sets the instance of the FishSortingButton to this object if it does not already exist
        if (fishSortingButton == null)
        {
            fishSortingButton = this;
        }
    }
}
