using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointingScript : MonoBehaviour
{
    private GameObject _objectToLookAt;
    private Quaternion _initialRotation;
    private Transform _npcModel;
    private bool _initialRotationSet = false;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeDirection(int section, GameObject talkingNpc)
    {
        if (!_initialRotationSet)
        {
            _initialRotation = transform.rotation;
            _initialRotationSet = true;
        }
        
        var vector3 = transform.position;
        vector3.y = transform.position.y;
        transform.position = vector3;
        // The object to look at is stored in the dialogue tree
        _objectToLookAt = talkingNpc.GetComponent<DialogueBoxController>().dialogueTreeRestart.sections[section]
            .objectToLookAt;
        // Find the object in the scene which corresponds to the prefab that is set in the dialogue tree
        _objectToLookAt = GameObject.Find(_objectToLookAt.name);

        if (_objectToLookAt == null)
        {
            Debug.Log("Object to look at not found");
        }
        else
        {
            // Look at the correct object based on the dialogue text
            Vector3 direction = _objectToLookAt.transform.position - transform.position;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }

    }
    // Reset the direction of the NPC
    public void ResetDirection(GameObject talkingNpc)
    {
        // The NPC is reset to it's initial rotation. The initial rotation always updated in case the NPC has moved 
       /* Vector3 rotation = _initialRotation.eulerAngles;
        rotation.y = transform.rotation.eulerAngles.y;
        _initialRotation = Quaternion.Euler(rotation); */
       if (_initialRotationSet)
       {
           transform.rotation = _initialRotation;
           _initialRotationSet = false;
       }
        


    } 
}
