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

        for (int i = 0; i < amountOfFish; i++)
        {
            GameObject newFish = Instantiate(fish, new Vector3(Random.Range(position.x - radius + 3, position.x + radius - 3), position.y - (height / 2)*2f, Random.Range(position.z - radius + 3, position.z + radius - 3)), fish.transform.rotation);
            newFish.GetComponent<MerdFish>().FishSystem = this.gameObject;
            newFish.transform.parent = gameObject.transform;
        }
    }

    public void SetTargetPos(Vector3 targetPos)
    {
        for (int i = 0; i < 3; i++)
        {
            // Find random fish to move
            int childNo = Mathf.RoundToInt(Random.Range(0, amountOfFish - 1));
            MerdFish merdFish = transform.GetChild(childNo).GetComponent<MerdFish>();
            merdFish.SetTarget(targetPos);

        }
    }


    public Vector3 JumpTargetPosition()
    {
        return new Vector3(Random.Range(position.x - radius + 3, position.x + radius - 3), Random.Range(position.y + 1f, 10f), Random.Range(position.z - radius + 3, position.z + radius - 3));
    }
}
