using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField]
    private int gillDamage = 3;
    [SerializeField]
    private int gillDamageGuessed = 0;
    [SerializeField]
    private int id;
    
    //The following variables are used for handeling behaviour with water
    private Vector3 waterBodyCenter;
    private Vector3 targetPosition;
    private float waterBodyXLength;
    private float waterBodyZLength;
    private bool isInWater;

    InspectionTaskManager inspectionTaskManager;


    // Start is called before the first frame update
    void Start()
    {
        //Skamløst kokt fra FishScript.cs:
        InvokeRepeating(nameof(PeriodicUpdates), Random.Range(0.0f, 5.0f), 3.0f);
        inspectionTaskManager = GameObject.FindObjectOfType<InspectionTaskManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isInWater){
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 2 * Time.deltaTime);
        }
    }

    void PeriodicUpdates() {
        SetMoveTarget();
    }

    private void SetMoveTarget() {
        //Mer Skamløs koking fra FishScript.cs:
        Debug.Log("Setting movement");
        float randX = Random.Range(waterBodyCenter.x -waterBodyXLength / 2,waterBodyCenter.x + waterBodyXLength / 2);
        float randZ = Random.Range(waterBodyCenter.z -waterBodyZLength / 2,waterBodyCenter.z + waterBodyZLength / 2);
        targetPosition = new Vector3(randX, transform.position.y, randZ);
        Debug.Log("xLength: " + randX + " zLength " + randZ);
        Quaternion target = Quaternion.LookRotation(targetPosition);
        target = Quaternion.RotateTowards(gameObject.transform.rotation, target, 360);
        gameObject.transform.rotation = target;
    }

    public void UpdateWaterBody(float waterheight, Vector3 bodyCenter, float xLength, float zLength, bool isInWater){
        Debug.Log("Updating body");
        gameObject.GetComponent<Floating>().waterHeight = waterheight;
        waterBodyCenter = bodyCenter;
        waterBodyXLength = xLength;
        waterBodyZLength = zLength;
        this.isInWater = isInWater;
    }

    public void SetAsSelectedFish() {
        inspectionTaskManager.SetSelectedFish(this); 
    }

    public void SetgillDamageGuessed(int guess) {
        gillDamageGuessed = guess;
    }

    public int GetGillDamage() {
        return gillDamage;
    }

    public int GetGillDamageGuessed() {
        return gillDamageGuessed;
    }

    public int GetId() {
        return id;
    }
}
