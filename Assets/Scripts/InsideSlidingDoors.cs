using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideSlidingDoors : MonoBehaviour
{
 public bool openDoor = false;
    public Animator anim;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
    }
    //Opens the sliding door
    public void CloseOpen()
    {
        if (!openDoor)
        {
            anim.Play("InsideOpenSlidingDoor 1");
            openDoor = true;
            StartCoroutine("CloseDoor");
        }
    }
    //Closes the slidingdoor after 7 seconds
    IEnumerator CloseDoor()
    {
        yield return new WaitForSeconds(7);
        anim.Play("InsideCloseSlidingDoor 1");
        openDoor = false;
    }

}