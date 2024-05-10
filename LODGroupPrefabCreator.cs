using UnityEditor;
using UnityEngine;

public class LODGroupPrefabCreator : EditorWindow
{
    private GameObject[] selectedMeshes;
    private string prefabName = "NewLODPrefab";
    private float[] lodThresholds;
    private string saveFolderPath = "Assets";
    private bool addColliderToLOD0 = false;

    // Add menu item to show the window
    [MenuItem("Tools/LOD Group Prefab Creator")]
    private static void ShowWindow()
    {
        GetWindow<LODGroupPrefabCreator>("LOD Group Prefab Creator");
    }

    // Draw the window's content
    private void OnGUI()
    {
        EditorGUILayout.LabelField("LOD Group Prefab Creator", EditorStyles.boldLabel);

        prefabName = EditorGUILayout.TextField("Prefab Name", prefabName);

        // Display selected meshes
        EditorGUILayout.LabelField("Select Meshes from Project View:");
        GameObject[] newSelectedMeshes = Selection.gameObjects;
        if (newSelectedMeshes != selectedMeshes)
        {
            selectedMeshes = newSelectedMeshes;
            lodThresholds = new float[selectedMeshes.Length];  // Initialize thresholds array based on selection count
            for (int i = 0; i < lodThresholds.Length; i++)
            {
                lodThresholds[i] = 1f - (i / (float)lodThresholds.Length);  // Default decreasing thresholds
            }
        }

        if (selectedMeshes != null && selectedMeshes.Length > 0)
        {
            for (int i = 0; i < selectedMeshes.Length; i++)
            {
                EditorGUILayout.LabelField($"{i + 1}: {selectedMeshes[i].name}");
            }
        }
        else
        {
            EditorGUILayout.LabelField("No meshes selected!");
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("LOD Settings:");
        for (int i = 0; i < lodThresholds.Length; i++)
        {
            lodThresholds[i] = EditorGUILayout.Slider($"LOD {i + 1} Threshold", lodThresholds[i], 0f, 1f);
        }

        EditorGUILayout.Space();
        addColliderToLOD0 = EditorGUILayout.Toggle("Add Collider to LOD0", addColliderToLOD0);

        EditorGUILayout.Space();
        if (GUILayout.Button("Select Save Folder"))
        {
            string selectedPath = EditorUtility.OpenFolderPanel("Select Prefab Save Folder", saveFolderPath, "");
            if (!string.IsNullOrEmpty(selectedPath) && selectedPath.StartsWith(Application.dataPath))
            {
                saveFolderPath = "Assets" + selectedPath.Substring(Application.dataPath.Length);
            }
            else if (string.IsNullOrEmpty(selectedPath))
            {
                Debug.LogWarning("Save folder selection was canceled.");
            }
            else
            {
                Debug.LogError("Invalid save path selected. Please choose a folder within the project.");
            }
        }

        EditorGUILayout.LabelField("Save Folder: ", saveFolderPath);

        if (GUILayout.Button("Create LOD Prefab"))
        {
            CreateLODPrefab();
        }
    }

    // Method to create LOD Prefab
    private void CreateLODPrefab()
    {
        if (selectedMeshes == null || selectedMeshes.Length == 0)
        {
            Debug.LogWarning("No meshes selected for creating the LOD group prefab.");
            return;
        }

        // Create a new GameObject for the prefab
        GameObject lodRoot = new GameObject(prefabName);
        LODGroup lodGroup = lodRoot.AddComponent<LODGroup>();

        LOD[] lods = new LOD[selectedMeshes.Length];

        for (int i = 0; i < selectedMeshes.Length; i++)
        {
            GameObject lodObj = Instantiate(selectedMeshes[i], lodRoot.transform);
            lodObj.name = $"LOD {i + 1}";

            // Add a MeshCollider to LOD0 if requested
            if (i == 0 && addColliderToLOD0)
            {
                MeshCollider collider = lodObj.AddComponent<MeshCollider>();
                MeshFilter meshFilter = lodObj.GetComponent<MeshFilter>();
                if (meshFilter != null)
                {
                    collider.sharedMesh = meshFilter.sharedMesh;
                }
            }

            Renderer[] renderers = lodObj.GetComponentsInChildren<Renderer>();
            lods[i] = new LOD(lodThresholds[i], renderers);
        }

        lodGroup.SetLODs(lods);
        lodGroup.RecalculateBounds();

        // Ensure the save folder path is set
        if (string.IsNullOrEmpty(saveFolderPath))
        {
            Debug.LogError("Invalid save path. Please select a valid save folder.");
            return;
        }

        // Save as a prefab
        string path = $"{saveFolderPath}/{prefabName}.prefab";
        PrefabUtility.SaveAsPrefabAsset(lodRoot, path);
        DestroyImmediate(lodRoot);

        Debug.Log($"LOD prefab created and saved at {path}");
    }
}
