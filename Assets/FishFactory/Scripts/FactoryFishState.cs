using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryFishState : MonoBehaviour
{
    public enum State
    {
        Alive,
        Stunned,
        Bleeding,
        Dead,
        BadCut
    }

    public State currentState;

    public void CutGills()
    {
        switch (currentState)
        {
            case State.Alive:
                currentState = State.Bleeding; // The player should get negative points for cutting the gills of a not stunned fish
                GameManager.instance.Score -= 1;
                break;

            case State.Stunned:
                currentState = State.Bleeding;
                GameManager.instance.Score += 1;
                break;

            case State.BadCut:
                currentState = State.Bleeding; // The player should not recieve points for cutting the gills of a fish that has already been cut incorrectly
                break;

            default:
                // Possible states: Bleeding, Dead
                // The player should not be able to cut the gills of a fish that is dead/bad or already bleeding
                break;
        }
    }

    public void BadCut()
    {
        // To make the cutting process more forgiving, we will not penalize the player for a bad cut if the fish is already cut correctly
        // The player will only be penalized for a bad cut if the fish is alive or has not been cut yet
        if (
            currentState == State.Bleeding
            || currentState == State.Dead
            || currentState == State.BadCut
        )
        {
            return;
        }
        currentState = State.BadCut;
        GameManager.instance.Score -= 1;
    }
}
