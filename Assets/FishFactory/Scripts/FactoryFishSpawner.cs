using System.Collections;
using UnityEngine;

public class FactoryFishSpawner : MonoBehaviour
{
    // ----------------- Editor Variables -----------------

    [Header("Fish Spawner Settings")]
    [SerializeField]
    private bool _isSpawnerOn = true;

    [SerializeField]
    [Tooltip(
        "The gameobject prefab to spawn. This prefab will be used as the tier 1 fish if fish tiers are enabeled"
    )]
    private GameObject _salmonFishPrefab;

    [SerializeField]
    [Tooltip("The maximum amount of fish that can be spawned by this spawner")]
    private int _maxAmountOfFish;

    [SerializeField]
    [Range(0.1f, 10)]
    [Tooltip("The rate at which the fish will spawn in seconds")]
    private float _spawnRate;

    [SerializeField]
    [Range(0, 10)]
    [Tooltip("The maximum spawn delay variation in seconds. 0 means no variation.")]
    private float _varationInSpawnrate;

    [Header("Fish Variation Settings")]
    [SerializeField]
    [Tooltip("If toggled, the fish will spawn in different sizes")]
    private bool _fishSizeVariation;

    [SerializeField]
    [Range(0, 100)]
    [Tooltip("The percentage of fish that should be alive. Higher number equals higher chance.")]
    private int _aliveFishPercent = 10;

    [SerializeField]
    [Range(0, 100)]
    [Tooltip(
        "The percentage of fish that should be bad quality. Higher number equals higher chance. The remaining percentage will be stunned."
    )]
    private int _badFishPercent = 10;

    [SerializeField]
    [Tooltip("The gameobject prefab to spawn if fish is bad or dead and should be thrown away")]
    private GameObject _badfishPrefab;

    [Header("Fish Tier Settings")]
    [SerializeField]
    [Tooltip("If toggled, the fish will spawn in different tiers")]
    private bool _toggleFishTier;

    [SerializeField]
    [Range(0, 100)]
    [Tooltip("The percentage of fish that should be Tier 1. Higher number equals higher chance.")]
    private int _tier1Percentage = 25;

    [SerializeField]
    [Range(0, 100)]
    [Tooltip(
        "The percentage of fish that should be Tier 2. Higher number equals higher chance. The remaining percentage will be Tier 3."
    )]
    private int _tier2Percentage = 50;

    [Header("Fish Gutting Settings")]
    [SerializeField]
    [Tooltip(
        "If toggled, the fish will be assigned a state defining if it has been successfully gutted or not"
    )]
    private bool _toggleFishGuttingChance;

    [SerializeField]
    [Range(0, 100)]
    [Tooltip("The percentage of fish that should be successfully gutted")]
    private int _successfullGuttingChance = 65;

    [SerializeField]
    [Range(0, 100)]
    [Tooltip("The percentage of fish that are not completely gutted")]
    private int _incompleteGuttingChance = 25;

    [Header("Use secondary task")]
    [SerializeField]
    [Tooltip("If true, the spawner will be turned on and off by the secondary task")]
    private bool _useSecondaryTask;

    // ------------------ Private Variables ------------------

    // Counts the amount of child gameobjects in the spawner
    private int _currentAmountOfFish;

    private Material _tier2;
    private Material _tier3;

    // ------------------ Unity Functions ------------------

    void Awake()
    {
        _tier2 = Resources.Load<Material>("Materials/Fish/salmonTier2");
        _tier3 = Resources.Load<Material>("Materials/Fish/salmonTier3");
    }

    // Start is called before the first frame update
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
        bool taskState;

        if (_useSecondaryTask)
        {
            taskState = GameManager.Instance.IsSecondaryTaskOn;
        }
        else
        {
            taskState = GameManager.Instance.IsTaskOn;
        }

        // XOR operator: If isTaskOn and isSpawnerOff are not the same, update the spawner state
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
    /// Initializes fish conveyor movement by adding a force to the fish
    /// </summary>
    private void InitializeConveyorMovement()
    {
        foreach (Transform child in transform)
        {
            Rigidbody rb = child.GetChild(2).GetChild(0).GetComponent<Rigidbody>();

            // Move the fish a bit to initialize a collision with the conveyor belt after turning it back on
            //FIXME: This is a solution to get the fish moving while dealing with the current iteration of the conveyor prefab. Should be replaced at a later stage
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
            GameObject fishPrefab = _salmonFishPrefab;
            string fishTag = "fish";

            // Get a random state and sets prefab to badfishPrefab if the state is BadQuality
            FactoryFishState.State randomizedFishState = RandomizeFishState();
            if (randomizedFishState == FactoryFishState.State.BadQuality)
            {
                fishPrefab = _badfishPrefab;
            }

            // Spawn object as a child of the spawner object, and as such limit the amount of spawned objects to increase performance.
            GameObject childGameObject = Instantiate(
                fishPrefab,
                transform.position,
                Random.rotation,
                transform
            );
            childGameObject.name = "FactoryFish" + transform.childCount.ToString();
            Renderer fishMaterial = childGameObject.transform.GetChild(0).GetComponent<Renderer>();

            // Randomizes the tier of the fish if enabled
            if (_toggleFishTier)
            {
                int randomValue = Random.Range(1, 101);

                if (randomValue <= _tier1Percentage)
                {
                    randomizedFishState = FactoryFishState.State.Tier1;
                }
                else if (randomValue <= _tier1Percentage + _tier2Percentage)
                {
                    randomizedFishState = FactoryFishState.State.Tier2;
                    fishMaterial.material = _tier2;
                }
                else
                {
                    randomizedFishState = FactoryFishState.State.Tier3;
                    fishMaterial.material = _tier3;
                }
            }

            // Set the state of the fish to the randomizedFishState
            FactoryFishState fishState = childGameObject.GetComponent<FactoryFishState>();
            if (fishState != null)
            {
                fishState.CurrentState = randomizedFishState;
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
    private FactoryFishState.State RandomizeFishState()
    {
        // Generates a number from 1 to 100 and assigns a fish state based on the number
        int randomValue = Random.Range(1, 101);

        FactoryFishState.State state;

        if (_toggleFishGuttingChance)
        {
            if (randomValue <= _successfullGuttingChance)
            {
                state = FactoryFishState.State.GuttingSuccess;
            }
            else if (randomValue <= _successfullGuttingChance + _incompleteGuttingChance)
            {
                state = FactoryFishState.State.GuttingIncomplete;
            }
            else
            {
                state = FactoryFishState.State.GuttingFailure;
            }
            return state;
        }

        if (randomValue <= _aliveFishPercent)
        {
            state = FactoryFishState.State.Alive;
        }
        else if (randomValue <= _aliveFishPercent + _badFishPercent)
        {
            state = FactoryFishState.State.BadQuality;
        }
        else
        {
            state = FactoryFishState.State.Stunned;
        }
        return state;
    }
}
