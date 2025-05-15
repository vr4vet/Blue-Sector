using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshHighlightGroup : MonoBehaviour
{
    [SerializeField] private List<MeshHighlight> meshHighlights = new();

    public void EnableMeshHighlightExclusive(MeshHighlight meshHighlight)
    {
        foreach (MeshHighlight m in meshHighlights)
        {
            if (m != meshHighlight)
                m.DisableHighlight();
            else
                m.EnableHighlight();
        }
    }

    public void BlockLaserPointerAllOrgans(bool block)
    {
        foreach (MeshHighlight m in meshHighlights)
            m.GetComponent<PointerEvents>().enabled = !block;
    }
}
