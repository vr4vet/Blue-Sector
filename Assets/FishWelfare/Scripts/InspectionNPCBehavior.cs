using BNG;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class InspectionNPCBehavior : MonoBehaviour 
{
    [HideInInspector] private NPCSpawner _npcSpawner;
    [HideInInspector] private GameObject _npc;
    [SerializeField] private GameObject ratingCanvas;
    private bool active = false;
    private Transform dialogueCanvas;
    private GameObject dialogueCanvasObject;
    private ConversationController _conversationController;
    private DialogueBoxController _dialogueBoxController;
    public bool dialogueStarted = false;

    // Start is called before the first frame update
    void Start() {

        _npcSpawner = GetComponent<NPCSpawner>();

        // Get the second NPC, which is the guide that will receive inspection input
        _npc = _npcSpawner._npcInstances[1];

        dialogueCanvasObject = _npc.transform.Find("DialogueCanvasV2").gameObject;
        dialogueCanvas = _npc.transform.Find("DialogueCanvasV2");
        dialogueCanvas.localPosition = new Vector3(dialogueCanvas.localPosition.x, dialogueCanvas.localPosition.y+0.4f, dialogueCanvas.localPosition.z);
        // Make the ratingCanvas a child of the dialogue canvas for the NPC
        ratingCanvas.transform.SetParent(_npc.transform);

        // Give the ratingCanvas the CameraCaster to fix targeting
        GameObject cameraCaster = GameObject.Find("CameraCaster");
        Camera eventCamera = cameraCaster.GetComponent<Camera>();
        ratingCanvas.GetComponent<Canvas>().worldCamera = eventCamera;

        _conversationController = _npc.GetComponentInChildren<ConversationController>();
        // ShouldTrigger false makes sure proximity dialogue activation is off


        //Change ratingCanvas position to position of dialogue canvas
        ratingCanvas.transform.localPosition = dialogueCanvas.localPosition;

        // After intro dialogue ends branch to rating canvas methods
        _dialogueBoxController = _npc.GetComponentInChildren<DialogueBoxController>();
        DialogueBoxController.OnDialogueEnded += OnFinishDialogue;

        //ToggleActiveRatingCanvas();
    }

    void Update() {
        // Makes sure that if the rating canvas is active, dialogueCavas is not
        if (ratingCanvas.activeSelf == true && dialogueCanvasObject.activeSelf == true) {
            dialogueCanvasObject.SetActive(false);
            Debug.Log("Setting to false");
        }
    }

    public void StartInspectionDialogue() {
        _conversationController.DialogueTrigger();
    }

    public void ToggleActiveRatingCanvas() {
        active = !active;
        ratingCanvas.SetActive(active);
    }

    public void OnFinishDialogue(String name) {
        if (name != _npc.name) { return; }
        ToggleActiveRatingCanvas();  
    }



}