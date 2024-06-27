using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopConveyorButton : MonoBehaviour
{
    /// <summary>
    /// Toggles the main task on and off
    /// </summary>
    public void ToggleTaskOn()
    {
        GameManager.Instance.ToggleTaskOn();
    }

    /// <summary>
    /// Toggles the secondary task on and off
    /// </summary>
    public void ToggleSecondaryTaskOn()
    {
        GameManager.Instance.ToggleSecondaryTaskOn();
    }
}
