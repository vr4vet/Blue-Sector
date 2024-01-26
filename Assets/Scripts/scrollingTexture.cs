using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrollingTexture : MonoBehaviour {

    public float scrollSpeed = 0.1F;
    public Renderer rend;
    public conveyorMove belt;
    void Start()
    {
        rend = GetComponent<Renderer>();
    }
    void Update()
    {
        // Rotates the texture, making it seem like the object is moving
        if (belt.beltOn == true)
        {
            float offset = Time.time * scrollSpeed;
            rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
        }
    }
}