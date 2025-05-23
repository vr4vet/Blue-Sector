using UnityEngine;
using UnityEngine.Events;

public class EquipEarProtection : MonoBehaviour
{

    public UnityEvent OnEquip;
    /// <summary>
    /// When the player head collides with the ear protection, equip it
    /// </summary>
    /// <param name="collision">The player head collider</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.Instance.EarProtectionOn = true;
            GameManager.Instance.PlaySound("correct");
            Destroy(gameObject);
            OnEquip.Invoke();
        }
    }
}
