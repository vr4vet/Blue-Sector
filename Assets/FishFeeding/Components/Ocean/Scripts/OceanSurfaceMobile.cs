using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanSurfaceMobile : MonoBehaviour
{
    public float scrollX;
    public float scrollY;
    private MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // sine wave to scroll texture in a wave-like motion
        float sineValue = Mathf.Abs(Mathf.Sin(Time.time)) * 0.005f + 0.005f;
        meshRenderer.material.mainTextureOffset += new Vector2(sineValue * scrollX / 10, sineValue * scrollY / 10);
    }
}
