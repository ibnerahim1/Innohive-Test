using UnityEngine;
using UnityEditor;
using System.IO;

public class ImportSuzanne : MonoBehaviour
{
    public string fbxFilePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "Suzanne.fbx");
    
    [ContextMenu("Import Suzanne")]
    public void ImportSuzanneModel()
    {
        if (File.Exists(fbxFilePath))
        {
            // Define the destination path inside our Unity project
            string destinationPath = "Assets/Models/Suzanne.fbx";
            
            // Copy the file from the desktop to our project's Assets folder
            File.Copy(fbxFilePath, destinationPath, true);
            
            // Refresh the Asset Database to detect the new file
            AssetDatabase.Refresh();
            
            Debug.Log("Suzanne has been imported to: " + destinationPath);
            
            // Import the model (by loading the asset)
            GameObject suzanneModel = AssetDatabase.LoadAssetAtPath<GameObject>(destinationPath);
            if (suzanneModel != null)
            {
                // Instantiate the model in the scene
                GameObject instance = Instantiate(suzanneModel);
                instance.name = "Suzanne";
                
                Debug.Log("Suzanne has been added to the scene!");
            }
            else
            {
                Debug.LogError("Failed to load the Suzanne model after import.");
            }
        }
        else
        {
            Debug.LogError("Suzanne FBX file not found at: " + fbxFilePath);
        }
    }
}
