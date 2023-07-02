using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {
    private void Update() {

        transform.root.position = transform.position - transform.localPosition;
    }

}