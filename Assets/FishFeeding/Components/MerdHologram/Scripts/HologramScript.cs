using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HologramScript : MonoBehaviour
{
    private GameObject hologramCamera;
    private Camera currentCamera;
    private float holoZ;
    private float holoY;
    private FishSystemScript currentFishSystem;
    private Camera[] cameras;
    private GameObject MerdCameraHost;
    private MerdCameraController MerdCameraController;

    [SerializeField]
    [Tooltip("Offset camera on Z-axis")]
    private float offsetZ = 0;

    [SerializeField]
    [Tooltip("Offset camera on Y-axis")]
    private float offsetY = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        MerdCameraHost = GameObject.Find("MerdCameraHost");
        MerdCameraController = MerdCameraHost.GetComponent<MerdCameraController>();
        cameras = MerdCameraController.Cameras;
        currentCamera = cameras[0];

        currentFishSystem = MerdCameraController.SelectedFishSystem;
        hologramCamera = transform.Find("MerdHologramCamera").gameObject;
        holoZ = (transform.GetComponent<BoxCollider>().size.z) / 2;     // divide by two because we're using radius, not diameter.
        holoY = (transform.GetComponent<BoxCollider>().size.y);  
    }

    // Update is called once per frame
    void Update()
    {
        // getting position between centre and radius (boundary) of active camera
        Vector3 position = currentCamera.transform.localPosition;
        float tempZ = (position.z + offsetZ) * holoZ;
        float tempY = (position.y + offsetY) * holoY;  
        hologramCamera.transform.localPosition = new Vector3(0.0f, tempY / currentFishSystem.height, tempZ / currentFishSystem.radius);
        hologramCamera.transform.localRotation = currentCamera.transform.localRotation;
    }

    public void SetCameraAndFishSystem(Camera camera, FishSystemScript script)
    {
        
        currentCamera = camera;
        currentFishSystem = script;
        Debug.Log("Camera switched");
        Debug.Log(currentFishSystem.radius);

    }


}
