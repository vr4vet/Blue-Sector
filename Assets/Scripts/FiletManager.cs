using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FiletManager : MonoBehaviour {


    public Transform spawnPoint;
	public GameObject filet;
    public GameObject filletBad;
    public Texture fishmeat;
    public conveyorMove belt;
    int filletCount = 0;
    public int filletWrong = 0;

    public AddPoints addPoints;
    public GameObject toggle1;
    public GameObject toggle2;


    void Start()
    {
        //Runs the spawn-function every 15 seconds
        InvokeRepeating("Spawn", 0f, 15.0f);
    }

    void Update () {
        // Keeps count on how many fillets have spawned an stops invoke when it is at max, also tracks wrong points and gives points accordingly
        if (filletCount == 20)
        {
            CancelInvoke("Spawn");
            addPoints.AddFysikk(2);
            toggle1.GetComponent<Toggle>().isOn = true;

            if (filletWrong == 0)
            {
                addPoints.AddDyktighet(2);
                toggle2.GetComponent<Toggle>().isOn = true;
            }
            filletCount ++;
        }
    }
    // Spawns fillets, good and bad, within a set interval
	void Spawn () {
        if (belt.beltOn)
        {
            if (filletCount == 2 || filletCount == 5 || filletCount == 10 || filletCount == 12 || filletCount == 17)
            {
                Instantiate(filet, spawnPoint.position, spawnPoint.rotation);
            }
            else
            {
                Instantiate(filletBad, spawnPoint.position, spawnPoint.rotation);
            }
            filletCount++;
        }
    }



}
