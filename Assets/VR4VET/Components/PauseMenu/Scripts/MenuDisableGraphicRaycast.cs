using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuDisableGraphicRaycast : MonoBehaviour
{
    private NewMenuManger _newMenuManger;
    private List<GraphicRaycaster> _graphicRaycasters = new();

    /// <summary>
    /// This script disables canvases' graphics raycasters to prevent them from obstructing the main menu.
    /// Canvases that are between the player's hand laser pointers and the main menu can prevent the laser pointers from reaching the main menu canvases.
    /// This script can be placed on objects containing such offending canvases to fix the problem.
    /// </summary>

    // Start is called before the first frame update
    void Start()
    {
        _newMenuManger = FindObjectOfType<NewMenuManger>();
        if (_newMenuManger)
            _newMenuManger.m_MenuToggled.AddListener(OnMainMenuToggled);

        // getting all graphic raycaster components that are enabled (disabled components not included, otherwise OnMainMenuToggled will enable them when menu is closed)
        _graphicRaycasters = GetComponentsInChildren<GraphicRaycaster>(true).ToList().FindAll(raycaster => raycaster.enabled);
    }


    /// <summary>
    /// Disables graphic raycaster components when main menu is open.
    /// This prevents the dialogue box from obstructing the main menu.
    /// </summary>
    /// <param name="open"></param>
    private void OnMainMenuToggled(bool open)
    {
        foreach (GraphicRaycaster raycaster in _graphicRaycasters)
            raycaster.enabled = !open;
    }
}
