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

    // ------------------ Private Variables ------------------

    private Material tier1;
    private Material tier2;
    private Material tier3;
    private GameObject ColorPanel;

    // ------------------ Unity Functions ------------------

    void Awake()
    {
        // Load materials
        tier1 = Resources.Load<Material>("Materials/QAPanel/Tier1");
        tier2 = Resources.Load<Material>("Materials/QAPanel/Tier2");
        tier3 = Resources.Load<Material>("Materials/QAPanel/Tier3");

        // Sets the instance of the FishSortingButton to this object if it does not already exist
        if (fishSortingButton == null)
        {
            fishSortingButton = this;
        }
        // Find first instance of ColorBox, if there are more than one sorting machine that the player should be able to use this will need to be changed
        ColorPanel = GameObject.Find("ColorBox");
    }

    // three set tier methods for each button and tier
    public void setTier1()
    {
        currentFishTier = FishTier.Tier1;
        ColorPanel.GetComponent<Renderer>().material = tier1;
        Debug.Log(currentFishTier);
    }
    public void setTier2()
    {
        currentFishTier = FishTier.Tier2;
        ColorPanel.GetComponent<Renderer>().material = tier2;
        Debug.Log(currentFishTier);
    }
    public void setTier3()
    {
        currentFishTier = FishTier.Tier3;
        ColorPanel.GetComponent<Renderer>().material = tier3;
        Debug.Log(currentFishTier);
    }
}
