using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecializedBehaviourReceptionist : MonoBehaviour
{
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

        // Moves the dialogue canvas to the receptionist higher
        Transform dialogueCanvas = _receptionistNpc.transform.GetChild(1);
        dialogueCanvas.localPosition = new Vector3(dialogueCanvas.localPosition.x, 1.68f, dialogueCanvas.localPosition.z);
    }
}
