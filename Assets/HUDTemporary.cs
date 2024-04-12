using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDTemporary : MonoBehaviour
{
    [SerializeField] private MaintenanceManager manager;
    [SerializeField] private GameObject confettiGroup;
    private float animationDuration = 8f;
    private float speed = 3f; // Adjust this to control the float speed
    private float floatStrength = 0.02f; // Adjust this to control the float range
    private bool isFloating = false; // Track whether the object is currently floating
    private CanvasGroup canvasGroup; // Reference to the CanvasGroup component
                                     // [SerializeField] private GameObject confettiGroup;

    private void Start()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>(); // Get the CanvasGroup component
        manager.TaskCompleted.AddListener(EnableToast);
    }

    public void EnableToast()
    {
        if (!isFloating) StartConfetti();
        StartCoroutine(FadeAnimation());
        isFloating = true;


    }
    private void StartConfetti()
    {
confettiGroup.SetActive(true);
        foreach (Transform child in confettiGroup.transform)
        {
            
            child.GetComponent<ParticleSystem>().Play();

        }


    }

    private System.Collections.IEnumerator FadeAnimation()
    {
        float startTime = Time.time;

        while (Time.time - startTime < animationDuration)
        {
            if (Time.time - startTime < 1.2f)
            {
                // Apply fade-in effect
                float alpha = Mathf.Lerp(0f, 1f, (Time.time - startTime) / (1.2f));
                canvasGroup.alpha = alpha;
            }
            else if (Time.time - startTime > 6.8f)
            {
                // Apply fade-out effect
                float alpha = Mathf.Lerp(1f, 0f, (Time.time - startTime - 6.8f) / (1.2f));
                canvasGroup.alpha = alpha;
            }



            yield return null; // Wait for the next frame
        }
        canvasGroup.alpha = 0;




    }


}
