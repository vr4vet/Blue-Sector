using UnityEngine;

public class FactoryFishState : MonoBehaviour
{
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
    }

    [SerializeField]
    Material bleedingFish;

    [SerializeField]
    Material aliveFish;

    [SerializeField]
    Material badFish;

    [SerializeField]
    Material stunnedFish;

    void Start() { }

    // The current public state of the fish.
    public State currentState;

    void Update()
    {
        Renderer fishRenderer = gameObject.transform.GetChild(1).GetComponent<Renderer>();

        GetComponent<Renderer>();

        Material[] fishMaterials = fishRenderer.materials;
        switch (currentState)
        {
            case State.Alive:
                // Set first material to bleeding fish
                fishMaterials[0] = aliveFish;
                // Set the updated materials
                fishRenderer.materials = fishMaterials;
                break;

            case State.Stunned:
                // Set first material to stunned fish
                fishMaterials[0] = stunnedFish;
                // Set the updated materials
                fishRenderer.materials = fishMaterials;
                break;

            case State.BadCut:
                // Set first material to badly cut fish
                break;

            case State.Bleeding:
                // Set first material to bleeding fish
                fishMaterials[0] = bleedingFish;
                // Set the updated materials
                fishRenderer.materials = fishMaterials;
                break;

            case State.BadQuality:
                // Set first material to bad fish
                fishMaterials[0] = badFish;
                // Set the updated materials
                fishRenderer.materials = fishMaterials;
                break;
        }
    }

    /// <summary>
    /// When the player cuts the gills of the fish.
    /// </summary>
    public void CutFishGills()
    {
        switch (currentState)
        {
            case State.Alive:
                currentState = State.Bleeding;
                // The player should not cut the gills of a fish that is alive
                GameManager.Instance.PlaySound("incorrect");
                break;

            case State.Stunned:
                currentState = State.Bleeding;
                GameManager.Instance.PlaySound("correct");
                break;

            case State.BadCut:
                currentState = State.Bleeding;
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
        if (currentState == State.Alive || currentState == State.Stunned)
        {
            currentState = State.BadCut;
            GameManager.Instance.PlaySound("incorrect");
        }
    }
}
