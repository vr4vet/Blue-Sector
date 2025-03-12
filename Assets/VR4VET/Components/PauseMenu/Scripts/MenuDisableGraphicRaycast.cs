using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDisableGraphicRaycast : MonoBehaviour
{
    private NewMenuManger _newMenuManger;
    [SerializeField] private List<GraphicRaycaster> graphicRaycasters = new();

    // Start is called before the first frame update
    void Start()
    {
        _newMenuManger = FindObjectOfType<NewMenuManger>();
        if (_newMenuManger)
            _newMenuManger.m_MenuToggled.AddListener(OnMainMenuToggled);
    }


    /// <summary>
    /// Disables graphic raycaster components when main menu is open.
    /// This prevents the dialogue box from obstructing the main menu.
    /// </summary>
    /// <param name="open"></param>
    private void OnMainMenuToggled(bool open)
    {
        foreach (GraphicRaycaster raycaster in graphicRaycasters)
            raycaster.enabled = !open;
    }
}
