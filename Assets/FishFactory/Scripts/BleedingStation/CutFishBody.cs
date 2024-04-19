using UnityEngine;

public class CutFishBody : MonoBehaviour
{
    [Tooltip("The fish state script attached to the main fish object")]
    [SerializeField]
    private FactoryFishState fishState;

    /// <summary>
    /// When the knife collides with the fish body, the player makes a wrong cut
    /// </summary>
    /// <param name="collision"> The knife object </param>
    private void OnCollisionEnter(Collision collisionObject)
    {
        if (collisionObject.gameObject.tag != "Knife")
        {
            return;
        }

        fishState.CutFishBody();
    }
}
