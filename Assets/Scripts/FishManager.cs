using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FishManager : MonoBehaviour
{

    public Transform spawnPoint;
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform spawnPoint3;
    public conveyorMove belt;
    public GameObject fish;
    public Material materialGood;
    public Material materialBad1;
    public Material materialBad2;
    int rand;
    float randSize;
    int fishCounter = 0;
    public AddPoints addPoints;
    public GameObject toggle1;
    public GameObject toggle2;
    public int wrongCounter = 0;
    public int maxSpawnCount = 100;


    void Start()
    {
        // Spawns a fish every 2 seconds
        InvokeRepeating("Spawn", 1.0f, 2f);
    }

    void Update()
    {
        //Alternates between the three spawn-points
        if (fishCounter % 3 == 0)
        {
            spawnPoint = spawnPoint1;
        }
        if (fishCounter % 3 == 1)
        {
            spawnPoint = spawnPoint2;
        }
        if (fishCounter % 3 == 2)
        {
            spawnPoint = spawnPoint3;
        }
        // Keeps count on how many fish have spawned an stops invoke when it is at max, also tracks wrong points and gives points accordingly
        if (fishCounter == maxSpawnCount)
        {
            CancelInvoke("Spawn");
            addPoints.AddDyktighet(1);
            addPoints.AddFysikk(3);
            toggle1.GetComponent<Toggle>().isOn = true;

            if (wrongCounter <= 10)
            {
                addPoints.AddDyktighet(1);
                addPoints.AddNoyaktighet(3);
                toggle2.GetComponent<Toggle>().isOn = true;
            }
            fishCounter += 1;
        }

        

    }

    // Spawns fish, good and bad, within a set interval
    void Spawn()
    {
        if (belt.beltOn){
            rand = Random.Range(0, 2);
            randSize = Random.Range(-0.3f, 0.3f);
            GameObject clone = Instantiate(fish, spawnPoint.position, spawnPoint.rotation);
            clone.transform.GetChild(0).GetChild(0).localScale += new Vector3(randSize, randSize, randSize);
            clone.transform.root.gameObject.AddComponent<FishGoodBad>();
            if (fishCounter % 5 == 0) {
                if (rand == 0){
                    clone.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = materialBad1;
                    clone.transform.root.GetComponent<FishGoodBad>().fishGood = false;
                }
                else if (rand == 1)
                {
                    
                    clone.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = materialBad2;
                    clone.transform.root.GetComponent<FishGoodBad>().fishGood = false;
                }
            }
            else
            {
                clone.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = materialGood;
                clone.transform.root.GetComponent<FishGoodBad>().fishGood = true;

            }
            fishCounter += 1;
        }
    }
}
