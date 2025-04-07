using UnityEditor;
using UnityEngine;

public class ResetTelemetryConsent
{
    [MenuItem("Tools/Reset Telemetry Consent")]
    public static void ResetConsent()
    {
        EditorPrefs.DeleteKey("VoiceSdk.Telemetry.Consent");
        EditorPrefs.DeleteKey("VoiceSdk.Telemetry.ConsentDate");
        Debug.Log("Telemetry consent has been reset. Restart the editor to see the consent popup again.");
    }
}