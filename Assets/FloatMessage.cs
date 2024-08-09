using UnityEngine;

public class FloatMessage : MonoBehaviour
{
    private Vector3 initialPosition;
    private Rigidbody rb;
    private float speed = 3f; // Adjust this to control the float speed
    private float floatStrength = 0.02f; // Adjust this to control the float range
    private float animationDuration = 4f; // Set the duration in seconds
    private bool isFloating = false; // Track whether the object is currently floating
    private CanvasGroup canvasGroup; // Reference to the CanvasGroup component
                                     // [SerializeField] private GameObject confettiGroup;

    private void Start()
    {
        initialPosition = gameObject.transform.position;
        rb = gameObject.GetComponent<Rigidbody>();
        canvasGroup = gameObject.GetComponent<CanvasGroup>(); // Get the CanvasGroup component
        EnableToast();

    }

    public void EnableToast()
    {
        // if (!isFloating) StartConfetti();
        StartCoroutine(FloatAnimation());
        // isFloating = true;


    }
    // private void StartConfetti()
    // {

    //     foreach (Transform child in confettiGroup.transform)
    //     {
    //         child.GetComponent<ParticleSystem>().Play();

    //     }


    // }
    private System.Collections.IEnumerator FloatAnimation()
    {
        float startTime = Time.time;

        while (Time.time - startTime < animationDuration)
        {
            float newY = initialPosition.y + Mathf.Sin((Time.time - startTime) * speed) * floatStrength;
            rb.MovePosition(new Vector3(rb.position.x, newY, rb.position.z));

            if (Time.time - startTime < 1.5f)
            {
                // Apply fade-in effect
                float alpha = Mathf.Lerp(0f, 1f, (Time.time - startTime) / (1.5f));
                canvasGroup.alpha = alpha;
            }
            else if (Time.time - startTime > 2.5f)
            {
                // Apply fade-out effect
                float alpha = Mathf.Lerp(1f, 0f, (Time.time - startTime - 2.5f) / (1.5f));
                canvasGroup.alpha = alpha;
            }


            yield return null; // Wait for the next frame
        }




    }
}
