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
       meshRenderer.material.mainTextureOffset += new Vector2(Time.deltaTime * scrollX / 10, Time.deltaTime * scrollY / 10);
    }
}
