using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private GameObject deadfish;
    // Start is called before the first frame update
    public void RespawnDeadfish()
    {
        GameObject fish = Instantiate(deadfish, new Vector3 (21.150299072265626f, 1.247399926185608f, -66.38359832763672f), Quaternion.Euler(84.157959f, 115.653328f, 299.139221f));
        fish.SetActive(true);
        fish.GetComponent<FishForce>().addForce();
    }
}
