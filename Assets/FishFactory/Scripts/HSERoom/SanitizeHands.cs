using UnityEngine;

public class SanitizeHands : MonoBehaviour
{
    // Player can put one or both hands into the sanitizer to clean their hands

    /// <summary>
    /// When the player hand collides with the sanitizer, change the hand state to sanitized. The player can put one or both hands into the sanitizer to clean their hands. Both hands must be sanitized before the player can put on gloves.
    /// </summary>
    /// <param name="collider">The player hand collider</param>
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Grabber")
        {
            if (
                collider.transform.parent.name == "LeftController"
                && GameManager.Instance.LeftHand == GameManager.PlayerHandState.Unsanitized
            )
            {
                GameManager.Instance.LeftHand = GameManager.PlayerHandState.Sanitized;
                GameManager.Instance.PlaySound("correct");
            }
            else if (GameManager.Instance.RightHand == GameManager.PlayerHandState.Unsanitized)
            {
                GameManager.Instance.RightHand = GameManager.PlayerHandState.Sanitized;
                GameManager.Instance.PlaySound("correct");
            }
        }
    }
}
