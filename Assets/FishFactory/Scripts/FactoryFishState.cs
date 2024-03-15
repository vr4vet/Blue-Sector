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
        Dead
    }

    public State currentState;
}


