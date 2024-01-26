using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoors : MonoBehaviour
{
 public bool openDoor = false;
    public Animator anim;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
    }

    // Opens the sliding doors
    public void CloseOpen()
    {
        if (!openDoor)
        {
            anim.Play("OpenSlidingDoor");
            openDoor = true;
            StartCoroutine("CloseDoor");
        }
    }
    // Closes the sliding doors after 7 seconds
    IEnumerator CloseDoor()
    {
        yield return new WaitForSeconds(7);
        anim.Play("CloseSlidingDoor");
        openDoor = false;
    }
 
}