using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerdFishSystem : MonoBehaviour
{
    [SerializeField] private GameObject fish;
    [SerializeField] private float radius;
    [SerializeField] private float height;
    [SerializeField] private int amountOfFish;
    private Vector3 position;



    public float Radius { get => radius; set => radius = value; }
    public float Height { get => height; set => height = value; }

    // Start is called before the first frame update
    void Start()
    {
        position = gameObject.transform.position;


    }

    public void GenerateFish(Vector3 targetPos)
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 rotationVector = new Vector3(0f, 45f, 0f);
            GameObject newFish = Instantiate(fish, new Vector3(targetPos.x + Random.Range(-0.5f, 0.5f), targetPos.y, targetPos.z + Random.Range(-0.5f, 0.5f)), Quaternion.Euler(rotationVector));
            newFish.GetComponent<MerdFish>().FishSystem = this.gameObject;
            newFish.transform.parent = gameObject.transform;
            Destroy(newFish, 15f); // fish dies after this many seconds
        }
    }

}
