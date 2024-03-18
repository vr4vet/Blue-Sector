using UnityEngine;
using System.Timers;
using UnityEngine;
using System.Timers;
using UnityEngine;

public class FloatMessage : MonoBehaviour
{
    private Vector3 initialPosition;
    private Rigidbody rb;
    private float speed = 3f; // Adjust this to control the float speed
    private float floatStrength = 0.01f; // Adjust this to control the float range
    public float animationDuration = 2f; // Set the duration in seconds
    [SerializeField] private TMP_Text displayText;
    public string text;

    private bool isFloating = false; // Track whether the object is currently floating


    private void Start()
    {
        initialPosition = gameObject.transform.position;
        rb = gameObject.GetComponent<Rigidbody>();
        displayText.text = text;
    }

    private void Update()
    {
        if (!isFloating && gameObject.activeInHierarchy)
        {
            StartCoroutine(FloatAnimation());
            isFloating = true;
        }
    }

    private System.Collections.IEnumerator FloatAnimation()
    {
        float startTime = Time.time;

        while (Time.time - startTime < animationDuration)
        {
            float newY = initialPosition.y + Mathf.Sin((Time.time - startTime) * speed) * floatStrength;
            rb.MovePosition(new Vector3(rb.position.x, newY, rb.position.z));
            yield return null; // Wait for the next frame
        }

        Destroy(gameObject);
    }
}

