using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadfishTask : MonoBehaviour
{
    [SerializeField] private GameObject[] deadfishEquipment;
    [SerializeField] private GameObject foldedLoader;
    // Start is called before the first frame update
    void Start()
    {

    }

    void OnDisable()
    {
        foreach (GameObject equipment in deadfishEquipment)
        {
            equipment.SetActive(true);
        }
        foldedLoader.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
