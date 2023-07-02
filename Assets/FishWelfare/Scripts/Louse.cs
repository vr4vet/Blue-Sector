using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Louse : MonoBehaviour, IPointerClickHandler
{

    public bool marked = false;
    private Transform parent;
    private Transform boneParent;

    private void Start() {
        parent = transform.parent;
        boneParent = parent.parent;
    }

    private void Update() {
        //parent.position = boneParent.position;
    }

    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log("pointer event from louse");
        boneParent.gameObject.GetComponent<Bone>().MarkLouse(eventData);
    }
}
