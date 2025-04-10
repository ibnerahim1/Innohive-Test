using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SuzanneImporter : MonoBehaviour
{
    public string desktopPath;
    
    void Start()
    {
        desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
    }
    
#if UNITY_EDITOR
    [ContextMenu("Import Suzanne")]
    public void ImportSuzanne()
    {
        string fbxFilePath = Path.Combine(desktopPath, "Suzanne.fbx");
        Debug.Log("Looking for Suzanne at: " + fbxFilePath);
        
        if (File.Exists(fbxFilePath))
        {
            // Define the destination path inside our Unity project
            string destinationPath = "Assets/Models/Suzanne.fbx";
            
            // Copy the file from the desktop to our project's Assets folder
            File.Copy(fbxFilePath, destinationPath, true);
            
            // Refresh the Asset Database to detect the new file
            AssetDatabase.Refresh();
            
            Debug.Log("Suzanne has been imported to: " + destinationPath);
        }
        else
        {
            Debug.LogError("Suzanne FBX file not found at: " + fbxFilePath);
        }
    }
#endif
}
