// Purpose: Loads environment variables from .env file (Editor Only).
// Note: This is generally unchanged from the deprecated version, but ensure it's placed correctly.
using System;
using System.IO;
using UnityEngine;

public static class EnvLoader
{
#if UNITY_EDITOR
    // Function for loading the environment variable defined in the project's root
    public static void Load(string fileName = ".env")
    {
        // Find project root relative to Assets folder
        string projectRootPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        string filePath = Path.Combine(projectRootPath, fileName);

        if (!File.Exists(filePath))
        {
            UnityEngine.Debug.LogWarning($"EnvLoader: Environment file not found at '{filePath}'. AI features requiring API keys may not work.");
            return;
        }

        int loadedCount = 0;
        try
        {
            foreach (var line in File.ReadAllLines(filePath))
            {
                // Skip comments and empty lines
                if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#"))
                    continue;

                var parts = line.Split('=', 2); // Split only on the first '='
                if (parts.Length == 2)
                {
                    string key = parts[0].Trim();
                    string value = parts[1].Trim();

                    // Remove potential surrounding quotes
                    if (value.StartsWith("\"") && value.EndsWith("\"") && value.Length >= 2)
                        value = value.Substring(1, value.Length - 2);
                    if (value.StartsWith("'") && value.EndsWith("'") && value.Length >= 2)
                        value = value.Substring(1, value.Length - 2);

                    if (!string.IsNullOrEmpty(key))
                    {
                        Environment.SetEnvironmentVariable(key, value);
                        loadedCount++;
                    }
                }
                else if (!string.IsNullOrWhiteSpace(line))
                {
                    // Log lines that are not comments/empty but don't parse correctly
                    UnityEngine.Debug.LogWarning($"EnvLoader: Skipping invalid line in .env file: '{line}'");
                }
            }

            if (loadedCount > 0)
                UnityEngine.Debug.Log($"EnvLoader: Loaded {loadedCount} environment variable(s) from '{filePath}'.");
            else
                UnityEngine.Debug.LogWarning($"EnvLoader: No valid environment variables found in '{filePath}'.");

        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"EnvLoader: Error reading .env file at '{filePath}'. Error: {e.Message}");
        }
    }

    // Automatically load before the first scene loads in the Editor
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        Load();
    }
#endif // UNITY_EDITOR

    // Add a method for runtime loading if needed, although generally discouraged for API keys
    // public static void LoadRuntime(string filePath) { ... }
}