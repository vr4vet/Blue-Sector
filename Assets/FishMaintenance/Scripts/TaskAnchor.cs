using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskAnchor : MonoBehaviour
{
    public List<Task.Step> Steps = new();
    public bool RequiresCageArrows = true;
    public bool RequiresEquipmentArrow = false;
    public bool RequiresBackToBoatArrow = false;
}
