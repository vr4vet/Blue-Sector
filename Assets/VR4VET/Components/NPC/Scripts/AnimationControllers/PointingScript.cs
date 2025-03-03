using UnityEngine;

public class PointingScript : MonoBehaviour
{
    private GameObject _objectToLookAt;
    
    // This method is used to change the rotation of the NPC to look at a specific object
    public bool ChangeDirection(string objectName, Transform npcTransform)
    {
        // Find the object in the scene which corresponds to what _objectToLookAt is set to
        _objectToLookAt = GameObject.Find(objectName);
        
        if (_objectToLookAt == null)
        {
            Debug.LogError("Object to look at not found");
            return false;
        }
        
        // Look at the correct object based on the dialogue text
        Vector3 direction = npcTransform.InverseTransformPoint(_objectToLookAt.transform.position) - npcTransform.localPosition;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            npcTransform.localRotation = Quaternion.LookRotation(direction); 
        }
        return true;
    }
    
    // Reset the direction of the NPC
    public void ResetDirection(Transform npcTransform)
    {
        // The NPC is reset to its initial rotation if the initial rotation has been set
        npcTransform.localEulerAngles = Vector3.zero;
    } 
    
    // Used to get the object that the NPC is currently looking at for other scripts
    public GameObject GetObjectToPointAt()
    {
        return _objectToLookAt;
    }
}
