using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairScript : MonoBehaviour
{
    [SerializeField] private TeleportPlayerOnCollide TeleportScript;
    [SerializeField] private Transform Destination;
   void OnTriggerEnter(Collider other){
    if (other.gameObject.tag == "Player"){
        TeleportScript.Destination = Destination;
   }}
}
