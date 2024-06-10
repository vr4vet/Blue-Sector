using System.Collections;
using UnityEngine;

public class FactoryFishState : MonoBehaviour
{
    // ------------ Public Variables ------------

    // The possible states of the fish.
    public enum State
    {
        Alive,
        Stunned,
        Bleeding,
        BadQuality,
        BadCut,
        GuttingSuccess,
        GuttingIncomplete,
        GuttingFailure,
        Tier1,
        Tier2,
        Tier3,
    }

    // The current public state of the fish.
    public State CurrentState;

    // ------------ Editor Variables ------------

    [SerializeField]
    private Material _bleedingFish;

    // ------------ Unity Functions ------------

    void Awake()
    {
        _bleedingFish = Resources.Load<Material>("Materials/Fish/salmonBleeding");
    }

    void Start()
    {
        // If the fish is alive, it should start moving
        if (CurrentState == State.Alive)
        {
            StartCoroutine(AliveFish());
        }
    }

    // ------------ Public Functions ------------

    /// <summary>
    /// When the player cuts the gills of the fish.
    /// </summary>
    public void CutFishGills()
    {
        Renderer fishMaterial = gameObject.transform.GetChild(0).GetComponent<Renderer>();

        switch (CurrentState)
        {
            case State.Alive:
                CurrentState = State.Bleeding;
                // The player should not cut the gills of a fish that is alive
                GameManager.Instance.PlaySound("incorrect");
                break;

            case State.Stunned:
                CurrentState = State.Bleeding;
                fishMaterial.material = _bleedingFish;
                GameManager.Instance.PlaySound("correct");
                break;

            case State.BadCut:
                CurrentState = State.Bleeding;
                fishMaterial.material = _bleedingFish;
                // The player can cut the gills of a fish that has already been cut incorrectly, fixing the mistake.
                GameManager.Instance.PlaySound("correct");
                break;

            case State.Bleeding:
                // The player should not be able to cut the gills of a fish that is already bleeding
                break;

            case State.BadQuality:
                // The player should not cut the gills of a fish that is bad
                GameManager.Instance.PlaySound("incorrect");
                break;
        }
    }

    /// <summary>
    /// When the player cuts the body of the fish.
    /// </summary>
    public void CutFishBody()
    {
        // To make the cutting process more forgiving, we will not penalize the player for a bad cut if the fish is already cut correctly
        // The player will only be penalized for a bad cut if the fish is alive or has not been cut yet
        if (CurrentState == State.Alive || CurrentState == State.Stunned)
        {
            CurrentState = State.BadCut;
            GameManager.Instance.PlaySound("incorrect");
        }
    }

    // ------------ Private Functions ------------

    /// <summary>
    /// Coroutine that makes the fish move while alive.
    /// </summary>
    /// <returns> The fish will move in a random direction </returns>
    private IEnumerator AliveFish()
    {
        // Every time the fish "gathers strength" it will shake 4 times
        for (int i = 0; i < 4; i++)
        {
            // Each repetition will have a random delay between 0.5 and 1 seconds
            yield return new WaitForSeconds(Random.Range(0.5f, 1f));

            // The fish head and tail rigidbodies
            Rigidbody head = transform.GetChild(2).transform.GetChild(0).GetComponent<Rigidbody>();
            Rigidbody tail = transform
                .GetChild(2)
                .transform.GetChild(0)
                .transform.GetChild(0)
                .transform.GetChild(0)
                .transform.GetChild(0)
                .transform.GetChild(0)
                .transform.GetChild(0)
                .transform.GetChild(0)
                .transform.GetChild(0)
                .GetComponent<Rigidbody>();

            // Makes the fish head move upwards
            head.AddForce(Vector3.up * 800, ForceMode.Force);

            // Makes the fish tail move upwards
            tail.AddForce(Vector3.up * 500, ForceMode.Force);

            // If the fish is no longer alive, stop the coroutine
            if (CurrentState != State.Alive)
            {
                yield break;
            }
        }

        // Wait for 2-8 seconds before repeating
        yield return new WaitForSeconds(Random.Range(1, 3));
        StartCoroutine(AliveFish());
    }
}
