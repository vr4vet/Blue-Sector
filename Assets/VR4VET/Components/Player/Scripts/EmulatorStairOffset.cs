using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using UnityEngine.XR.Management;

public class EmulatorStairOffset : MonoBehaviour
{
    private bool _hmdExists;
    private Vector3 _basePosition;
    [SerializeField] private Transform Player;
    // Start is called before the first frame update
    void Start()
    {
        if(!Player)
        {
            try
            {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
            }
            catch (UnassignedReferenceException)
            {
                Debug.LogError("Assign the player with a Player tag");
                throw;
            }
        }

        // Get the PlayerController
        Player = Player.Find("PlayerController");
        _hmdExists = XRGeneralSettings.Instance.Manager.activeLoader != null;
        _basePosition = transform.position;
        Debug.Log(_basePosition);
        Debug.Log(transform);
    }

    void Update()
    {
        if(!_hmdExists)
        {
            transform.position = _basePosition + new Vector3(0f, Player.GetComponent<BNGPlayerController>().ElevateCameraHeight+0.5f,0f);
        }
    }
}
