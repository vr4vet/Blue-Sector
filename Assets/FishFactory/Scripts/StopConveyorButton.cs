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
        if (!GameManager.Instance) {
            // for testing purposes where there is no GameManager Instance
            return;
        } else {
            GameManager.Instance.ToggleTaskOn();
        }
    }

    /// <summary>
    /// Toggles the secondary task on and off
    /// </summary>
    public void ToggleSecondaryTaskOn()
    {
        if (!GameManager.Instance) {
            // for testing purposes where there is no GameManager Instance
            return;
        } else {             
            GameManager.Instance.ToggleSecondaryTaskOn();
        }
    }
}
