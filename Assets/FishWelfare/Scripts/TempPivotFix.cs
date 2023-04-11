using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPivotFix : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        rectTransform.pivot = new Vector2(mousePosition.x, mousePosition.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
