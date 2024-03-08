using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerdFish : MonoBehaviour
{
    private float rotationSpeed = 3f;
    private float moveSpeed = 1f;
    private bool reachedPoint;
    private Vector3 targetPos;

    private Vector3 fishPosition;
    private Vector3 direction;
    private Vector3 movement;
    private float radiusSquared;
    private float top;
    private float bottom;
    private bool dead = false;

    private float swimSpeedVertical = 1f;
    private float swimSpeedHorizontal = 1f;
    private Animation fishAnimation;
    private GameObject fishSystem;
    private Vector3 fishSystemPosition;
    private MerdFishSystem fishSystemScript;

    public GameObject FishSystem { get => fishSystem; set => fishSystem = value; }
    // Start is called before the first frame update
    void Start()
    {
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
        top = (fishSystem.transform.position.y + (fishSystemScript.Height / 2));  // top of merd/water surface
        // targetPos = fishSystemScript.JumpTargetPosition();
        bottom = (fishSystem.transform.position.y - (fishSystemScript.Height / 2)); // bottom of merd

    }

    /*
     * this bool is needed to override fish movement when returning after leaving fish system (fish cage) boundaries, to ensure it does not 
     * move about until it has returned safely
    */

    private bool waitingForReturn = false;
    private bool readytoTurn = false;
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
    bool IsAtSurface() => gameObject.transform.position.y >= top - 0.5f;
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
        const float rotationLimit = 0.2f;
        lookRotation.y = Mathf.Clamp(lookRotation.y, -rotationLimit, rotationLimit);

        // perform the actual rotation
        Quaternion target = Quaternion.LookRotation(lookRotation);
        target = Quaternion.RotateTowards(gameObject.transform.rotation, target, 360);
        gameObject.transform.rotation = target;
    }
    /*
     * runs every 3rd second
     * this function handles all main movement logic
     */
    void PeriodicUpdates()
    {
        fishPosition = gameObject.transform.position;
        float posY = fishPosition.y;


        // no modifications to movement if returning from outside boundaries or still swimming downwards after eating
        if (IsColliding(fishPosition)) return;
        if (dead)
        {
            // dead fish continue to sink until hitting bottom, in the speed of swimSpeedVertical field's value
            if (!IsAtBottom())
            {
                movement = new Vector3(direction.x, -swimSpeedVertical, direction.z);
            }
        }


        // move down/up if hitting upper/lower boundaries of fish cage, otherwise pick some random fairly horisontal direction
        if (IsAtSurface())
        {
            direction.y = -swimSpeedVertical;
        }
        else if (IsAtBottom())
        {
            direction.y = swimSpeedVertical;
        }
        else
        {
            direction.y = Random.Range(-0.1f, 0.1f);
        }
        if (gameObject.transform.position.y >= 8f)
        {
            direction.y = -swimSpeedVertical * 7;
            direction.x = -direction.x;
            direction.z = -direction.z;
        }

        if (gameObject.transform.position.y <= top)
        {
            direction.y = swimSpeedVertical;
            direction.x = Random.Range(-swimSpeedHorizontal, swimSpeedHorizontal);
            direction.z = direction.x < 0 ? 1 + direction.x : 1 - direction.x;  //  x + z should be equal to 1 or -1 always, to ensure consistent swimming speed
            direction.z = Random.Range(0.0f, 1.0f) < 0.5 ? direction.z * -1 : direction.z; // randomly make negative to prevent repetitive patterns  
        }

        // new random direction horisontally

        RotateFish(); // Rotates the fish in the right direction


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
            // Instantiate(SplashIn)
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