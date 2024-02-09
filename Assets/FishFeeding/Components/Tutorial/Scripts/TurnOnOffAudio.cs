using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class TurnOnOffAudio : MonoBehaviour
{
    [SerializeField] private GameObject PlayerController;
    public bool turnOnAudio;
    private Vector3 oldPos;
    private Vector3 newPos;

    // Start is called before the first frame update
    void Start()
    {
        oldPos = newPos = PlayerController.transform.position;

        PlayerTeleport.OnAfterTeleport += UpdateTeleportationPosition;

        foreach (var playAudio in GetComponentsInChildren<PlayAudio>()) {
            playAudio.AudioBoxColliderTriggered.AddListener(checkValidAudioTrigger);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!turnOnAudio) 
        { 
            TurnOffAudio();
        } 
        else 
        { 
            TurnOnAudio(); 
        }
        
    }

    public void TurnOffAudio() 
    {
        foreach (var audioSource in GetComponentsInChildren<AudioSource>()) {
            audioSource.Stop();
        }

        foreach (var playAudio in GetComponentsInChildren<PlayAudio>()) {
            playAudio.ResetTrigger();
            playAudio.TurnOnTutorialAudio = false;
        }
    }

    public void TurnOnAudio() 
    {
        foreach (var playAudio in GetComponentsInChildren<PlayAudio>()) {
            playAudio.TurnOnTutorialAudio = true;
        }
    }

    private void UpdateTeleportationPosition() 
    {
        oldPos = newPos;
        newPos = PlayerController.transform.position;
    }

    /// <summary>
    /// Checks if the old position after teleporting was outside the collider. 
    /// </summary>
    private void checkValidAudioTrigger(PlayAudio playerAudio) 
    {
        BoxCollider collider = playerAudio.GetComponent<BoxCollider>();
        if (IsInside(collider, newPos) && !IsInside(collider, oldPos)) 
        {
            playerAudio.validTeleportTrigger = true;
        }
        else 
        {
            playerAudio.validTeleportTrigger = false;
        }
    }

    public static bool IsInside(Collider c, Vector3 point) {
        Vector3 closest = c.ClosestPoint(point);
        // Because closest=point if point is inside
        return closest == point;
    }
}
