using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDMessage : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] private MaintenanceManager manager;
    [SerializeField] private GameObject SkillUnlocked;
    [SerializeField] private GameObject TaskCompleted;
    [SerializeField] private GameObject confettiGroup;
    [SerializeField] private AudioClip soundEffect;
    private CanvasGroup canvasGroup;


    void Start()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        manager.SkillCompleted.AddListener(EnableSkillDisplay);
        manager.TaskCompleted.AddListener(EnableTaskDisplay);
    }

    public void EnableTaskDisplay(Task.Task task)
    {
        if (task != null)
        {
            SkillUnlocked.SetActive(false);
            canvasGroup.alpha = 0;
            StartCoroutine(DelayAnimation());

        }
    }


    private System.Collections.IEnumerator DelayAnimation()
    {
        yield return new WaitForSeconds(5f);
        TaskCompleted.SetActive(true);
        canvasGroup.alpha = 0;
        PlayAudio(soundEffect);
        StartCoroutine(FadeAnimation(7f, 3.5f));

    }


    public void EnableSkillDisplay(Task.Skill skill)
    {
        if (skill != null)
        {

            TMP_Text skillText = SkillUnlocked.transform.Find("txt_SkillName").GetComponent<TMP_Text>();
            skillText.text = skill.Name;
            GameObject badge = SkillUnlocked.transform.Find("badgeDisplay").gameObject;
            UnityEngine.UI.Image badgeIcon = badge.transform.Find("icon_badge").GetComponent<UnityEngine.UI.Image>();
            badgeIcon.sprite = skill.Icon;
            StartCoroutine(FadeAnimation(5.5f, 3.5f));

        }
    }


    private System.Collections.IEnumerator FadeAnimation(float animationDuration, float fadeduration)
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
            else if (Time.time - startTime > fadeduration)
            {
                // Apply fade-out effect
                float alpha = Mathf.Lerp(1f, 0f, (Time.time - startTime - fadeduration) / (1.2f));
                canvasGroup.alpha = alpha;
            }



            yield return null; // Wait for the next frame
        }
        canvasGroup.alpha = 0;




    }


    public void PlayAudio(AudioClip audio)
    {
        //if the gameobject has audiosource
        if (TryGetComponent<AudioSource>(out AudioSource audioSource))
        {
            audioSource.Stop();
            audioSource.PlayOneShot(audio);
            return;
        }

        //otherwise create audiosource
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.PlayOneShot(audio);
    }



}
