using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes sprites face the camera. Credits to https://gamedevbeginner.com/billboards-in-unity-and-how-to-make-your-own/
/// </summary>
public class SpriteFaceCamera : MonoBehaviour
{

    private Camera PlayerCamera;
    [SerializeField] private bool Flip = false;

    // Start is called before the first frame update
    void Start()
    {
        PlayerCamera = Camera.main;
        if (Flip)
            GetComponent<SpriteRenderer>().flipX = true;
    }

    private void LateUpdate()
    {
        transform.LookAt(PlayerCamera.transform);
        transform.Rotate(0, 180, 0);
    }
}
