using System;
using UnityEngine;

public class FishScript : MonoBehaviour
{
    private float radius;
    private float fullnessDivider;
    private float top;
    private float bottom;

    [SerializeField]
    private bool dead = false;
    private bool returnedAfterEating = true;   // used to ensure "even spread" of fish when swimming downwards
    private float verticalLimit = 1.5f; // limits how far up or down the next destination will be

    [SerializeField]
    private bool randomFish;    // makes fish ignore hunger state, meaning it swims randomly. makes fish stream more dynamic

    private Animation fishAnimation;
    private GameObject fishSystem;
    private FishSystemScript fishSystemScript;

    private float destinationY;
    private Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        // unique seed per fish for rng, and invoke PeriodicUpdates() with random offset
        //UnityEngine.Random.InitState((int)DateTime.Now.Ticks);

        // get properties of parent fish cage
        fishSystem = gameObject.transform.parent.gameObject;
        fishSystemScript = fishSystem.GetComponent<FishSystemScript>();
        fishAnimation = gameObject.transform.GetChild(0).GetComponent<Animation>();

        radius = fishSystemScript.radius;
        top = fishSystemScript.height / 2;  // top of merd/water surface
        bottom = -fishSystemScript.height / 2; // bottom of merd
        fullnessDivider = bottom + ((top - bottom) * fishSystemScript.fullnessDivider); // border between hungry and full fish

        fishSystemScript.FishStateChanged.AddListener(SetReturnedAfterEating);

        GenerateDestination(); // initialize a destination before fish start swimming
    }

    private void SetReturnedAfterEating(FishSystemScript fishSystem)
    {
        returnedAfterEating = false;
        GenerateDestination();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.localPosition, destination) < 0.001f) // generate new destination when reached
        {
            returnedAfterEating = true;
            GenerateDestination();
        }
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, destination, 1 * Time.deltaTime);
        RotateFish();
    }

    // rotates fish towards its current destination
    void RotateFish()
    {
        if (dead)
        {
            return;
        }
        const float rotationLimit = 0.3f;   // prevent fish from tipping too far upwards/downwards
        Vector3 targetDirection = destination - transform.localPosition;
        targetDirection.y = Mathf.Clamp(targetDirection.y, -rotationLimit, rotationLimit);
        if (targetDirection != Vector3.zero)
        {
            // The commented out code makes turning gradual
/*            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 15 * Time.deltaTime, 360);
            transform.rotation = Quaternion.LookRotation(newDirection);*/
            transform.rotation = Quaternion.LookRotation(targetDirection);
        }
    }

    private void GenerateDestination()
    {
        FishSystemScript.FishState state = fishSystemScript.state;
        FishSystemScript.FeedingIntensity feedingIntensity = fishSystemScript.feedingIntensity;
        Vector3 fishPosition = transform.localPosition;

        if (dead)
        {
            return; 
        }

        if (state != FishSystemScript.FishState.Idle && !randomFish)
        {
            if (feedingIntensity == FishSystemScript.FeedingIntensity.High && (state == FishSystemScript.FishState.Hungry || state == FishSystemScript.FishState.Dying))
            {
                destinationY = UnityEngine.Random.Range(fullnessDivider, top - 0.5f);
            }
            else if (state == FishSystemScript.FishState.Full)
            {
                if (returnedAfterEating)
                    destinationY = Mathf.Clamp(UnityEngine.Random.Range(fishPosition.y - verticalLimit, fishPosition.y + verticalLimit), bottom, fullnessDivider);
                else
                    destinationY = Mathf.Clamp(UnityEngine.Random.Range(bottom, fishPosition.y + verticalLimit), bottom, fullnessDivider);

            }
        } 
        else
        {
            destinationY = Mathf.Clamp(UnityEngine.Random.Range(fishPosition.y - verticalLimit, fishPosition.y + verticalLimit), bottom, top);
        }
        Vector2 destinationXZ = UnityEngine.Random.insideUnitCircle * radius;   // generate random point in a circle (horizontal cross section of cage)
        destination = new Vector3(destinationXZ.x, destinationY , destinationXZ.y);
    }

    // public function allowing fish system to kill starving fish
    public void Kill()
    {
        destination = new Vector3(destination.x, bottom + 0.5f, destination.z); // make fish hit bottom
        dead = true;
        fishAnimation.Stop();
    }

    // public function allowing fish system to revive dead fish
    public void Revive()
    {
        dead = false;
        fishAnimation.Play();
    }

    // public function telling this fish to swim randomly and not get hungry etc. Make the fish stream a bit less monotone.
    public void SetRandomFish() => randomFish = true;

    // Draw destination in scene view for debugging purposes
    private void OnDrawGizmos()
    {
        if (fishSystemScript != null)
        {
            Vector3 systemPos = fishSystemScript.transform.position;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(new Vector3(systemPos.x + destination.x, systemPos.y + destination.y, systemPos.z + destination.z), 0.05f);
        }

    }
}
