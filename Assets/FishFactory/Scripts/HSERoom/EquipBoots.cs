using UnityEngine;
using UnityEngine.Events;

public class EquipBoots : MonoBehaviour
{
    public UnityEvent OnEquip;
    /// <summary>
    /// When the player's feet or body-collider of choice collide with the boots, equip them
    /// </summary>
    /// <param name="collision">The player's feet collider</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (
            collision.gameObject.tag == "Player"
            && gameObject.GetComponent<BootsState>().Boots == BootsState.BootsStatus.Clean
        )
        {
            GameManager.Instance.BootsOn = true;
            GameManager.Instance.PlaySound("correct");
            Destroy(gameObject);
            OnEquip.Invoke();
        }
    }
}
