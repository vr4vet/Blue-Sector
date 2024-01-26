using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour {
    public GameObject selectButton;
    public RenderTexture renderTexture;
    public float cameraRange;
    public float timeToTravel;

    private Vector3 startPos;
    private bool selected;
    //Move cameras down
    public void MoveDown() {
        Vector3 newPos = startPos - new Vector3(0f, cameraRange, 0f);
        StartCoroutine(MoveToPosition(transform, newPos, timeToTravel));
    }
    //Move cameras up
    public void MoveUp() {
        StartCoroutine(MoveToPosition(transform, startPos, timeToTravel));
    }
    //Sets starting position of player
    void Start() {
        startPos = transform.position;
        selected = false;
        gameObject.GetComponent<Camera>().enabled = false;
    }

    public void SelectCamera() {
        selected = true;
        //Resetting the texture
        StartCoroutine(ChangeRenderTexture(gameObject, null));
        //Applying the new texture
        StartCoroutine(ChangeRenderTexture(gameObject, renderTexture));
    }

    public void SetSelected(bool selected) {
        this.selected = selected;
    }

    public bool IsSelected() {
        return selected;
    }
    //Moves player to the set camera
    private IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove) {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1) {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
    }
    // Changes the camera that is viewed on the TV
    private IEnumerator ChangeRenderTexture(GameObject camera, RenderTexture texture) {
        camera.GetComponent<Camera>().targetTexture = texture;
        yield return null;
    }

}
