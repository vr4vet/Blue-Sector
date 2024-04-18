using UnityEngine;

public class CutFishGill : MonoBehaviour
{
    [Tooltip("The fish state script attached to the main fish object")]
    [SerializeField]
    private FactoryFishState fishState;

    /// <summary>
    /// When the knife collides with the fish gill, cut the gill
    /// </summary>
    /// <param name="collider"> The knife collider </param>
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag != "Knife")
        {
            return;
        }

        fishState.CutFishGills();
    }
}
