using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class FactoryFishSpawner : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField]
    private bool isSpawnerOn = true;

    [SerializeField]
    [Tooltip("The gameobject prefab to spawn")]
    private GameObject fishPrefab;

    [SerializeField]
    [Tooltip("The maximum amount of fish that can be spawned")]
    private int maxAmountOfFish;

    [SerializeField]
    [Tooltip(
        "The chance of a fish being a different state than stunned. Higher number equals lower chance"
    )]
    private int randomFishStateChance;

    // [SerializeField]
    // [Tooltip("The chance of a fish being dead. Higher number equals higher chance")]
    // private int fishDeadChance;

    [SerializeField]
    [Range(2, 20)]
    [Tooltip("The chance of a fish not being stunned. Higher number equals lower chance")]
    private int fishAliveChance;

    // Counts the amount of child gameobjects in the spawner
    private int currentAmountOfFish;

    [SerializeField]
    [Range(0.1f, 10)]
    [Tooltip("The rate at which the fish will spawn in seconds")]
    private float spawnRate;

    [SerializeField]
    [Range(0, 10)]
    [Tooltip("The maximum spawn delay variation in seconds. 0 means no variation.")]
    private float varationInSpawnrate;

    [SerializeField]
    [Tooltip("If toggled, the fish will spawn with the different sizes")]
    private bool fishSizeVariation;

    [SerializeField]
    [Tooltip("If toggled, the fish will spawn with the different sizes")]
    private bool toggleFishTier;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;

        StartCoroutine(SpawnFish());
    }

    void Update()
    {
        // True the first time the task is turned on and the spawner has not yet turned on
        if (gameManager.IsTaskOn && !isSpawnerOn)
        {
            isSpawnerOn = true;

            foreach (Transform child in transform)
            {
                Rigidbody rb = child
                    .transform.GetChild(0)
                    .transform.GetChild(0)
                    .GetComponent<Rigidbody>();
                // move the fish a bit to initialize a collision with the conveyor belt after turning it back on
                rb.AddForce(transform.up * 50, ForceMode.Acceleration);
            }
        }
        else if (!gameManager.IsTaskOn && isSpawnerOn)
        {
            isSpawnerOn = false;
        }

        // Checks the amount of spawned gameobjects in the simulation
        currentAmountOfFish = transform.childCount;
    }

    private IEnumerator SpawnFish()
    {
        // Waits for number of seconds equal to the spawnrate + variation
        yield return new WaitForSeconds(spawnRate + RandomizeSpawnRateVariation());

        if (currentAmountOfFish < maxAmountOfFish && isSpawnerOn)
        {
            // spawn object as a child of the spawner object
            GameObject childGameObject = Instantiate(
                fishPrefab,
                transform.position,
                Random.rotation,
                transform
            );
            childGameObject.name = "FactoryFish" + transform.childCount.ToString();

            FactoryFishState fishState = childGameObject.GetComponent<FactoryFishState>();
            if (fishState != null)
            {
                fishState.currentState = RandomizeFishState();
            }

            // this randomized the size of the fish if enabled
            if (fishSizeVariation)
            {
                float randomSize = RandomizeObjectSize();
                childGameObject.transform.localScale = new Vector3(
                    randomSize,
                    randomSize,
                    randomSize
                );
            }

            if (toggleFishTier)
            {
                int randomValue = Random.Range(1, randomFishStateChance + 1);
                if (randomValue == 1)
                {
                    childGameObject.tag = "Tier1";
                }
                else if (randomValue <= 3)
                {
                    childGameObject.tag = "Tier2";
                }
                else
                {
                    childGameObject.tag = "Tier3";
                }
            }
        }

        StartCoroutine(SpawnFish());
    }

    private float RandomizeObjectSize()
    {
        // the size variation of the fish relative to the parent spawner
        return Random.Range(9, 15);
    }

    private float RandomizeSpawnRateVariation()
    {
        return Random.Range(0.5f, varationInSpawnrate);
    }

    // Generates a number from 1 to our set chance, to determine the chance of different states.
    private FactoryFishState.State RandomizeFishState()
    {
        int randomValue = Random.Range(1, randomFishStateChance + 1);
        FactoryFishState.State state;

        if (randomValue == 1)
        {
            state = FactoryFishState.State.Dead;
        }
        else if (randomValue <= 3)
        {
            state = FactoryFishState.State.Alive;
        }
        else
        {
            state = FactoryFishState.State.Stunned;
        }
        return state;
    }
}
