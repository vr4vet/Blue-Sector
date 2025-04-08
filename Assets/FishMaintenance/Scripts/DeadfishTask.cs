using UnityEngine;

public class DeadfishTask : MonoBehaviour
{
    [SerializeField] private GameObject[] deadfishEquipment;
    [SerializeField] private GameObject foldedLoader;

    public void EnableEquipment()
    {
        foreach (GameObject equipment in deadfishEquipment)
        {
            equipment.SetActive(true);
        }
        foldedLoader.SetActive(false);
    }
}
