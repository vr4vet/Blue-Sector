using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSortingButton : MonoBehaviour
{
    // ------------------ Editor Variables ------------------

    [Tooltip("The color panel that changes color based on the tier of the fish")]
    [SerializeField]
    private GameObject _colorPanel;

    // ------------------ Public Variables ------------------
    public enum FishTier
    {
        Tier1,
        Tier2,
        Tier3
    }

    // ------------------ Private Variables ------------------

    private List<GameObject> _sortedFish = new List<GameObject>();
    public List<GameObject> SortedFish
    {
        get { return _sortedFish; }
    }

    private FishTier _currentTier;
    public FishTier CurrentTier
    {
        get { return _currentTier; }
    }

    private Material _tier1Texture;
    private Material _tier2Texture;
    private Material _tier3Texture;

    // ------------------ Unity Functions ------------------

    void Awake()
    {
        // Load materials
        _tier1Texture = Resources.Load<Material>("Materials/QAPanel/Tier1");
        _tier2Texture = Resources.Load<Material>("Materials/QAPanel/Tier2");
        _tier3Texture = Resources.Load<Material>("Materials/QAPanel/Tier3");
    }

    // ------------------ Public Functions ------------------


    /// <summary>
    ///  Sorts the fish based on the tier
    /// </summary>
    /// <param name="tier"> The tier of the fish. Should be set in the QA sorting machine button </param>
    public void SetTier(int tier)
    {
        Material material = _tier1Texture;
        switch (tier)
        {
            case 1:
                _currentTier = FishTier.Tier1;
                material = _tier1Texture;
                break;
            case 2:
                _currentTier = FishTier.Tier2;
                material = _tier2Texture;
                break;
            case 3:
                _currentTier = FishTier.Tier3;
                material = _tier3Texture;
                break;
            default:
                Debug.LogError("Invalid tier number");
                break;
        }
        if (_colorPanel)
        {
            _colorPanel.GetComponent<Renderer>().material = material;
        }
    }
}
