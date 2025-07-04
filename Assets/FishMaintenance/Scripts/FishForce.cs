using UnityEngine;

public class FishForce : MonoBehaviour
{
    [SerializeField] private GameObject respawnPoint;
    private Rigidbody _rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        addForce();
    }

    public void addForce()
    {
        _rigidBody.AddForce(0.7f, 1f, 0, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("LargerBoat"))
            _rigidBody.position = respawnPoint.transform.position;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "FishContainer")
            _rigidBody.position = respawnPoint.transform.position;
    }
}
