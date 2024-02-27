using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryFishSpawner : MonoBehaviour
{
    [HideInInspector]
    enum FishState
    {
        Alive,
        Stunned,
        Dead // TODO: Not used at this state, but can be implemented down the line
    }

    [SerializeField]
    private GameObject fishPrefab;

    [SerializeField]
    private int maxAmountOfFish;

    private int currentAmountOfFish;

    [SerializeField]
    [Range(0.1f, 10)]
    [Tooltip("The rate at which the fish will spawn in seconds")] //FIXME: check if seconds and if range is fine
    private float spawnRate;

    // TODO: private int varationInSpawnrate

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnFish());
    }

    void Update()
    {
        currentAmountOfFish = gameObject.transform.childCount;
    }

    private IEnumerator SpawnFish()
    {
        // When simulation starts, spawn fish
        yield return new WaitForSeconds(spawnRate);

        if (currentAmountOfFish < maxAmountOfFish)
        {
            // spawn object as a child of the spawner object
            GameObject childGameObject = Instantiate(
                fishPrefab,
                transform.position,
                Quaternion.identity,
                transform
            );
            childGameObject.name = "FactoryFish" + gameObject.transform.childCount.ToString();
        }

        StartCoroutine(SpawnFish());
    }
}
