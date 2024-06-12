using System;
using NUnit.Framework;
using UnityEngine;

public class Despawner : MonoBehaviour
{
    /// <summary>
    /// Destroy the fish object when it collides with the despawner
    /// </summary>
    /// <param name="collider">The head bone collider of the fish object</param>
    GameObject colliderObject;

    private void OnTriggerEnter(Collider collider)
    { 
        colliderObject = collider.gameObject;
        try 
        {
            colliderObject = collider.transform.parent.transform.parent.gameObject;
        }
        catch (NullReferenceException e)
        {
            IgnoreException.Equals(e.Message, "Not a fish");
        }

        if (colliderObject.tag != "Fish")
        {
            return;
        }
        // Destroy the main fish object
        Destroy(colliderObject);
    }
}
