using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDMessage : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] private MaintenanceManager manager;
    [SerializeField] private GameObject badge;

    [SerializeField] private TMP_Text skillText;

    private float animationDuration = 5f;
    private Task.Skill skill;
    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        manager.SkillCompleted.AddListener(EnableSkillDisplay);
    }


    public void EnableSkillDisplay(Task.Skill skill)
    {
        skillText.text = skill.Name;
        badge.GetComponent<UnityEngine.UI.Image>().sprite = skill.Icon;
        StartCoroutine(FadeAnimation());
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
            else if (Time.time - startTime > 3.8f)
            {
                // Apply fade-out effect
                float alpha = Mathf.Lerp(1f, 0f, (Time.time - startTime - 3.8f) / (1.2f));
                canvasGroup.alpha = alpha;
            }



            yield return null; // Wait for the next frame
        }
        canvasGroup.alpha = 0;




    }


}
