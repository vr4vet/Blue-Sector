using System.Collections;
using UnityEngine;

public class FactoryFishSpawner : MonoBehaviour
{
    // ----------------- Editor Variables -----------------

    [Header("Fish Spawner Settings")]
    [SerializeField]
    private bool _isSpawnerOn = true;

    [Tooltip(
        "The gameobject prefab to spawn. This prefab will be used as the tier 1 fish if fish tiers are enabeled"
    )]
    [SerializeField]
    private GameObject _salmonFishPrefab;

    [Tooltip("The maximum amount of fish that can be spawned by this spawner")]
    [SerializeField]
    private int _maxAmountOfFish;

    [Tooltip("The rate at which the fish will spawn in seconds")]
    [SerializeField]
    [Range(0.1f, 10)]
    private float _spawnRate;

    [Tooltip("The maximum spawn delay variation in seconds. 0 means no variation.")]
    [SerializeField]
    [Range(0, 10)]
    private float _varationInSpawnrate;

    [Header("Fish Variation Settings")]
    [Tooltip("If toggled, the fish will spawn in different sizes")]
    [SerializeField]
    private bool _fishSizeVariation;

    [Tooltip("The percentage of fish that should be alive. Higher number equals higher chance.")]
    [SerializeField]
    [Range(0, 100)]
    private int _aliveFishPercent = 10;

    [Tooltip("If toggled, the fish will spawn in different tiers")]
    [Header("Fish Tier Settings")]
    [SerializeField]
    private bool _toggleFishTier;

    [Tooltip("The percentage of fish that should be Tier 1. Higher number equals higher chance.")]
    [SerializeField]
    [Range(0, 100)]
    private int _tier1Percentage = 25;

    [Tooltip(
        "The percentage of fish that should be Tier 2. Higher number equals higher chance. The remaining percentage will be Tier 3."
    )]
    [SerializeField]
    [Range(0, 100)]
    private int _tier2Percentage = 40;
    [Tooltip(
        "The percentage of fish that should be Tier 2. Higher number equals higher chance. The remaining percentage will be Tier 3."
    )]
    [SerializeField]
    [Range(0, 100)]
    private int _tier3Percentage = 15;

    [Tooltip(
        "The percentage of fish that should be bad quality. Higher number equals higher chance. The remaining percentage will be stunned."
    )]
    [SerializeField]
    [Range(0, 100)]
    private int _badFishPercent = 10;

    [Tooltip("The gameobject prefab to spawn if fish is bad or dead and should be thrown away")]
    [SerializeField]
    private GameObject _badfishPrefab;

    [Tooltip(
        "If toggled, the fish will be assigned a state defining if it has been successfully gutted or not"
    )]
    [Header("Fish Gutting Settings")]
    [SerializeField]
    private bool _toggleFishGuttingChance;

    [Tooltip("The percentage of fish that should be successfully gutted")]
    [SerializeField]
    [Range(0, 100)]
    private int _successfullGuttingChance = 65;

    [Tooltip("The percentage of fish that are not completely gutted")]
    [Range(0, 100)]
    [SerializeField]
    private int _incompleteGuttingChance = 25;

    [Header("Use secondary task")]
    [Tooltip("If true, the spawner will be turned on and off by the secondary task")]
    [SerializeField]
    private bool _useSecondaryTask;

    // ------------------ Private Variables ------------------

    // Counts the amount of child gameobjects in the spawner
    private int _currentAmountOfFish;

    private Material _tier2;
    private Material _tier3;
    private FactoryFishState.GuttingState randomizedGuttingState;
    private FactoryFishState.Tier randomizedTier;
    private bool randomizedStunnChance;
    private GameObject fishPrefab;


    // ------------------ Unity Functions ------------------

    void Awake()
    {
        _tier2 = Resources.Load<Material>("Materials/Fish/salmonTier2");
        _tier3 = Resources.Load<Material>("Materials/Fish/salmonTier3");
    }

    void Start()
    {
        StartCoroutine(SpawnFish());
    }

    void Update()
    {
        UpdateSpawnerState();

        // Checks the amount of spawned gameobjects in the spawner
        _currentAmountOfFish = transform.childCount;
    }

    // ------------------ Private Functions ------------------

    /// <summary>
    /// Updates the state of the spawner based on the task state
    /// </summary>
    private void UpdateSpawnerState()
    {
        // The task state uses the secondary task if the useSecondaryTask is true
        bool taskState = _useSecondaryTask
            ? GameManager.Instance.IsSecondaryTaskOn
            : GameManager.Instance.IsTaskOn;

        //If isTaskOn and isSpawnerOff are not the same, while not equal, update the spawner state.
        if (taskState ^ _isSpawnerOn)
        {
            _isSpawnerOn = taskState;
            if (_isSpawnerOn)
            {
                InitializeConveyorMovement();
            }
        }
    }

