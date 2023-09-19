using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class StairScript : MonoBehaviour
{
    [SerializeField] private TeleportPlayerOnEnter[] TeleportScript;
    [SerializeField] private Transform Destination;
   void OnTriggerEnter(Collider other){
    if (other.gameObject.tag == "Player"){
        foreach(TeleportPlayerOnEnter Script in TeleportScript){
        Script.TeleportDestination = Destination;
        }
   }}
}
