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

    // The current public state of the fish.
    public State currentState;

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
                // The player can cut the gills of a fish that has already been cut incorrectly
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
        if (
            currentState == State.Bleeding
            || currentState == State.BadQuality
            || currentState == State.BadCut
        )
        {
            return;
        }
        currentState = State.BadCut;
        GameManager.Instance.PlaySound("incorrect");
    }
}
