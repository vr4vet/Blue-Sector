using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AddInstructionsToWatch : MonoBehaviour
{
    public TextMeshPro textMesh;

    void AddInstruction()
    {
        textMesh.SetText("Legg til det manglende tauet ved å ta tak i omrisset. Bruk knappen bak pekefingeren.");
    }
}
