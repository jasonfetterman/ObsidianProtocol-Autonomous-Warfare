using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class BlackspireTabletopHologramBuilder
{
    private const string ScenePath =
        "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";
    private const string ModelPath =
        "Assets/Art/Models/Map designs/Blackspire Expanse/Meshy_AI_Realistic_sci_fi_mili_1015104137_texture.fbx";
    private const string MaterialFolder = "Assets/Art/Materials/Holograms";
    private const string MaterialPath =
        MaterialFolder + "/MAT_HOLO_BLACKSPIRE_TERRAIN.mat";
    private const string RootName = "[HOLOGRAM] BLACKSPIRE CONTINENT";

    [MenuItem("Obsidian Protocol/Deployment/Build Blackspire Tabletop Hologram")]
    public static void Build()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            Debug.LogError("[BLACKSPIRE BUILD] Exit Play Mode first.");
            return;
        }

        if (!File.Exists(ScenePath) || AssetDatabase.LoadAssetAtPath<GameObject>(ModelPath) == null)
        {
            Debug.LogError("[BLACKSPIRE BUILD] Required scene or FBX is missing. No changes made.");
            return;
        }

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            Debug.LogWarning("[BLACKSPIRE BUILD] Cancelled; current scene changes were not saved.");
            return;
        }

        string backupFolder = Path.Combine(Application.dataPath, "../Backups/Deployment");
        Directory.CreateDirectory(backupFolder);
        string backupPath = Path.Combine(
            backupFolder,
            "SCN12_DEPLOYMENT_BlackspireBackup_" +
            DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".unity");

        File.Copy(ScenePath, backupPath, false);

        Scene scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
        GameObject surface = FindInScene(scene, "[MAP] DISPLAY SURFACE");

        if (surface == null)
        {
            Debug.LogError("[BLACKSPIRE BUILD] Display surface not found. Backup: " + backupPath);
            return;
        }

        GameObject modelAsset = AssetDatabase.LoadAssetAtPath<GameObject>(ModelPath);
        if (modelAsset == null)
        {
            Debug.LogError("[BLACKSPIRE BUILD] FBX could not be loaded. Backup: " + backupPath);
            return;
        }

        if (!AssetDatabase.IsValidFolder("Assets/Art/Materials"))
            AssetDatabase.CreateFolder("Assets/Art", "Materials");
        if (!AssetDatabase.IsValidFolder(MaterialFolder))
            AssetDatabase.CreateFolder("Assets/Art/Materials", "Holograms");

        Material hologram = AssetDatabase.LoadAssetAtPath<Material>(MaterialPath);
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null)
        {
            Debug.LogError("[BLACKSPIRE BUILD] URP/Lit shader unavailable. Backup: " + backupPath);
            return;
        }

        if (hologram == null)
        {
            hologram = new Material(shader);
            hologram.name = "MAT_HOLO_BLACKSPIRE_TERRAIN";
            AssetDatabase.CreateAsset(hologram, MaterialPath);
        }

        hologram.shader = shader;
        hologram.SetColor("_BaseColor", new Color(0.006f, 0.055f, 0.075f, 1f));
        hologram.SetColor("_EmissionColor", new Color(0.02f, 0.9f, 1.0f, 1f));
        hologram.EnableKeyword("_EMISSION");
        hologram.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
        hologram.SetFloat("_Smoothness", 0.9f);
        EditorUtility.SetDirty(hologram);
        AssetDatabase.SaveAssets();

        GameObject root = FindInScene(scene, RootName);
        if (root != null)
            Undo.DestroyObjectImmediate(root);

        root = new GameObject(RootName);
        Undo.RegisterCreatedObjectUndo(root, "Create Blackspire tabletop hologram");

        root.transform.SetParent(surface.transform.parent, false);
        root.transform.position =
            surface.transform.TransformPoint(new Vector3(0f, 0.5f, 0f)) +
            surface.transform.up * 0.025f;
        root.transform.rotation = surface.transform.rotation;
        root.transform.localScale = Vector3.one * 0.0013f;

        GameObject model = (GameObject)PrefabUtility.InstantiatePrefab(modelAsset, scene);
        if (model == null)
        {
            Undo.DestroyObjectImmediate(root);
            Debug.LogError("[BLACKSPIRE BUILD] FBX instance creation failed. Backup: " + backupPath);
            return;
        }

        Undo.RegisterCreatedObjectUndo(model, "Add Blackspire terrain model");
        model.name = "[MODEL] BLACKSPIRE TERRAIN";
        model.transform.SetParent(root.transform, false);
        model.transform.localPosition = Vector3.zero;
        model.transform.localRotation = modelAsset.transform.localRotation;
        model.transform.localScale = modelAsset.transform.localScale;

        int rendererCount = 0;
        foreach (Renderer renderer in model.GetComponentsInChildren<Renderer>(true))
        {
            Material[] materials = renderer.sharedMaterials;
            for (int i = 0; i < materials.Length; i++)
                materials[i] = hologram;
            renderer.sharedMaterials = materials;
            rendererCount++;
        }

        if (rendererCount == 0)
        {
            Undo.DestroyObjectImmediate(root);
            Debug.LogError("[BLACKSPIRE BUILD] Model has no renderers. Backup: " + backupPath);
            return;
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();

        Renderer mainRenderer = model.GetComponentInChildren<Renderer>(true);
        Debug.Log("[BLACKSPIRE BUILD SUCCESS] Scene=" + ScenePath +
                  " | Backup=" + backupPath +
                  " | Material=" + MaterialPath +
                  " | WorldPosition=" + root.transform.position +
                  " | ProjectionScale=" + root.transform.localScale +
                  " | TerrainBounds=" + (mainRenderer != null ? mainRenderer.bounds.ToString() : "(unknown)") +
                  " | Renderers=" + rendererCount +
                  " | Original FBX preserved=true");
    }

    private static GameObject FindInScene(Scene scene, string objectName)
    {
        foreach (GameObject go in scene.GetRootGameObjects())
        {
            if (go.name == objectName)
                return go;

            foreach (Transform t in go.GetComponentsInChildren<Transform>(true))
                if (t.name == objectName)
                    return t.gameObject;
        }

        return null;
    }
}