    /// <summary>
    /// Initializes fish conveyor movement by adding a force to the fish.
    /// This function should be moved to a script directly related to a conveyor belt in the future,
    /// however this is not compatible with conveyour prefabs provided to the team (per May 2024).
    /// </summary>
    private void InitializeConveyorMovement()
    {
        // Move the fish a bit to initialize a collision with the conveyor belt after turning it back on
        foreach (Transform child in transform)
        {
            Rigidbody rb = child.GetChild(2).GetChild(0).GetComponent<Rigidbody>();
            rb.AddForce(transform.up * 50, ForceMode.Acceleration);
        }
    }

    /// <summary>
    /// Spawns fish at a set rate. The fish will have different states, sizes and tiers based on the spawner settings.
    /// </summary>
    /// <returns> Waits for a set amount of time before spawning a fish </returns>
    private IEnumerator SpawnFish()
    {
        // Waits for number of seconds equal to the spawnrate + variation
        yield return new WaitForSeconds(_spawnRate + RandomizeSpawnRateVariation());

        if (_currentAmountOfFish < _maxAmountOfFish && _isSpawnerOn)
        {
            // The prefab to modify and spawn in the spawner
            fishPrefab = _salmonFishPrefab;
            int randomValue = Random.Range(1, 101);

            // Get a random state and sets prefab to badfishPrefab if the state is BadQuality
            if (_toggleFishGuttingChance)
            {
                randomizedGuttingState = RandomizeGuttingState(randomValue);
            }
            if (_toggleFishTier)
            {
                randomizedTier = RandomizeFishTier(randomValue);
            }
            randomizedStunnChance = RandomizeAliveFishState(randomValue);

            // Spawn object as a child of the spawner object, and as such limit the amount of spawned objects to increase performance.
            GameObject childGameObject = Instantiate(
                fishPrefab,
                transform.position,
                Random.rotation,
                transform
            );
            childGameObject.name = "FactoryFish" + transform.childCount.ToString();
            Renderer fishMaterial = childGameObject.transform.GetChild(0).GetComponent<Renderer>();
            
            // Set the states of the fish to the randomized states
            FactoryFishState fishState = childGameObject.GetComponent<FactoryFishState>();
            if (fishState != null)
            {
                fishState.fishTier = randomizedTier;
                fishState.guttingState = randomizedGuttingState;
                fishState.Stunned = randomizedStunnChance;

                switch (randomizedTier)
                {
                    case FactoryFishState.Tier.Tier2:
                        fishMaterial.material = _tier2;
                        break;
                    case FactoryFishState.Tier.Tier3:
                        fishMaterial.material = _tier3;
                        break;
                }
            }

            // Randomizes the size of the fish if enabled
            if (_fishSizeVariation)
            {
                float randomSize = RandomizeObjectSize();
                childGameObject.transform.localScale = new Vector3(
                    randomSize,
                    randomSize,
                    randomSize
                );
            }
        }

        // Recursively call the function to spawn fish
        StartCoroutine(SpawnFish());
    }

    /// <summary>
    /// Randomizes the size of the fish object
    /// </summary>
    /// <returns> The size of the fish object </returns>
    private float RandomizeObjectSize()
    {
        // the size variation of the fish relative to the parent spawner
        return Random.Range(0.7f, 1.2f);
    }

    /// <summary>
    /// Randomizes the rate of which the fish will spawn
    /// </summary>
    /// <returns> The spawn rate variation </returns>
    private float RandomizeSpawnRateVariation()
    {
        return Random.Range(0.5f, _varationInSpawnrate);
    }

    /// <summary>
    /// Randomizes the state of the fish
    /// </summary>
    /// <returns> The state of the fish </returns>
    private FactoryFishState.GuttingState RandomizeGuttingState(int randomValue)
    {
        FactoryFishState.GuttingState state;

        if (randomValue <= _successfullGuttingChance)
        {
            state = FactoryFishState.GuttingState.GuttingSuccess;
        }
        else if (randomValue <= _successfullGuttingChance + _incompleteGuttingChance)
        {
            state = FactoryFishState.GuttingState.GuttingIncomplete;
        }
        else
        {
            state = FactoryFishState.GuttingState.GuttingFailure;
        }
        return state;
    }

    private bool RandomizeAliveFishState(int randomValue)
    {
        bool state;

        if (randomValue <= _aliveFishPercent)
        {
            state = false;
        }
        else
        {
            state = true;
        }
        return state;
    }
    private FactoryFishState.Tier RandomizeFishTier(int randomValue)
    {
        FactoryFishState.Tier state;

        if (randomValue <= _tier1Percentage)
        {
            state = FactoryFishState.Tier.Tier1;
        }
        else if (randomValue <= _tier1Percentage + _tier2Percentage)
        {
            state = FactoryFishState.Tier.Tier2;
        }
        else if (randomValue <= _tier1Percentage + _tier2Percentage + _tier3Percentage)
        {
            state = FactoryFishState.Tier.Tier3;
        }
        else 
        {
            state = FactoryFishState.Tier.BadQuality;
            fishPrefab = _badfishPrefab;
        }
        return state;
    }
}
