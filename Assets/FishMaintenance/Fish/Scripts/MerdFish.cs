using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerdFish : MonoBehaviour
{

    private Vector3 fishPosition;
    private Vector3 direction;
    private Vector3 movement;
    private float radiusSquared;
    private float top;
    private float bottom;
    private bool dead = false;

    private float swimSpeedVertical = 0.5f;
    private float swimSpeedHorizontal = 1f;
    private Animation fishAnimation;
    private GameObject fishSystem;
    private Vector3 fishSystemPosition;
    private MerdFishSystem fishSystemScript;
    private bool jumpingFish = false;
    public GameObject FishSystem { get => fishSystem; set => fishSystem = value; }
    public bool JumpingFish { get => jumpingFish; set => jumpingFish = value; }
    // Start is called before the first frame update
    void Start()
    {
        // if (Random.Range(-0.3f, 1f) < 0)
        // {
        //     jumpingFish = true;
        //     swimSpeedVertical = 1f;
        // }
        // unique seed per fish for rng, and invoke PeriodicUpdates() with random offset
        Random.InitState(GetInstanceID());
        InvokeRepeating(nameof(PeriodicUpdates), Random.Range(0.0f, 5.0f), 3.0f);

        // get properties of parent fish cage
        fishSystem = gameObject.transform.parent.gameObject;
        fishSystemPosition = fishSystem.transform.position;
        fishSystemScript = fishSystem.GetComponent<MerdFishSystem>();
        fishAnimation = gameObject.transform.GetChild(0).GetComponent<Animation>();
        fishPosition = transform.position;
        radiusSquared = fishSystemScript.Radius * fishSystemScript.Radius;
        top = (fishSystem.transform.position.y);  // top of merd/water surface
        // targetPos = fishSystemScript.JumpTargetPosition();
        bottom = (fishSystem.transform.position.y - (fishSystemScript.Height / 2)); // bottom of merd

    }

    /*
     * this bool is needed to override fish movement when returning after leaving fish system (fish cage) boundaries, to ensure it does not 
     * move about until it has returned safely
    */

    private bool waitingForReturn = false;

    // Update is called once per frame
    void Update()
    {


        //     reachedPoint = Vector3.Distance(targetPos, transform.position) == 0;
        //     Vector3 posOffset = new Vector3(0, 0, 0);
        //     posOffset = new Vector3(targetPos.x, targetPos.y + 6f, targetPos.z);
        //     var rotation = Quaternion.LookRotation(posOffset - transform.position);
        //     transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);


        //     if (!reachedPoint)
        //     {
        //         transform.position = new Vector3(targetPos.x, -targetPos.y + 3f, targetPos.z);
        //         if (readytoTurn)
        //         {
        //             Invoke("FallSpeed", 0.2f);
        //             targetPos = fishPosition;
        //             reachedPoint = true;
        //             readytoTurn = false;
        //         }
        //         // if (target)
        //         // {
        //         //     float dist = ;
        //         // }

        //     }
        //     else
        //     {
        //         transform.position += transform.forward * moveSpeed * Time.deltaTime;
        //         readytoTurn = true;
        //     }
        // }
        // void FallSpeed()
        // {
        //     moveSpeed *= 3;
        // }


        fishPosition = gameObject.transform.position;
        if (IsColliding(fishPosition))
        {
            if (!waitingForReturn)
            {
                // modify vector3 for 180 degrees rotation (towards fish cage)
                direction = Vector3.Scale(direction, new Vector3(-1, 1, -1));
                RotateFish();   // perform the rotation
                waitingForReturn = true;    // stays true (locks movement) until fish has returned
            }
        }
        else
        {
            waitingForReturn = false;
        }

        // move fish using the current movement vector, both when dead or living, as the fish should sink until hitting the bottom.
        if (!dead || fishPosition.y > bottom)
        {
            transform.position = fishPosition + movement * Time.deltaTime;
        }
    }




    // checks if fish is within fish cage boundaries(horisontally)
    bool IsColliding(Vector3 fishPosition)
    {
        // Get the x,z components of the fish position;
        var maskedPosition = Vector3.Scale(fishPosition - fishSystemPosition, Vector3.right + Vector3.forward);
        // check if x and z position is within fish cage (cross section of the fish system, a circle)
        return maskedPosition.sqrMagnitude >= radiusSquared;
    }

    // functions for checking if fish is too high or low (outside boundaries)
    bool IsAtSurface() => gameObject.transform.position.y >= top - 0.3f;
    bool IsAtBottom() => gameObject.transform.position.y <= bottom + 0.5f;

    // check if fish has reached its destination when swimming downwards when full


    // rotates fish towards its current destination
    void RotateFish()
    {
        Vector3 lookRotation = movement = direction;
        if (lookRotation == Vector3.zero)   // fish spawn with a zero vector, which can not be used for rotation.
        {
            return;
        }

        // if the upwards/downwards direction is steeper than rotationLimit (fish is angled too vertically), set to limit.
        const float rotationLimit = 0.3f;
        lookRotation.y = Mathf.Clamp(lookRotation.y, -rotationLimit, rotationLimit);

        // perform the actual rotation
        Quaternion target = Quaternion.LookRotation(lookRotation);
        // target = Quaternion.RotateTowards(gameObject.transform.rotation, target, 360);
        target = Quaternion.Slerp(transform.rotation, target, 360);
        gameObject.transform.rotation = target;
    }
    /*
     * runs every 3rd second
     * this function handles all main movement logic
     */
    void PeriodicUpdates()
    {
        transform.position += transform.forward * 3f * Time.deltaTime;
        fishPosition = transform.position;

        if (IsColliding(fishPosition) || (dead && !IsAtBottom()))
        {
            movement = new Vector3(direction.x, -swimSpeedVertical, direction.z);
        }
        if (IsAtSurface())
        {
            direction.y = -swimSpeedVertical;
        }
        else if (IsAtBottom())
        {
            direction.y = swimSpeedVertical;
        }


        // else if (fishPosition.y > top && jumpingFish)
        // {
        //     direction.y = -2f * swimSpeedVertical;
        //     direction.x = -direction.x;
        //     direction.z = -direction.x;
        // }
        else if (fishPosition.y <= top)
        {
            direction.y = swimSpeedVertical;
            float horizontalSpeed = Random.Range(-swimSpeedHorizontal, swimSpeedHorizontal);
            direction.x = horizontalSpeed;
            direction.z = Mathf.Sign(horizontalSpeed) * (1 - Mathf.Abs(horizontalSpeed));
            direction.z *= Random.Range(0.0f, 1.0f) < 0.5f ? -1 : 1;
        }
        else
        {
            direction.y = Random.Range(-0.1f, 0.1f);
        }


        RotateFish();
    }

    public void SetTarget(Vector3 targetPos)
    {
        transform.position = targetPos;

        jumpingFish = false;

        RotateFish();
    }

    // public function allowing fish system to kill starving fish
    public void Kill()
    {
        dead = true;
        fishAnimation.Stop();
    }

    // public function allowing fish system to kill starving fish
    public void Revive()
    {
        dead = false;
        fishAnimation.Play();
        float posY = Random.Range(bottom + 1, top - 1);
        transform.position = new Vector3(transform.position.x, posY, transform.position.z);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("water"))
        {

        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("water"))
        {
            // Instantiate(SplashOut)
        }
    }

}