using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandFeeding : MonoBehaviour
{

    [SerializeField] private GameObject bucket;
    [SerializeField] private GameObject shovel;
    [SerializeField] private BNG.Grabber grabberLeft;
    [SerializeField] private BNG.Grabber grabberRight;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnEnable()
    {
        BNG.Grabbable bucketGrabbable = bucket.GetComponent<BNG.Grabbable>();
        BNG.Grabbable shovelGrabbable = shovel.GetComponent<BNG.Grabbable>();
        // bucket.SetActive(true);
        grabberLeft.GrabGrabbable(bucketGrabbable);
        // shovel.SetActive(true);
        grabberRight.GrabGrabbable(shovelGrabbable);




    }
}
