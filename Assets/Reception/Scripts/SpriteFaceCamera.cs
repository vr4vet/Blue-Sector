using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFaceCamera : MonoBehaviour
{

    private Camera PlayerCamera;
    private float StartTime;
    [SerializeField] private bool Flip = false;
    [SerializeField] private bool LockZRotation = false;
    [SerializeField] private bool LockYRotation = false;
    [SerializeField] private bool LockXRotation = false;

    // Start is called before the first frame update
    void Start()
    {
        StartTime = Time.time;
        PlayerCamera = Camera.main;
        if (Flip)
            GetComponent<SpriteRenderer>().flipX = true;
    }

    private void LateUpdate()
    {
        Vector3 newRotation = PlayerCamera.transform.eulerAngles;
        
        if (LockZRotation)
            newRotation.z = 0;
        if (LockYRotation)
            newRotation.y = 0;
        if (LockXRotation)
            newRotation.x = 0;

        transform.eulerAngles = newRotation;
    }
}
