using UnityEngine;

public class ScrollingTexture : MonoBehaviour
{
    // ----------------- Editor Variables -----------------

    [SerializeField]
    private float scrollSpeedX = 0.1F;

    [SerializeField]
    private float scrollSpeedY = 0F;

    [SerializeField]
    private Renderer rend;

    [SerializeField]
    private ConveyorController belt;

    // ----------------- Unity Functions -----------------

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void FixedUpdate()
    {
        // Rotates the texture, making it seem like the object is moving
        if (belt.IsActive)
        {
            float offsetX = Time.time * scrollSpeedX;
            float offsetY = Time.time * scrollSpeedY;
            rend.material.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));
        }
    }
}
