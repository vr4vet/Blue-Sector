using UnityEngine;

public class PointingScript : MonoBehaviour
{
    private GameObject _objectToLookAt;
    private Quaternion _initialRotation;
    private bool _initialRotationSet = false;
    
    // This method is used to change the rotation of the NPC to look at a specific object
    public bool ChangeDirection(string objectName, Transform npcTransform)
    {
        // The intial rotation of the NPC is stored before changing it, so that it can be reset. 
        if (!_initialRotationSet)
        {
            _initialRotation = npcTransform.rotation;
            _initialRotationSet = true;
        }
        
        var vector3 = npcTransform.position;
        vector3.y = npcTransform.position.y;
        npcTransform.position = vector3;
        
        // Find the object in the scene which corresponds to what _objectToLookAt is set to
        _objectToLookAt = GameObject.Find(objectName);
        
        if (_objectToLookAt == null)
        {
            Debug.LogError("Object to look at not found");
            return false;
        }
        
        // Look at the correct object based on the dialogue text
        Vector3 direction = _objectToLookAt.transform.position - npcTransform.position;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            npcTransform.rotation = Quaternion.LookRotation(direction); 
        }
        return true;
    }
    
    // Reset the direction of the NPC
    public void ResetDirection(Transform npcTransform)
    {
        // The NPC is reset to its initial rotation if the initial rotation has been set
       if (_initialRotationSet)
       {
           npcTransform.rotation = _initialRotation;
           _initialRotationSet = false;
       }

    } 
}
