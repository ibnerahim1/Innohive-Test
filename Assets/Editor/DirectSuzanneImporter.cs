using UnityEngine;
using UnityEditor;
using System.IO;

public static class DirectSuzanneImporter
{
    [MenuItem("Assets/Import Suzanne")]
    public static void ImportSuzanne()
    {
        string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        string fbxFilePath = Path.Combine(desktopPath, "Suzanne.fbx");
        
        Debug.Log("Looking for Suzanne at: " + fbxFilePath);
        
        if (File.Exists(fbxFilePath))
        {
            // Create Models directory if it doesn't exist
            if (!Directory.Exists("Assets/Models"))
            {
                Directory.CreateDirectory("Assets/Models");
                AssetDatabase.Refresh();
            }
            
            // Define the destination path inside our Unity project
            string destinationPath = "Assets/Models/Suzanne.fbx";
            
            try
            {
                // Copy the file from the desktop to our project's Assets folder
                File.Copy(fbxFilePath, destinationPath, true);
                
                // Refresh the Asset Database to detect the new file
                AssetDatabase.Refresh();
                
                Debug.Log("Suzanne has been imported to: " + destinationPath);
                
                // Load the asset
                GameObject suzanneAsset = AssetDatabase.LoadAssetAtPath<GameObject>(destinationPath);
                if (suzanneAsset != null)
                {
                    // Instantiate in scene
                    GameObject suzanne = GameObject.Instantiate(suzanneAsset);
                    suzanne.name = "Suzanne";
                    Debug.Log("Suzanne has been added to the scene!");
                }
                else
                {
                    Debug.LogError("Failed to load Suzanne asset after import");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error copying Suzanne: " + e.Message);
            }
        }
        else
        {
            Debug.LogError("Suzanne FBX file not found at: " + fbxFilePath);
        }
    }
}
