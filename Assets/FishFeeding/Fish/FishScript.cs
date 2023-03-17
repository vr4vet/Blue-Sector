using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishScript : MonoBehaviour
{
    private float randX;
    private float randY;
    private float randZ;
    private Vector3 movement;
    private float radius;
    public float fullness;
    private float fullnessDivider;
    private float fullnessLimit;
    private float hungerRate;
    private float top;
    private float bottom;
    private bool dead;
    private float swimSpeedVertical;
    private float swimSpeedHorizontal;
    public Status status;


    public GameObject fishSystem;
    public FishSystemScript fishSystemScript;

    public enum Status
    {
        Full,
        Hungry,
        Dead
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
        InvokeRepeating(nameof(PeriodicUpdates), Random.Range(0.0f, 5.0f), 3.0f);
        //fishSystem = GameObject.FindGameObjectWithTag("Fish System");
        fishSystem = GameObject.Find("FishSystem");
        fishSystemScript = fishSystem.GetComponent<FishSystemScript>();
        radius = fishSystemScript.radius;
        fullness = 100.0f; //Random.Range(0.0f, 100.0f);
        top = (fishSystem.transform.position.y + (fishSystemScript.height / 2)) - 1;  // top of merd/water surface
        bottom = (fishSystem.transform.position.y - (fishSystemScript.height / 2)) + 1; // bottom of merd
        fullnessDivider = bottom + (((bottom - top) * (fishSystemScript.fullnessDivider)) * -1); // border between hungry and full fish
        hungerRate = fishSystemScript.hungerRate;
        swimSpeedVertical = fishSystemScript.swimSpeedVertical;
        swimSpeedHorizontal = fishSystemScript.swimSpeedHorizontal;
        fullnessLimit = fishSystemScript.fullnessLimit;
        dead = false;
        status = Status.Full;
        //Debug.Log(fishSystemScript.height);
        //Debug.Log(top);
        //Debug.Log(fishSystemScript.swimSpeedVertical);
    }

    private bool waitingForReturn = false;

    // Update is called once per frame
    void Update()
    {
        /*Debug.Log(fullness);*/
        if (IsColliding())
        {
            if (!waitingForReturn)
            {
                //movement = new Vector3(0 - gameObject.transform.position.x, 0 - gameObject.transform.position.y, 0 - gameObject.transform.position.z);
                movement.x *= -1;
                movement.z *= -1;
                Quaternion target = Quaternion.LookRotation(movement);
                target = Quaternion.RotateTowards(gameObject.transform.rotation, target, 360);
                gameObject.transform.rotation = target;
                waitingForReturn = true;
            }
        } else
        {
            waitingForReturn = false;
        }

        if (!(dead && gameObject.transform.position.y <= bottom))
        {
            gameObject.transform.position += movement * Time.deltaTime;
        }  


    }

    bool IsColliding()
    {
        if ((Mathf.Pow(gameObject.transform.position.x, 2) + Mathf.Pow(gameObject.transform.position.z, 2) - Mathf.Pow(radius, 2)) >= 0)
        {
            return true;
        }
        return false;
    }

    // runs every 3rd second
    void PeriodicUpdates()
    {
        float posY = gameObject.transform.position.y;

        

        if (IsColliding())
        {
            return;
        }
        
        if (fullness >= 0)
        {
            fullness -= hungerRate;
        } else if (fullness < 0)
        {
            dead = true;
            status = Status.Dead;
        }

        /*Debug.Log(fullness);
        Debug.Log(posY);*/

        if (dead)
        {
            if (posY > bottom)
            {
                movement = new Vector3(randX, -swimSpeedVertical, randZ);
            }
        }
        else
        {
            if (posY >= top)
            {
                randY = -swimSpeedVertical;
            }
            else if (posY <= bottom)
            {
                randY = swimSpeedVertical;
            }
            else if (fullness < fullnessLimit && posY < fullnessDivider)
            {
                randY = swimSpeedVertical;
                status = Status.Hungry;
            }
            else if (fullness >= fullnessLimit && posY > fullnessDivider)
            {
                randY = -swimSpeedVertical;
                status = Status.Full;
            }

            // Rotates the fish in the right direction
            randX = Random.Range(-swimSpeedHorizontal, swimSpeedHorizontal);
            randZ = Random.Range(-swimSpeedHorizontal, swimSpeedHorizontal);
            //randY = Random.Range(-0.3f, 0.3f);

            movement = new Vector3(randX, randY, randZ);
            Quaternion target = Quaternion.LookRotation(movement);
            target = Quaternion.RotateTowards(gameObject.transform.rotation, target, 360);
            gameObject.transform.rotation = target;
        }
    }
}
