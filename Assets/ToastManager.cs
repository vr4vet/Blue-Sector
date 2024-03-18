using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastManager : MonoBehaviour
{
    private MaintenanceManager manager;
    [SerializeField] private GameObject toast;

    void Start()
    {
        manager = gameObject.GetComponent<MaintenanceManager>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void DisplayToast(GameObject toast, Vector3 position, Vector3 rotation)
    {
        Instantiate(toast, position, Quaternion.Euler(rotation));
        // toast.GetComponent < Flo
    }
}
