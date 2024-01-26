using UnityEngine;
using System.Collections;
using System;

public class ExampleUseof_MeshCut : MonoBehaviour {

    public Material capMaterial;
    public GameObject[] pieces;
    
    void Start () {


		
	}
	
	void Update(){
	}
    
    // Triggers the MeshCut and gets the two pieces of the object
    public void cutting()
    {
        
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {

            GameObject victim = hit.collider.gameObject;
            if (victim.tag == "Cuttable")
            {
                pieces = new GameObject[1];
                pieces = BLINDED_AM_ME.MeshCut.Cut(victim, transform.position, transform.right, capMaterial);
                // Adds components to the right side of the cut object because the object is new

                try
                {
                    if (pieces[1].GetComponent<Renderer>().bounds.size.magnitude < 0.15)
                    {
                        Destroy(pieces[1]);
                    }
                    if (pieces[0].GetComponent<Renderer>().bounds.size.magnitude < 0.15)
                    {
                        Destroy(pieces[0]);
                    }
                    if (pieces[1].name == "right side")
                    {
                        if (pieces[1].GetComponent<Renderer>().bounds.size.magnitude > 0.2)
                        {
                            pieces[1].AddComponent<MeshCollider>().convex = true;
                        }
                        else
                        {
                            pieces[1].AddComponent<BoxCollider>();
                        }
                        pieces[1].AddComponent<Rigidbody>().mass = 1;
                        pieces[1].AddComponent<Valve.VR.InteractionSystem.ComplexThrowable>();
                        pieces[1].AddComponent<Valve.VR.InteractionSystem.Throwable>();
                        pieces[1].AddComponent<Valve.VR.InteractionSystem.VelocityEstimator>();
                        pieces[1].AddComponent<DestroyOnTrigger>();
                        if (pieces[1].GetComponent<Renderer>().bounds.size.magnitude > 0.2)
                        {
                            pieces[1].tag = "Cuttable";
                        }

                    }
                    //Creates new MeshCollider for the left side of the cut object, this side already has the components from the cut object
                    Destroy(pieces[0].GetComponent<MeshCollider>());
                    Destroy(pieces[0].GetComponent<BoxCollider>());
                    Destroy(pieces[0].GetComponent<SphereCollider>());
                    if (pieces[0].GetComponent<Renderer>().bounds.size.magnitude > 0.2){
                        pieces[0].AddComponent<MeshCollider>().convex = true;
                    }
                    else{
                        pieces[0].AddComponent<BoxCollider>();
                    }
                    
                    if (pieces[0].GetComponent<Renderer>().bounds.size.magnitude < 0.2)
                    {
                        pieces[0].tag = "Untagged";
                    }
                    //UnityEditor.Selection.activeGameObject = pieces[1];
                }
                catch (NullReferenceException e)
                {
                    throw;
                }
            }
        }
    }
    // Detects object to cut
    void OnDrawGizmosSelected() {

		Gizmos.color = Color.green;

		Gizmos.DrawLine(transform.position, transform.position + transform.forward * 0.2f);
		Gizmos.DrawLine(transform.position + transform.up * 0.2f, transform.position + transform.up * 0.2f + transform.forward * 0.2f);
		Gizmos.DrawLine(transform.position + -transform.up * 0.2f, transform.position + -transform.up * 0.2f + transform.forward * 0.2f);

		Gizmos.DrawLine(transform.position, transform.position + transform.up * 0.2f);
		Gizmos.DrawLine(transform.position,  transform.position + -transform.up * 0.2f);

	}

}
