using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingTexture : MonoBehaviour {

    public float scrollSpeed = 0.1F;
    public Renderer rend;
    public ConveyorController belt;
    void Start()
    {
        rend = GetComponent<Renderer>();
    }
    void FixedUpdate()
    {
        // Rotates the texture, making it seem like the object is moving
        if (belt.IsBeltOn)
        {
            float offset = Time.time * scrollSpeed;
            rend.material.SetTextureOffset("_BaseMap", new Vector2(offset, 0));
        }
    }
}