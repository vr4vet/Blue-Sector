using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    private float fullnessLimit;
    private float hungerRate;
    private float top;
    private float bottom;
    private bool dead;
    private float swimSpeedVertical;
    private float swimSpeedHorizontal;

    private Animation fishAnimation;
    private GameObject fishSystem;
    private FishSystemScript fishSystemScript;
    
    // Start is called before the first frame update
    void Start()
    {   
        InvokeRepeating(nameof(PeriodicUpdates), Random.Range(0.0f, 5.0f), 3.0f);
        fishSystem = GameObject.Find("FishSystem");
        fishSystemScript = fishSystem.GetComponent<FishSystemScript>();
        fishAnimation = gameObject.transform.GetChild(0).GetComponent<Animation>();
        
        radius = fishSystemScript.radius;
        fullness = 100.0f; //Random.Range(0.0f, 100.0f);
        top = (fishSystem.transform.position.y + (fishSystemScript.height / 2)) - 1.5f;  // top of merd/water surface
        bottom = (fishSystem.transform.position.y - (fishSystemScript.height / 2)) + 1.5f; // bottom of merd
        fullnessDivider = bottom + (((bottom - top) * (fishSystemScript.fullnessDivider)) * -1); // border between hungry and full fish
        hungerRate = fishSystemScript.hungerRate;
        swimSpeedVertical = fishSystemScript.swimSpeedVertical;
        swimSpeedHorizontal = fishSystemScript.swimSpeedHorizontal;
        fullnessLimit = fishSystemScript.fullnessLimit;
        dead = false;
    }

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

        // 1/2 likelihood of getting fed if player is feeding
        if (fishSystemScript.feeding && posY > fullnessDivider)
        {
            if (Random.Range(0, 100) <= 50)
            {
                if (fullness >= 100)
                {
                    fullness = 100;
                } else
                {
                    fullness += 20;
                }
            }
        }

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
        }

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
            }
            else if (fullness >= fullnessLimit && posY > fullnessDivider)
            {
                randY = -swimSpeedVertical;
            }

            // Rotates the fish in the right direction
            randX = Random.Range(-swimSpeedHorizontal, swimSpeedHorizontal);
            randZ = Random.Range(-swimSpeedHorizontal, swimSpeedHorizontal);

            movement = new Vector3(randX, randY, randZ);
            Quaternion target = Quaternion.LookRotation(movement);
            target = Quaternion.RotateTowards(gameObject.transform.rotation, target, 360);
            gameObject.transform.rotation = target;
        }
    }
}
