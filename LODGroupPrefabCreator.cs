using UnityEditor;
using UnityEngine;

public class LODGroupPrefabCreator : EditorWindow
{
    private GameObject[] selectedMeshes;
    private string prefabName = "NewLODPrefab";
    private float[] lodThresholds;

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

            Renderer[] renderers = lodObj.GetComponentsInChildren<Renderer>();
            lods[i] = new LOD(lodThresholds[i], renderers);
        }

        lodGroup.SetLODs(lods);
        lodGroup.RecalculateBounds();

        // Save as a prefab
        string path = $"Assets/{prefabName}.prefab";
        PrefabUtility.SaveAsPrefabAsset(lodRoot, path);
        DestroyImmediate(lodRoot);

        Debug.Log($"LOD prefab created and saved at {path}");
    }
}
