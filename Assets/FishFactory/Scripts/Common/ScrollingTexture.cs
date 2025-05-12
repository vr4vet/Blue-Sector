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
        if (belt.IsBeltOn)
            rend.material.mainTextureOffset += new Vector2(scrollSpeedX, scrollSpeedY);
    }
}
