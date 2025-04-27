using System.Collections;
using UnityEngine;

    public class ConsentForm : MonoBehaviour
    {
        private NPCSpawner _npcSpawner;
        private GameObject _aiConsentNpc;
        private ActionManager _actionManager;

        void Start()
            {
             _npcSpawner = GetComponent<NPCSpawner>();
             if (_npcSpawner == null)
                {
                    Debug.LogError("No NPCSpawner found");
                    return;
                }

                _aiConsentNpc = _npcSpawner._npcInstances[2];

                // Moves the dialogue canvas to Aisha higher
                Transform dialogueCanvas = _aiConsentNpc.transform.GetChild(1);
                dialogueCanvas.localPosition = new Vector3(dialogueCanvas.localPosition.x, 1.68f, dialogueCanvas.localPosition.z);

                _actionManager = ActionManager.Instance;
                ButtonSpawner.OnAnswer += ConsentForm_OnAnswer;
        }

        private void ConsentForm_OnAnswer(string answer, string question, string name)
    {
        // Only listen to when the buttons belonging to AI Consent NPC's dialogue tree are pressed
        if (name == _aiConsentNpc.name)
        {
            if (question == "Would you like to enable AI features?" && answer == "Yes")
            {
               _actionManager.SetAIEnabled(true);
                Debug.Log("User has consented to data usage.");
            }
            else
            {
                _actionManager.SetAIEnabled(false);
                Debug.Log("User has not consented to data usage.");
            }
        }
    }

    void OnDestroy()
    {
        ButtonSpawner.OnAnswer -= ConsentForm_OnAnswer;
    }
}
