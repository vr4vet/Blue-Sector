using UnityEngine;

public class DeadfishTask : MonoBehaviour
{
    [SerializeField] private GameObject[] deadfishEquipment;
    [SerializeField] private GameObject foldedLoader;

    void OnDisable()
    {
        foreach (GameObject equipment in deadfishEquipment)
        {
            equipment.SetActive(true);
        }
        foldedLoader.SetActive(false);
    }
}
