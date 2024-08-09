using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterHit : MonoBehaviour
{
    [SerializeField] private GameObject splash;
    [SerializeField] private AudioClip audio;
    private MerdFishSystem merdFishSystem;
    private int _spawnNumber = 0;
    public int SpawnNumber { get => _spawnNumber; set => _spawnNumber = value; }
    // Start is called before the first frame update
    void Start()
    {
        merdFishSystem = FindObjectOfType<MerdFishSystem>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Water"))
        {
            Vector3 updatedPos = new Vector3(transform.position.x, 0.1f, transform.position.z);
            Instantiate(splash, updatedPos, splash.transform.rotation);
            ParticleSystem particleSystem = splash.GetComponent<ParticleSystem>();
            particleSystem.Play();
            if (SpawnNumber == 1)
            {
                PlayAudio();
                Vector3 targetPos = new Vector3(transform.position.x, -1f, transform.position.z);
                merdFishSystem.GenerateFish(targetPos);
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("OceanFloor"))
        {
            Destroy(this.gameObject);
        }
    }

    public void PlayAudio()
    {   //otherwise create audiosource
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.volume = 0.5f;
        newAudioSource.PlayOneShot(audio);
    }
}
