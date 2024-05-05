using UnityEngine;

public class CutFishBody : CutFish
{
    /// <summary>
    /// When the knife collides with the fish body, the player makes a wrong cut
    /// </summary>
    /// <param name="collision"> The knife object </param>
    private void OnCollisionEnter(Collision collisionObject)
    {
        cutEvent(collisionObject.collider.tag);
    }
}

