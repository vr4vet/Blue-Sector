using UnityEngine;

public class HandFeeding : MonoBehaviour
{
    [SerializeField] private GameObject bucket;
    [SerializeField] private GameObject shovel;
    [SerializeField] private GameObject videoObject;
    [SerializeField] private BNG.Grabber grabberLeft;
    [SerializeField] private BNG.Grabber grabberRight;
    [SerializeField] private Task.Subtask subtask;
    [SerializeField] private Task.Step feedingStep;
    [SerializeField] private Task.Step requiredEquipmentStep;
    [SerializeField] private Task.Step watchVideoStep;
    private DropItem dropItem;



    // Start is called before the first frame update
    void Start()
    {
        dropItem = gameObject.GetComponent<DropItem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (feedingStep.IsCompeleted()) dropItem.DropAll();
        if (subtask.Compleated())
        {
            bucket.SetActive(false);
        }

    }

    void OnEnable()
    {
        SetEquipmentActive();
    }

    public void OnTriggerExit(Collider other)
    {
/*        if (other.CompareTag("Player"))
        {
            bucket.SetActive(false);
            videoObject.SetActive(false);
        }*/
    }

    public void SetEquipmentActive()
    {
        bucket.SetActive(true);
        shovel.SetActive(true);
        videoObject.SetActive(true);

        // BNG.Grabbable bucketGrabbable = bucket.GetComponent<BNG.Grabbable>();
        BNG.Grabbable shovelGrabbable = shovel.GetComponent<BNG.Grabbable>();
        // grabberLeft.GrabGrabbable(bucketGrabbable);
        grabberRight.GrabGrabbable(shovelGrabbable);
    }

}