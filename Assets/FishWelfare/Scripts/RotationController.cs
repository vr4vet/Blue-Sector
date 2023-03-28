using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This scripts makes sure the UI on the fish allways faces the player
public class RotationController : MonoBehaviour
{

    [SerializeField] GameObject _object;
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _object.transform.position);
    }
}
