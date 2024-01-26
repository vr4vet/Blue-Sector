using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodControls : MonoBehaviour {
    private float lifeTime;
    private ParticleSystem pSystem;

    public ParticleSystem.Particle[] particle;
    public int maxTimeRange;
    public int minTimeRange;

    public void StartFood() {
        pSystem.Play();
        Invoke("ChangeParticleLifeTime", Random.Range(minTimeRange, maxTimeRange));
    }

    public void ChangeParticleLifeTime() {
        StartCoroutine(ChangeLifeTime(pSystem, 30f));
    }

   
    // Use this for initialization
    void Start() {
        pSystem = gameObject.GetComponent<ParticleSystem>();
#pragma warning disable CS0618 // Type or member is obsolete
        lifeTime = pSystem.startLifetime;
        lifeTime = 2f;
#pragma warning restore CS0618 // Type or member is obsolete
        pSystem.Stop();
        
    }

    private IEnumerator ChangeLifeTime(ParticleSystem pSys, float lifeTime) {
#pragma warning disable CS0618 // Type or member is obsolete
        pSys.startLifetime = lifeTime;
#pragma warning restore CS0618 // Type or member is obsolete
        yield return null;
    }
}
