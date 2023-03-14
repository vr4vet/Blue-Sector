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
    private float fullness;
    private float fullnessDivider;
    private float hungerRate;
    private float height;
    private bool dead;
    private float upDownSpeed;
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
        InvokeRepeating(nameof(periodicUpdates), Random.Range(0.0f, 5.0f), 3.0f);
        fishSystem = GameObject.FindGameObjectWithTag("Fish System");
        fishSystemScript = fishSystem.GetComponent<FishSystemScript>();

        radius = fishSystemScript.radius;
        fullness = 100.0f; //Random.Range(0.0f, 100.0f);
        fullnessDivider = fishSystemScript.fullnessDivider;
        height = fishSystemScript.height;
        hungerRate = fishSystemScript.hungerRate;
        upDownSpeed = fishSystemScript.upDownSpeed;
        dead = false;
        status = Status.Full;
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

        if (!(dead && gameObject.transform.position.y <= 0))
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
    void periodicUpdates()
    {
        if (IsColliding())
        {
            return;
        }

        //if (gameObject.transform.position.x > fishSystemScript.height)
        //{
        //    randZ = -2;
        //}
        //else if (gameObject.transform.position.x < 0)
        //{
        //    ranz
        //}
       
        
        if (fullness >= 0)
        {
            fullness -= hungerRate;
        } else if (fullness < 0)
        {
            dead = true;
            status = Status.Dead;
        }


        float posY = gameObject.transform.position.y;
        /*Debug.Log(fullness);
        Debug.Log(posY);*/

        if (dead)
        {
            if (posY > 0)
            {
                movement = new Vector3(randX, -upDownSpeed, randZ);
            }
        }
        else
        {
            if (posY > height)
            {
                randY = -upDownSpeed;
            }
            else if (posY <= 0)
            {
                randY = upDownSpeed;
            }
            else if (fullness < 70 && posY < fullnessDivider)
            {
                randY = upDownSpeed;
                status = Status.Hungry;
            }
            else if (fullness > 70 && posY > fullnessDivider)
            {
                randY = -upDownSpeed;
                status = Status.Full;
            }

            // Rotates the fish in the right direction
            randX = Random.Range(-1.0f, 1.0f);
            randZ = Random.Range(-1.0f, 1.0f);
            //randY = Random.Range(-0.3f, 0.3f);

            movement = new Vector3(randX, randY, randZ);
            Quaternion target = Quaternion.LookRotation(movement);
            target = Quaternion.RotateTowards(gameObject.transform.rotation, target, 360);
            gameObject.transform.rotation = target;
        }
    }
}
