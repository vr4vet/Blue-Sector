using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.OpenXR.Input;
using UnityEngine.InputSystem;
using TMPro;
using BNG;
using System;

public class AddInstructionsToWatch : MonoBehaviour
{
    public TextMeshPro textMesh;
    // [SerializeField] InputActionReference lefthand;
    // public HandController left;

    public void addInstructions()
    {
        // OpenXRInput.SendHapticImpulse(lefthand, 1, 1, 1, UnityEngine.InputSystem.XR.XRController.leftHand);

        var device = UnityEngine.InputSystem.XR.XRController.leftHand;
        var command = UnityEngine.InputSystem.XR.Haptics.SendHapticImpulseCommand.Create(0, 0.3f, 1);
        device.ExecuteCommand(ref command);

        textMesh.SetText("Legg til det manglende tauet ved å holde hånda der tauet skulle ha vært og trykke på knappen bak pekefingeren din. Du vil kjenne vibrering i hånda når du holder over riktig plass");
    }
}
