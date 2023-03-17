// Created by Trym Lund Flogard <trym@flogard.no>
using BNG;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

[DisallowMultipleComponent]
[RequireComponent(typeof(TextMeshPro))]
public class Popup : MonoBehaviour
{
    private const int AnimationDurationMs = 500;
    private BNGPlayerController playerController;
    private TextMeshPro textMesh;
    private Renderer currentRenderer;
    private Renderer backgroundRenderer;

    [field: SerializeField]
    public GameObject Background { get; set; }

    private BNGPlayerController Player
    {
        get
        {
            if (playerController == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                playerController = player != null
                    ? player.GetComponentInChildren<BNGPlayerController>()
                    : FindObjectOfType<BNGPlayerController>();
            }

            return playerController;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
        currentRenderer = GetComponent<Renderer>();
        if (Background)
        {
            backgroundRenderer = Background.GetComponent<Renderer>();
        }
        enabled = currentRenderer.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        enabled = currentRenderer.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Make the popup face the player
        Vector3 playerPosition = Player != null ? Player.transform.position : Camera.main.transform.position;
        var playerToSelf = transform.position - playerPosition;
        transform.rotation = Quaternion.LookRotation(playerToSelf);

        // Update the rounded rect background
        var size = textMesh.textBounds.size;
        backgroundRenderer.sharedMaterial.SetVector("_Size", size);
        Background.transform.localScale = new Vector3(size.x / size.y, size.y / size.x, 1);
    }

    /// <summary>
    /// Fades the popup into view
    /// </summary>
    public void Show()
    {
        StopAllCoroutines();
        var material = currentRenderer.material;
        Color color = material.color;
        color.a = 1;
        StartCoroutine(Fade(static progress => Easing.InCubic(progress)).GetEnumerator());
        enabled = currentRenderer.enabled = true;
    }

    /// <summary>
    /// Hides the popup from view
    /// </summary>
    public void Hide()
    {
        StopAllCoroutines();
        StartCoroutine(Out());

        IEnumerator Out()
        {
            foreach (var _ in Fade(static progress => Easing.OutCubic(1 - progress)))
            {
                yield return null;
            }

            enabled = currentRenderer.enabled = false;
        }
    }

    private IEnumerable Fade(Func<float, float> interpolate)
    {
        var material = currentRenderer.material;
        float start = Time.time;
        Color color = material.color;
        float targetAlpha = color.a;
        color.a = interpolate(0);
        material.color = color;
        float progress = 0;
        while (progress < 1)
        {
            progress = (Time.time - start) / (AnimationDurationMs / 1000);
            color.a = targetAlpha * interpolate(progress);
            material.color = color;
            yield return null;
        }
    }
}
