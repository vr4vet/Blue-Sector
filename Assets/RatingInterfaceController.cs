using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatingInterfaceController : MonoBehaviour
{

private bool active = false;

void Start() {
    gameObject.SetActive(active);
}

public void ToggleActive() {
    active = !active;
    gameObject.SetActive(active);
}

public void SeteActive(bool active) {
    this.active = active;
    gameObject.SetActive(active);
}
}
