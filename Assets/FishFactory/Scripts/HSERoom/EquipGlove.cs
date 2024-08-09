using UnityEngine;
using UnityEngine.Events;

public class EquipGlove : MonoBehaviour
{
    public UnityEvent OnEquip;
    // Player should be able to change the glove they are wearing on either hand
    // The player needs to wear a steel glove and a blue glove on either hand to progress to the factory

    /// <summary>
    /// When the player collides with the glove, equip it
    /// </summary>
    /// <param name="collider">The player hand collider</param>
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Grabber")
        {
            GameManager.PlayerHandState gloveType =
                name == "SteelGlove"
                    ? GameManager.PlayerHandState.SteelGlove
                    : GameManager.PlayerHandState.BlueGlove;

            if (
                collider.transform.parent.name == "LeftController"
                && GameManager.Instance.RightHand != GameManager.PlayerHandState.Unsanitized
            )
            {
                GameManager.Instance.LeftHand = gloveType;
                GameManager.Instance.PlaySound("correct");
                OnEquip.Invoke();
            }
            else if (GameManager.Instance.RightHand != GameManager.PlayerHandState.Unsanitized)
            {
                GameManager.Instance.RightHand = gloveType;
                GameManager.Instance.PlaySound("correct");
                OnEquip.Invoke();
            }
            else
            {
                GameManager.Instance.PlaySound("incorrect");
            }
        }
    }
}
