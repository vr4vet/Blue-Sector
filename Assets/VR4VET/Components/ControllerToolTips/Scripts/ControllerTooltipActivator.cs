using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerTooltipActivator : MonoBehaviour
{
    [SerializeField] private float TriggerSizeFactor = 1.5f;
    private ControllerTooltipManager _controllerTooltipManager;

    // Start is called before the first frame update
    void Start()
    {
        _controllerTooltipManager = GameObject.Find("ControllerToolTipManager").GetComponent<ControllerTooltipManager>();
        BoxCollider collider = gameObject.AddComponent(typeof(BoxCollider)) as BoxCollider;
        collider.isTrigger = true;
        collider.size = transform.parent.GetComponent<MeshFilter>().mesh.bounds.size * TriggerSizeFactor;
        collider.center = transform.parent.GetComponent<MeshFilter>().mesh.bounds.center;
    }


    private int _handsCnt = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Grabber"))
        {
            if (other.transform.parent.name.Equals("LeftController"))
                Debug.Log("Left hand entered");
            else if (other.transform.parent.name.Equals("RightController"))
                Debug.Log("Right hand entered");
        
            if (!_controllerTooltipManager.OculusModelsActive())
                _controllerTooltipManager.SetOculusHandModels();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Equals("Grabber"))
        {
            if (other.transform.parent.name.Equals("LeftController"))
                Debug.Log("Left hand exited");
            else if (other.transform.parent.name.Equals("RightController"))
                Debug.Log("Right hand exited");
        
            if (_controllerTooltipManager.OculusModelsActive())
                _controllerTooltipManager.SetDefaultHandModels();
        }

    }
}
