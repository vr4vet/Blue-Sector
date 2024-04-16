using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingTexture : MonoBehaviour
{

    public float scrollSpeedX = 0.1F;
    public float scrollSpeedY = 0F;
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
            float offsetX = Time.time * scrollSpeedX;
            float offsetY = Time.time * scrollSpeedY;
            rend.material.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));
        }
    }
}