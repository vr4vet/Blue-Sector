using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainToggle : MonoBehaviour
{
    private GameObject TerrainDetailed, TerrainSimple;

    // Start is called before the first frame update
    void Start()
    {
        TerrainDetailed = GameObject.Find("OceanFloor");
        TerrainSimple = GameObject.Find("OceanFloorSprites");
        TerrainDetailed.SetActive(false);
        TerrainSimple.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TerrainDetailed.SetActive(false);
            TerrainSimple.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TerrainDetailed.SetActive(true);
            TerrainSimple.SetActive(false);
        }
    }
}
