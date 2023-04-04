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
        fishSystem = gameObject.transform.parent.gameObject;//GameObject.Find("FishSystem");
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
                movement.x *= -1;
                movement.z *= -1;
                
/*                Quaternion target = Quaternion.LookRotation(movement);
                target = Quaternion.RotateTowards(gameObject.transform.rotation, target, 360);
                gameObject.transform.rotation = target;*/
                waitingForReturn = true;
            }
        } else {
            waitingForReturn = false;
        }

        if (!(dead && gameObject.transform.position.y <= bottom))
        {
            gameObject.transform.position += movement * Time.deltaTime;
            RotateFish();
        } 
    }

    bool IsColliding()
    {
        Vector3 fishSystemPosition = fishSystem.transform.position; 
        Vector3 fishPosition = gameObject.transform.position;
        if ((Mathf.Pow(fishPosition.x - fishSystemPosition.x, 2) + Mathf.Pow(fishPosition.z - fishSystemPosition.z, 2) - Mathf.Pow(radius, 2)) >= 0)
        {
            return true;
        }
        return false;
    }

    bool IsAtSurface() => gameObject.transform.position.y >= top;
    bool IsAtBottom() => gameObject.transform.position.y <= bottom;

    void RotateFish()
    {
        movement = new Vector3(directionX, directionY, directionZ);
        Quaternion target;
        if (directionY > 0.3)
        {
            target = Quaternion.LookRotation(new Vector3(directionX, 0.3f, directionZ));
        }
        else if (directionY < -0.3)
        {
            target = Quaternion.LookRotation(new Vector3(directionX, -0.3f, directionZ));
        }
        else
            target = Quaternion.LookRotation(movement);


        //Quaternion target = Quaternion.LookRotation(movement);
        //Quaternion target = Quaternion.LookRotation(directionY > 0.3 ? new Vector3(directionX, 0.3f, directionZ) : movement);
        target = Quaternion.RotateTowards(gameObject.transform.rotation, target, 360);
        gameObject.transform.rotation = target;
    }
    // runs every 3rd second
    void PeriodicUpdates()
    {
        //Vector3 fishPosition = gameObject.transform.position;
        float posY = gameObject.transform.position.y;
        FishSystemScript.FishState state = fishSystemScript.state;

 
        if (IsColliding()) return;  // no modifications to movement if returning from outside boundaries
        if (dead)
        {
            // dead fish continue to sink until hitting bottom
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
                directionY = Random.Range(-0.2f, 0.2f);
            }

            // stay within hunger/full segment (over/under fullnessDivider) of fish cage when not in idle state
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
            directionZ = Random.Range(-swimSpeedHorizontal, swimSpeedHorizontal);

            // Rotates the fish in the right direction
            RotateFish();
/*            movement = new Vector3(directionX, directionY, directionZ);
            Quaternion target;
            if (directionY > 0.3)
            {
                target = Quaternion.LookRotation(new Vector3(directionX, 0.3f, directionZ));
            }
            else if (directionY < -0.3)
            {
                target = Quaternion.LookRotation(new Vector3(directionX, -0.3f, directionZ));
            }
            else
                target = Quaternion.LookRotation(movement);


            //Quaternion target = Quaternion.LookRotation(movement);
            //Quaternion target = Quaternion.LookRotation(directionY > 0.3 ? new Vector3(directionX, 0.3f, directionZ) : movement);
            target = Quaternion.RotateTowards(gameObject.transform.rotation, target, 360);
            gameObject.transform.rotation = target;*/
        }
    }

    // public function allowing fish system to kill starving fish
    public void Kill() => dead = true;
}
