using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class TeleportPlayerOnCollide : MonoBehaviour
{
    public Transform Destination;
    [SerializeField] private PlayerTeleport Teleport;

void OnCollisionEnter(Collision collision){
    foreach(ContactPoint contact in collision.contacts){
        Debug.Log(contact);
        if(contact.otherCollider.gameObject.tag =="Player"){
            Teleport.TeleportPlayerToTransform(Destination);
        }
    }
}
}
