using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChildToggle : MonoBehaviour
{
    [SerializeField] private Toggle parentToggle, childToggle;
    // Start is called before the first frame update
    void Start()
    {
        if (parentToggle == null || childToggle == null)
            Debug.LogError("Both toggles need to be set in the Inspector");

        parentToggle.onValueChanged.AddListener(delegate
        {
            OnParentToggled(parentToggle.isOn);
        });
    }

    private void OnParentToggled(bool isOn) => childToggle.interactable = isOn; 
}
