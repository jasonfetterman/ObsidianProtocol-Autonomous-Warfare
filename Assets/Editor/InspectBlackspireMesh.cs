using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class InspectBlackspireMesh
{
    [MenuItem("Obsidian Protocol/World Building/Inspect Blackspire Mesh")]
    public static void Inspect()
    {
        const string modelPath =
            "Assets/Art/Models/Map designs/Blackspire Expanse/Meshy_AI_Realistic_sci_fi_mili_1015104137_texture.fbx";

        GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>(modelPath);
        if (model == null)
        {
            Debug.LogError("[BLACKSPIRE INSPECT] FBX could not be loaded: " + modelPath);
            return;
        }

        Scene preview = EditorSceneManager.NewPreviewScene();
        GameObject instance = null;

        try
        {
            instance = (GameObject)PrefabUtility.InstantiatePrefab(model, preview);
            if (instance == null)
            {
                Debug.LogError("[BLACKSPIRE INSPECT] Could not instantiate FBX preview.");
                return;
            }

            foreach (Transform t in instance.GetComponentsInChildren<Transform>(true))
            {
                Debug.Log("[BLACKSPIRE TRANSFORM] " + t.name +
                    " | Local scale: " + t.localScale.ToString("F4") +
                    " | World scale: " + t.lossyScale.ToString("F4") +
                    " | Local rotation: " + t.localEulerAngles.ToString("F2"));
            }

            var renderers = instance.GetComponentsInChildren<Renderer>(true);
            var filters = instance.GetComponentsInChildren<MeshFilter>(true);
            Bounds combined = new Bounds();
            bool hasBounds = false;

            foreach (Renderer renderer in renderers)
            {
                if (!hasBounds)
                {
                    combined = renderer.bounds;
                    hasBounds = true;
                }
                else
                {
                    combined.Encapsulate(renderer.bounds);
                }

                foreach (Material material in renderer.sharedMaterials)
                {
                    if (material != null)
                    {
                        Debug.Log("[BLACKSPIRE MATERIAL] " + material.name +
                            " | Shader: " +
                            (material.shader != null ? material.shader.name : "None"));
                    }
                }
            }

            Debug.Log("[BLACKSPIRE INSPECT] Model: " + modelPath);
            Debug.Log("[BLACKSPIRE INSPECT] Renderer count: " + renderers.Length);
            Debug.Log("[BLACKSPIRE INSPECT] MeshFilter count: " + filters.Length);

            if (hasBounds)
            {
                Debug.Log("[BLACKSPIRE INSPECT] Bounds size XYZ: " +
                    combined.size.ToString("F2") +
                    " | Center: " + combined.center.ToString("F2"));
            }
            else
            {
                Debug.LogWarning("[BLACKSPIRE INSPECT] No renderers found in the FBX.");
            }

            foreach (MeshFilter filter in filters)
            {
                if (filter.sharedMesh != null)
                {
                    Debug.Log("[BLACKSPIRE MESH] " + filter.name +
                        " | Mesh: " + filter.sharedMesh.name +
                        " | Vertices: " + filter.sharedMesh.vertexCount +
                        " | Local bounds: " + filter.sharedMesh.bounds.size.ToString("F2"));
                }
            }
        }
        finally
        {
            if (instance != null)
                Object.DestroyImmediate(instance);

            EditorSceneManager.ClosePreviewScene(preview);
        }
    }
}

