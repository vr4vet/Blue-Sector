using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stream : MonoBehaviour
{
    private LineRenderer lineRenderer = null;
    private Vector3 targetPosition = Vector3.zero;
    private ParticleSystem splashParticle = null;
    private Coroutine pourRoutine = null;

    private TankController target = null;

    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        splashParticle = GetComponentInChildren<ParticleSystem>();
    }

    private void Start() {
        MovetoPosition(0, transform.position);
        MovetoPosition(1, transform.position);
    }

    public void Begin() {
        StartCoroutine(UpdateParticle());
        pourRoutine = StartCoroutine(BeginPour());
    }

    private IEnumerator BeginPour() {
        while(gameObject.activeSelf) {
            targetPosition = findEndPoint(); 
            MovetoPosition(0, transform.position);
            AnimateToPosition(1, targetPosition);
            yield return null;
        }
    }

    public void End() {
        StopCoroutine(pourRoutine);
        pourRoutine = StartCoroutine(EndPour());
    }

    private IEnumerator EndPour() {
        while(!HasReachedPosition(0, targetPosition)) {
            AnimateToPosition(0, targetPosition);
            AnimateToPosition(1, targetPosition);
            yield return null;
        }

        Destroy(gameObject);
    }

    private Vector3 findEndPoint() {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        Physics.Raycast(ray, out hit, 10f);
        if(hit.collider.transform.gameObject.GetComponent<TankController>() != null){
            target = hit.collider.transform.gameObject.GetComponent<TankController>();
        }
        Vector3 endPoint = hit.collider ? hit.point : ray.GetPoint(10f);
        return endPoint;
    }

    private void MovetoPosition(int index, Vector3 targetPosition) {
        lineRenderer.SetPosition(index, targetPosition);
    }

    public void AnimateToPosition(int index, Vector3 targetPosition) {
        Vector3 currentPoint = lineRenderer.GetPosition(index);
        Vector3 newPosition = Vector3.MoveTowards(currentPoint, targetPosition, Time.deltaTime * 1.75f);
        lineRenderer.SetPosition(index, newPosition);
    }

    private bool HasReachedPosition(int index, Vector3 targetPosition) {
        Vector3 currentPosition = lineRenderer.GetPosition(index);
        return currentPosition == targetPosition;
    }

    private IEnumerator UpdateParticle() {
        while(gameObject.activeSelf) {
            splashParticle.gameObject.transform.position = targetPosition;
            bool isHitting = HasReachedPosition(1, targetPosition);
            splashParticle.gameObject.SetActive(isHitting);
            if(target != null) {
                target.sedativeConsentration += 0.00005f;
            }
            yield return null;
        }
    }
}   
