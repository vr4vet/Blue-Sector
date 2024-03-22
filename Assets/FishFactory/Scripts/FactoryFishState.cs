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
        currentState = State.Bleeding;
    }

    public void BadCut()
    {
        currentState = State.BadCut;
    }
}
