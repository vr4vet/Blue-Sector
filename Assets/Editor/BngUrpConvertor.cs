namespace Unity.Template.VR.Editor
{
    using UnityEditor;
    using UnityEngine;
    using static UnityEngine.GUILayout;
    using System.Collections;
    using System.Collections.Generic;

    public class BngUrpConvertor : EditorWindow
    {
        private string path;
        private string[] guids;
        private string targetShader = "Standard"; 
        private List<string> statusMessages = new List<string>();
        
        
        [MenuItem("Tools/URP Converter")]
        public static void ShowWindow()
        {
            GetWindow<BngUrpConvertor>("BNG URP Converter");
        }

        private void OnGUI()
        {
            GUILayout.Label("Convert BNG Standard Shaders to URP", EditorStyles.boldLabel);
            if (GUILayout.Button("Convert to URP"))
            {
                statusMessages.Clear();
                Debug.Log("Conversion to URP started...");
                
                
                // Find all materials in the BNG Framework Assets folder
                path = "Assets/BNG Framework";
                string[] guids = AssetDatabase.FindAssets("t:Material", new[] { path });
                
                statusMessages.Add($"Found {guids.Length} materials in {path}");
                statusMessages.Add($"Searching for materials with shader: {targetShader}...");

                foreach (string guid in guids)
                {
                    // Load the material using the GUID
                    string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    Material mat = AssetDatabase.LoadAssetAtPath<Material>(assetPath);
                    
                    // Find materials with the targeted shader, in this case "Standard"
                    if (mat != null && mat.shader.name == targetShader)
                    {
                        statusMessages.Add($"Shader: {mat.shader}  Material: {mat.name} Texture: {mat.mainTexture} Color: {mat.color}");
                        
                        // Save the material's original needed properties
                        Color color = mat.color;
                        if (mat.mainTexture != null)
                        {
                            Texture texture = mat.mainTexture;
                            // Convert the material to URP Lit shader and restore properties
                            mat.shader = Shader.Find("Universal Render Pipeline/Lit");
                            mat.mainTexture = texture;
                            mat.color = color;
                        }
                        else
                        {
                            mat.shader = Shader.Find("Universal Render Pipeline/Lit");
                            mat.color = color;
                        }
                        statusMessages.Add($"Converted Material: {mat.name} to URP Lit shader.");

                    }
                }
                statusMessages.Add($"Converted all materials with shader: {targetShader} to URP Lit shader.");
            }
            // Display status messages
            foreach (var msg in statusMessages)
            {
                GUILayout.Label(msg, EditorStyles.label);
            }
        }
        
    } 
}


