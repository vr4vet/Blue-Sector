using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FishSystemScript : MonoBehaviour
{
    public GameObject fish;
    private ParticleSystem foodParticles;
    public float radius = 10;
    public float height = 20;
    public float amountOfFish = 30;
    public float fullnessDivider = 0.7f;
    public float fullnessLimit = 70;
    public float hungerRate = 3.0f;
    public float swimSpeedVertical = 0.5f;
    public float swimSpeedHorizontal = 1.0f;
    public bool feeding = false;    // all fish in the top part ("hunger zone") will be fed when this is true

    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = gameObject.transform.position;
        foodParticles = gameObject.GetComponent<ParticleSystem>();
        // positioning particles at top of fish system
        for (int i = 0; i < amountOfFish; i++)
        {
            GameObject newFish = Instantiate(fish, new Vector3(Random.Range(position.x - radius + 3, position.x + radius - 3), Random.Range(position.y - (height/2)+ 3,position.y + (height/2)- 3), Random.Range(position.z -radius + 3, position.z + radius - 3)), fish.transform.rotation);
            newFish.transform.parent = gameObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            feeding = true;
            foodParticles.Play();
        } else
        {
            feeding = false;
            foodParticles.Stop();
        }
    }
}

[CustomEditor(typeof(FishSystemScript))]
public class FishSystemVisualization : Editor
{
    public void OnSceneGUI()
    {
        // getting parameters for drawing visualization
        var t = target as FishSystemScript;
        var transform = t.transform;
        var position = t.transform.position;
        float height = t.height;
        float radius = t.radius;

        // drawing top and bottom of system
        float top = t.transform.position.y + (height / 2);  // top of merd/water surface
        float bottom = t.transform.position.y - (height / 2); // bottom of merd
        Handles.color = Color.green;
        Handles.DrawWireDisc(position + new Vector3(0, top, 0), transform.up, radius);
        Handles.DrawWireDisc(position + new Vector3(0, bottom, 0), transform.up, radius);
        
        // drawing vertical lines form top to bottom
        Handles.DrawLine(position + new Vector3(radius, top, 0), position + new Vector3(radius, bottom, 0));
        Handles.DrawLine(position + new Vector3(-radius, top, 0), position + new Vector3(-radius, bottom, 0));
        Handles.DrawLine(position + new Vector3(0, top, radius), position + new Vector3(0, bottom, radius));
        Handles.DrawLine(position + new Vector3(0, top, -radius), position + new Vector3(0, bottom, -radius));

        // drawing the divider between the zones of hungry and full fish
        float fullnessDivider = bottom + (((bottom - top) * (t.fullnessDivider)) * -1); // border between hungry and full fish
        Handles.color = Color.red;
        Handles.DrawWireDisc(position + new Vector3(0, fullnessDivider, 0), transform.up, radius);

        // moving food particle system to the top of fish system
        var foorParticlesShape = GameObject.Find("FishSystem").GetComponent<ParticleSystem>().shape;
        foorParticlesShape.position = new Vector3(foorParticlesShape.position.x, GameObject.Find("FishSystem").transform.position.y + (height / 2), foorParticlesShape.position.z);

    }

}
