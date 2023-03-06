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

    InspectionTaskManager inspectionTaskManager;


    // Start is called before the first frame update
    void Start()
    {
        inspectionTaskManager = GameObject.FindObjectOfType<InspectionTaskManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
