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
        TerrainSimple.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TerrainDetailed.GetComponent<Terrain>().drawHeightmap = false;
            TerrainDetailed.GetComponent<Terrain>().drawTreesAndFoliage = false;
            TerrainSimple.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TerrainDetailed.GetComponent<Terrain>().drawHeightmap = true;
            TerrainDetailed.GetComponent<Terrain>().drawTreesAndFoliage = true;
            TerrainSimple.SetActive(false);
        }
    }
}
