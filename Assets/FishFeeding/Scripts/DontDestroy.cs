using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    [SerializeField]
    private GameObject obj;

    private void Awake() 
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(obj.tag);

        if (objs.Length > 1) {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

    }
}
