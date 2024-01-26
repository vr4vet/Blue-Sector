using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenClose : MonoBehaviour {
    public bool openDoor = false;
    public Animator anim;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
    }
    // Opens the door
    public void CloseOpen()
    {
        if (!openDoor)
        {
            anim.Play("Door_open");
            openDoor = true;
            StartCoroutine("CloseDoor");
        }
    }
    // Closes the door in 7 second
    IEnumerator CloseDoor()
    {
        yield return new WaitForSeconds(7);
        anim.Play("Door_Close");
        openDoor = false;
    }
}
