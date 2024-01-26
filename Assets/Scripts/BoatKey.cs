using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatKey : MonoBehaviour
{

    public bool boatOn;
    public bool canSwitch;
    public bool isParent;
    public newAccel accel;
    public newWheel wheel;
    public GameObject canvas;
    public AudioSource boataud;
    public Collider boatcollider;
    public Collider playercollider;
    public float keySwitchDelay;

    public Collider can_hit_merd;
    public Collider can_hit_merd2;
    public Collider can_hit_merd3;
    public Collider can_hit_merd4;
    public Collider can_hit_merd5;
    public Collider can_hit_merd6;
    public Collider can_hit_land1;
    public Collider can_hit_land2;

    public GameObject brygge_marker;
    public GameObject slakt_marker;
    public GameObject flyt_marker;

    void Start()
    {
        keySwitchDelay = 1.5f;
        boatOn = false;
        canSwitch = true;
        Physics.IgnoreCollision(boatcollider, playercollider);

        can_hit_merd.enabled = true;
        can_hit_merd2.enabled = true;
        can_hit_merd3.enabled = true;
        can_hit_merd4.enabled = true;
        can_hit_merd5.enabled = true;
        can_hit_merd6.enabled = true;
        can_hit_land1.enabled = true;
        can_hit_land2.enabled = true;

        brygge_marker.SetActive(false);
        slakt_marker.SetActive(false);
        flyt_marker.SetActive(false);

    }
    public void TurnOnOff()
    {
        //Checks if the boat can switch and if it is already on
        if (!boatOn && canSwitch)
        {
            StartCoroutine(Foo());
          
            gameObject.transform.parent.Rotate(90, 0, 0, Space.Self);
            
            GetComponent<AudioSource>().enabled = true;
            boataud.enabled = true;
            boatOn = true;
            canvas.SetActive(false);

            can_hit_merd.enabled = true;
            can_hit_merd2.enabled = true;
            can_hit_merd3.enabled = true;
            can_hit_merd4.enabled = true;
            can_hit_merd5.enabled = true;
            can_hit_merd6.enabled = true;
            can_hit_land1.enabled = true;
            can_hit_land2.enabled = true;

            brygge_marker.SetActive(true);
            slakt_marker.SetActive(true);
            flyt_marker.SetActive(true);


        }

        else if (canSwitch)
        {
            accel.source.Stop();
            accel.enabled = false;
            wheel.enabled = false;
            gameObject.transform.parent.Rotate(-90, 0, 0, Space.Self);


            GetComponent<AudioSource>().enabled = false;
            boataud.enabled = true;
            boatOn = false;
            canvas.SetActive(true);
            StartCoroutine(Foo());

            can_hit_merd.enabled = false;
            can_hit_merd2.enabled = false;
            can_hit_merd3.enabled = false;
            can_hit_merd4.enabled = false;
            can_hit_merd5.enabled = false;
            can_hit_merd6.enabled = false;
            can_hit_land1.enabled = false;
            can_hit_land2.enabled = false;

            brygge_marker.SetActive(false);
            slakt_marker.SetActive(false);
            flyt_marker.SetActive(false);
        }
    }
    // Coroutine for delaying the switch and parenting in order to make sure the player is not unparented too early
    public IEnumerator Foo()
    {
        canSwitch = false;
        yield return new WaitForSeconds(keySwitchDelay);  // Wait some seconds
        if (isParent)
        {
            GameObject.FindGameObjectWithTag("Player").transform.parent = null;
            isParent = false;

        }
        else
        {
            GameObject.FindGameObjectWithTag("Player").transform.parent = transform.root;
            isParent = true;
            accel.enabled = true;
            wheel.enabled = true;
        }
    
        canSwitch = true;
    }

}
