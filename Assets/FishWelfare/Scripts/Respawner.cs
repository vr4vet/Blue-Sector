using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    [Header("Respawn properties")]
    [Tooltip("The depth at which a respawn will be triggered")]
    [SerializeField] private float _killPlane;
    [Tooltip("Set to false if setting respawn position in code. Set to true if setting in inspector by placing a gameobject ine the SpawnObject field")]
    [SerializeField] private bool _useSpawnObject = true;
    [Tooltip("The gameobject will respawn at this gameobject's position (Respawn Position will be set to this value)")]
    [SerializeField] private GameObject _spawnObject;
    [Tooltip("Where the gameobject will respawn. Can either be set in code, or in the inspector (for example using a gameobject)")]
    [SerializeField] private Vector3 _respawnPosition;
    [Tooltip("What rotation the object will have when respawned. Can either be set in code, or in the inspector")]
    [SerializeField] private Quaternion _respawnRotation;


    // Start is called before the first frame update
    void Start()
    {
        if (_useSpawnObject)
        {
            _respawnPosition = _spawnObject.transform.position;
            _respawnRotation = _spawnObject.transform.rotation;
        }
            
            
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= _killPlane)
            Respawn();
    }

    public void SetRespawnPosition(Vector3 respawnPosition)
    {
        _respawnPosition = respawnPosition;
    }

    public void SetRespawnRotation(Quaternion respawnRotation)
    {
        _respawnRotation = respawnRotation;
    }

    public void SetKillPlane(float yValue)
    {
        _killPlane = yValue;
    }

    public void Respawn()
    {
        transform.position = _respawnPosition;
        transform.rotation = _respawnRotation;
        if (gameObject.GetComponent<Rigidbody>() != null)
        {
            Debug.Log("Oi!");
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        else
        {
            foreach (Rigidbody body in GetComponentsInChildren<Rigidbody>())
            {
                Debug.Log("Child rigid happening!");
                body.velocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
            }
        }

    }
}
