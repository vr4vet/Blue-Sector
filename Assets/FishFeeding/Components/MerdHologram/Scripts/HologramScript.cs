using System.Collections;
using System.Collections.Generic;
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
        float tempZ = position.z * holoZ;
        float tempY = (position.y + 1) * holoY;     // +1 on y-axis because merd cam's neutral position is (0, -1, 0)
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
