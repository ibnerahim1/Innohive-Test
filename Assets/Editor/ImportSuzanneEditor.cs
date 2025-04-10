using UnityEngine;
using UnityEditor;
using System.IO;

public class ImportSuzanneEditor : Editor
{
    [MenuItem("Tools/Import Suzanne from Desktop")]
    public static void ImportSuzanneFromDesktop()
    {
        string fbxFilePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "Suzanne.fbx");
        
        if (File.Exists(fbxFilePath))
        {
            // Define the destination path inside our Unity project
            string destinationPath = "Assets/Models/Suzanne.fbx";
            
            // Make sure the Models folder exists
            if (!AssetDatabase.IsValidFolder("Assets/Models"))
            {
                AssetDatabase.CreateFolder("Assets", "Models");
            }
            
            // Copy the file from the desktop to our project's Assets folder
            File.Copy(fbxFilePath, destinationPath, true);
            
            // Refresh the Asset Database to detect the new file
            AssetDatabase.Refresh();
            
            Debug.Log("Suzanne has been imported to: " + destinationPath);
            
            // Import settings
            ModelImporter importer = AssetImporter.GetAtPath(destinationPath) as ModelImporter;
            if (importer != null)
            {
                importer.materialImportMode = ModelImporterMaterialImportMode.ImportStandard;
                importer.SaveAndReimport();
            }
            
            // Import the model (by loading the asset)
            GameObject suzanneModel = AssetDatabase.LoadAssetAtPath<GameObject>(destinationPath);
            if (suzanneModel != null)
            {
                // Instantiate the model in the scene
                GameObject instance = PrefabUtility.InstantiatePrefab(suzanneModel) as GameObject;
                instance.name = "Suzanne";
                
                // Position the model in front of the camera
                instance.transform.position = new Vector3(0, 0, 0);
                
                // Select the object in the hierarchy
                Selection.activeGameObject = instance;
                
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
            EditorUtility.DisplayDialog("File Not Found", "Suzanne FBX file not found at: " + fbxFilePath, "OK");
        }
    }
}
