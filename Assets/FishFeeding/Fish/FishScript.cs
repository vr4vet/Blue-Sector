using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FishScript : MonoBehaviour
{
    private float directionX;
    private float directionY;
    private float directionZ;
    private Vector3 movement;
    private float radius;
    private float fullnessDivider;
    private float top;
    private float bottom;
    private bool dead;
    private float swimSpeedVertical;
    private float swimSpeedHorizontal;
    public Status status;

    private Animation fishAnimation;
    private GameObject fishSystem;
    private FishSystemScript fishSystemScript;

    public enum Status
    {
        Idle,
        Full,
        Hungry,
        Dead
    }
    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(PeriodicUpdates), Random.Range(0.0f, 5.0f), 3.0f);
        fishSystem = gameObject.transform.parent.gameObject;
        fishSystemScript = fishSystem.GetComponent<FishSystemScript>();
        fishAnimation = gameObject.transform.GetChild(0).GetComponent<Animation>();
        
        radius = fishSystemScript.radius;
        top = (fishSystem.transform.position.y + (fishSystemScript.height / 2)) - 1.5f;  // top of merd/water surface
        bottom = (fishSystem.transform.position.y - (fishSystemScript.height / 2)) + 1.5f; // bottom of merd
        fullnessDivider = bottom + (((bottom - top) * (fishSystemScript.fullnessDivider)) * -1); // border between hungry and full fish
        swimSpeedVertical = fishSystemScript.swimSpeedVertical;
        swimSpeedHorizontal = fishSystemScript.swimSpeedHorizontal;
        dead = false;
        status = Status.Full;
    }

    /*
     * this bool is needed to override fish movement when returning after leaving fish system (fish cage) boundaries, to ensure it does not 
     * move about until it has returned safely
    */
    private bool waitingForReturn = false;
    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            fishAnimation.Stop();
        }

        if (IsColliding())
        {
            if (!waitingForReturn)
            {
                directionX *= -1;   // modify vector3 for 180 degrees rotation (towards fish cage)
                directionZ *= -1;
                RotateFish();   // perform the rotation
                waitingForReturn = true;    // stays true (locks movement) until fish has returned
            }
        } else {
            waitingForReturn = false;
        }

        // move fish using the current movement vector, both when dead or living, as the fish should sink until hitting the bottom.
        if (!(dead && gameObject.transform.position.y <= bottom))
        {
            gameObject.transform.position += movement * Time.deltaTime;
        }

    }

    // checks if fish is within fish cage boundaries (horisontally)
    bool IsColliding()
    {
        Vector3 fishSystemPosition = fishSystem.transform.position; 
        Vector3 fishPosition = gameObject.transform.position;

        // check if x and z position is within fish cage (cross section of the fish system, a circle)
        if ((Mathf.Pow(fishPosition.x - fishSystemPosition.x, 2) + Mathf.Pow(fishPosition.z - fishSystemPosition.z, 2) - Mathf.Pow(radius, 2)) >= 0)
        {
            return true;
        }
        return false;
    }

    // functions for checking if fish is too high or low (outside boundaries)
    bool IsAtSurface() => gameObject.transform.position.y >= top;
    bool IsAtBottom() => gameObject.transform.position.y <= bottom;

    // rotates fish towards its current destination
    void RotateFish()
    {
        movement = new Vector3(directionX, directionY, directionZ);
        if (movement != Vector3.zero)   // fish spawn with a zero vector, which can not be used for rotation.
        {
            // if the upwards/downwards direction is steeper than rotationLimit (fish is angled too vertically), set to limit.
            float rotationLimit = 0.2f;
            if (directionY > rotationLimit)
                movement.y = rotationLimit;
            else if (directionY < -rotationLimit)
                movement.y = -rotationLimit;

            // perform the actual rotation
            Quaternion target = Quaternion.LookRotation(movement);
            target = Quaternion.RotateTowards(gameObject.transform.rotation, target, 360);
            gameObject.transform.rotation = target;
        }
    }
    /*
     * runs every 3rd second
     * this function handles all main movement logic
     */
    void PeriodicUpdates()
    {
        //Vector3 fishPosition = gameObject.transform.position;
        float posY = gameObject.transform.position.y;
        FishSystemScript.FishState state = fishSystemScript.state;

 
        if (IsColliding()) return;  // no modifications to movement if returning from outside boundaries
        if (dead)
        {
            // dead fish continue to sink until hitting bottom, in the speed of swimSpeedVertical field's value
            if (!IsAtBottom())
            {
                movement = new Vector3(directionX, -swimSpeedVertical, directionZ);
            }
        }
        else
        {
            // move down/up if hitting upper/lower boundaries of fish cage, otherwise pick some random fairly horisontal direction
            if (IsAtSurface())
            {
                directionY = -swimSpeedVertical;
            }
            else if (IsAtBottom())
            {
                directionY = swimSpeedVertical;
            }
            else
            {
                directionY = Random.Range(-0.1f, 0.1f);
            }

            /* stay within hunger/full segment (over/under fullnessDivider) of fish cage when not in idle state.
             * hungry fish swim high, as they approach the food comming from above,
             * full fish swim lower, as they do not approach the food.
            */
            if (state != FishSystemScript.FishState.Idle)    
            {
                if ((state == FishSystemScript.FishState.Hungry || state == FishSystemScript.FishState.Dying) && posY < fullnessDivider)
                {
                    directionY = swimSpeedVertical;
                }
                else if (state == FishSystemScript.FishState.Full && posY > fullnessDivider)
                {
                    directionY = -swimSpeedVertical;
                }
            } 

            // new random direction horisontally
            directionX = Random.Range(-swimSpeedHorizontal, swimSpeedHorizontal);
            directionZ = directionX < 0 ? 1 + directionX : 1 - directionX;  //  x + z should be equal to 1 or -1 always, to ensure consistent swimming speed
            directionZ = Random.Range(0.0f, 1.0f) < 0.5 ? directionZ * -1 : directionZ; // randomly make negative to prevent repetitive patterns  
            RotateFish(); // Rotates the fish in the right direction

        }
    }

    // public function allowing fish system to kill starving fish
    public void Kill() => dead = true;
}
