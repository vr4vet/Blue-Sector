using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFishBoundary : MonoBehaviour {
    private Vector3 startPos;

    public float distance;

    public void MoveUp() {
        Vector3 newPos = startPos - new Vector3(0f, -distance, 0f);
        StartCoroutine(MoveToPosition(transform, newPos, 2.0f));
    }
    public void MoveDown() {
        StartCoroutine(MoveToPosition(transform, startPos, 2.0f));
    }

    // Use this for initialization
    void Start () {
        startPos = gameObject.transform.position;
	}
	
    private IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove) {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1) {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
    }
}
