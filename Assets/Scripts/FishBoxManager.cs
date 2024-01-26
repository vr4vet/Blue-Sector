using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FishBoxManager : MonoBehaviour
{

    public Transform spawnPoint;
    public Transform spawnPointFish;
    public conveyorMove belt;
    public GameObject fishBox;
    public GameObject fishBox2;
    public GameObject wrongFishBox;
    public GameObject wrongFishBox2;
    public GameObject fish;
    public AddPoints addPoints;
    public GameObject toggle1;
    public GameObject toggle2;
    public int wrongCounter = 0;
    int boxCounter = 0;


    void Start()
    {
        // Runs the Spawn function every 10 seconds
        InvokeRepeating("Spawn", 1.0f, 10f);
    }

    void Update()
    {
        // Keeps count on how many fishboxes have spawned an stops invoke when it is at max, also tracks wrong points and gives points accordingly
        if (boxCounter == 20)
        {
            CancelInvoke("Spawn");
            addPoints.AddDyktighet(1);
            addPoints.AddFysikk(3);
            toggle1.GetComponent<Toggle>().isOn = true;

            if (wrongCounter == 0)
            {
                addPoints.AddDyktighet(1);
                addPoints.AddNoyaktighet(3);
                toggle2.GetComponent<Toggle>().isOn = true;
            }
            boxCounter += 1;
        }

        

    }
    // Spawns fishboxes, good and bad, within a set interval
    void Spawn()
    {
        if (belt.beltOn)
        {

            if (boxCounter == 2 || boxCounter == 4 || boxCounter == 7 || boxCounter == 10 || boxCounter == 13 || boxCounter == 17 || boxCounter == 19)
            {
                GameObject wrongclone = Instantiate(wrongFishBox, spawnPoint.position, spawnPoint.rotation);
                GameObject fishclone = Instantiate(fish, spawnPointFish.position, spawnPoint.rotation);
                fishclone.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.AddComponent<FishBoxOutside>();
            }
            /*else if (){
                GameObject wrongclone = Instantiate(wrongFishBox2, spawnPoint.position, spawnPoint.rotation);
            }*/

            else if (boxCounter == 3 || boxCounter == 6 || boxCounter == 12 || boxCounter == 15) {
                GameObject clone = Instantiate(fishBox2, spawnPoint.position, spawnPoint.rotation);
            }
            else
            {
                GameObject clone = Instantiate(fishBox, spawnPoint.position, spawnPoint.rotation);
            }
            boxCounter++;
        }

    }
}
