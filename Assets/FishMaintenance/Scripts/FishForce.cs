using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishForce : MonoBehaviour
{
    [SerializeField] private FishSpawner fishSpawner;
    // Start is called before the first frame update
    void Start()
    {
        addForce();
    }

    public void addForce()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.AddForce(0.7f, 1f, 0, ForceMode.Impulse);
        gameObject.GetComponent<Outline>().enabled = true;
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DeadfixRespawnBox"))
        {
            // gameObject.transform.position = new Vector3(-0.5608958f,1.0359474f,-2.769325f);
            fishSpawner.RespawnDeadfish();
            Debug.Log("Respawned");
            Destroy(gameObject);
        }
    }
}
