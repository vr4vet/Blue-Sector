using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attached on the NPCSpawner prefab in the reception scene.
/// </summary>

// This should probably be changed such that you can choose an npc instead of it just being the indexed 0 one
// That way it can be set and adjusted for any npc in the scene, would be nice to have.
public class SpecializedBehaviourReceptionist : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Height adjustment for the dialouge canvas")]
    private float dialogueCanvasHeightAdjustment = 1.42f;

    private NPCSpawner _npcSpawner;
    private GameObject _receptionistNpc;

    // Start is called before the first frame update
    void Start()
    {
        _npcSpawner = GetComponent<NPCSpawner>();
        if (_npcSpawner == null)
        {
            Debug.LogError("No NPCSpawner found");
            return;
        }

        _receptionistNpc = _npcSpawner._npcInstances[0];

        // -- Dialogue canvas -- \\
        Transform dialogueCanvas = _receptionistNpc.transform.GetChild(1);
        dialogueCanvas.localPosition = new Vector3(dialogueCanvas.localPosition.x, dialogueCanvasHeightAdjustment, dialogueCanvas.localPosition.z);

        // -- Name tag canvas -- \\
        Transform nameTagCanvas = _receptionistNpc.transform.GetChild(2);
        nameTagCanvas.localPosition = new Vector3(nameTagCanvas.localPosition.x, dialogueCanvasHeightAdjustment -0.5f, nameTagCanvas.localPosition.z);
    }
}
