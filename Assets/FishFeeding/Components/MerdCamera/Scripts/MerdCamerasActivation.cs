using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MerdCamerasActivation : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject[] merdCameras;
    void Start()
    {
        merdCameras = GameObject.FindGameObjectsWithTag("MerdCamera");
        DeactivateCameras();
    }

    public void ActivateCameras()
    {
        foreach (GameObject cam in merdCameras)
        {
            cam.GetComponent<Camera>().cullingMask = LayerMask.GetMask("Water", "Fish", "Merd", "UnderwaterShade");
        }
    }

    public void DeactivateCameras()
    {
        foreach (GameObject cam in merdCameras)
        {
            cam.GetComponent<Camera>().cullingMask = LayerMask.GetMask("Nothing");
        }
    }
}
