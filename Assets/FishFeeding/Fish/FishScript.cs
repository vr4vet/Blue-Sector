using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FishScript : MonoBehaviour
{
    private Vector3 direction;
    private Vector3 movement;
    private float radiusSquared;
    private float fullnessDivider;
    private float top;
    private float bottom;
    private bool dead;
    private bool randomFish;
    private float swimSpeedVertical;
    private float swimSpeedHorizontal;

    private Animation fishAnimation;
    private GameObject fishSystem;
    private Vector3 fishSystemPosition;
    private FishSystemScript fishSystemScript;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(PeriodicUpdates), Random.Range(0.0f, 5.0f), 3.0f);
        fishSystem = gameObject.transform.parent.gameObject;
        fishSystemPosition = fishSystem.transform.position;
        fishSystemScript = fishSystem.GetComponent<FishSystemScript>();
        fishAnimation = gameObject.transform.GetChild(0).GetComponent<Animation>();

        radiusSquared = fishSystemScript.radius * fishSystemScript.radius;
        top = (fishSystem.transform.position.y + (fishSystemScript.height / 2));  // top of merd/water surface
        bottom = (fishSystem.transform.position.y - (fishSystemScript.height / 2)); // bottom of merd
        fullnessDivider = bottom + ((top - bottom) * fishSystemScript.fullnessDivider); // border between hungry and full fish
        swimSpeedVertical = fishSystemScript.swimSpeedVertical;
        swimSpeedHorizontal = fishSystemScript.swimSpeedHorizontal;
        dead = false;
    }

    /*
     * this bool is needed to override fish movement when returning after leaving fish system (fish cage) boundaries, to ensure it does not 
     * move about until it has returned safely
    */
    private bool waitingForReturn = false;
    // Update is called once per frame
    void Update()
    {

        var transform = gameObject.transform;
        var fishPosition = transform.position;
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

    // checks if fish is within fish cage boundaries (horisontally)
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
        fishSystemPosition = fishSystem.transform.position;

        Vector3 fishPosition = gameObject.transform.position;
        float posY = fishPosition.y;

        FishSystemScript.FishState state = fishSystemScript.state;


        if (IsColliding(fishPosition)) return;  // no modifications to movement if returning from outside boundaries
        if (dead)
        {
            // dead fish continue to sink until hitting bottom, in the speed of swimSpeedVertical field's value
            if (!IsAtBottom())
            {
                movement = new Vector3(direction.x, -swimSpeedVertical, direction.z);
            }
        }
        else
        {
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

            /* stay within hunger/full segment (over/under fullnessDivider) of fish cage when not in idle state and not a randomFish.
             * hungry fish swim high, as they approach the food comming from above,
             * full fish swim lower, as they do not approach the food.
            */
            if (state != FishSystemScript.FishState.Idle && !randomFish)
            {
                if ((state == FishSystemScript.FishState.Hungry || state == FishSystemScript.FishState.Dying) && posY < fullnessDivider)
                {
                    direction.y = swimSpeedVertical;
                }
                else if (state == FishSystemScript.FishState.Full && posY > fullnessDivider)
                {
                    direction.y = -swimSpeedVertical;
                }
            }

            // new random direction horisontally
            direction.x = Random.Range(-swimSpeedHorizontal, swimSpeedHorizontal);
            direction.z = direction.x < 0 ? 1 + direction.x : 1 - direction.x;  //  x + z should be equal to 1 or -1 always, to ensure consistent swimming speed
            direction.z = Random.Range(0.0f, 1.0f) < 0.5 ? direction.z * -1 : direction.z; // randomly make negative to prevent repetitive patterns  
            RotateFish(); // Rotates the fish in the right direction

        }
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

    // public function telling this fish to swim randomly and not get hungry etc. Make the fish stream a bit less monotone.
    public void SetRandomFish() => randomFish = true;
}
