using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public static class OPAWTerminalGlassTool
{
    private const string MaterialFolder = "Assets/Materials";
    private const string MaterialPath = "Assets/Materials/OPAW_Terminal_CyanGlass.mat";

    [MenuItem("Obsidian Protocol/Garage/Apply Terminal Cyan Glass")]
    private static void Apply()
    {
        Scene scene = EditorSceneManager.GetActiveScene();

        if (scene.name != "SCN04_GARAGE" || string.IsNullOrEmpty(scene.path))
        {
            Debug.LogError("[TerminalGlass] Open and save SCN04_GARAGE first. No changes made.");
            return;
        }

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            Debug.LogWarning("[TerminalGlass] Cancelled by user. No changes made.");
            return;
        }

        string backupPath = scene.path + ".terminal-glass-backup";
        if (File.Exists(backupPath))
        {
            backupPath = scene.path + ".terminal-glass-backup-" +
                         DateTime.Now.ToString("yyyyMMdd-HHmmss");
        }

        File.Copy(scene.path, backupPath);
        Debug.Log("[TerminalGlass] Scene backup: " + backupPath);

        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null)
        {
            Debug.LogError("[TerminalGlass] URP Lit shader not found. Scene was backed up; no renderers changed.");
            return;
        }

        if (!AssetDatabase.IsValidFolder(MaterialFolder))
            AssetDatabase.CreateFolder("Assets", "Materials");

        Material glass = AssetDatabase.LoadAssetAtPath<Material>(MaterialPath);
        if (glass == null)
        {
            glass = new Material(shader);
            glass.name = "OPAW_Terminal_CyanGlass";
            AssetDatabase.CreateAsset(glass, MaterialPath);
        }
        else if (glass.shader != shader)
        {
            glass.shader = shader;
        }

        Color baseColor = new Color(0.025f, 0.22f, 0.30f, 0.22f);
        Color emission = new Color(0.0f, 0.65f, 0.9f, 1.0f);

        SetFloat(glass, "_Surface", 1f);
        SetFloat(glass, "_Blend", 0f);
        SetFloat(glass, "_AlphaClip", 0f);
        SetFloat(glass, "_SrcBlend", 5f);
        SetFloat(glass, "_DstBlend", 10f);
        SetFloat(glass, "_ZWrite", 0f);
        SetFloat(glass, "_Cull", 2f);
        SetColor(glass, "_BaseColor", baseColor);
        SetColor(glass, "_Color", baseColor);
        SetColor(glass, "_EmissionColor", emission);

        glass.SetOverrideTag("RenderType", "Transparent");
        glass.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        glass.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        glass.DisableKeyword("_ALPHATEST_ON");
        glass.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        glass.EnableKeyword("_EMISSION");

        EditorUtility.SetDirty(glass);
        AssetDatabase.SaveAssets();

        int changed = 0;
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            Transform[] transforms = root.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in transforms)
            {
                if (t.name != "DISPLAY") continue;
                if (t.parent == null || !t.parent.name.StartsWith("TERMINAL_"))
                {
                    Debug.LogWarning("[TerminalGlass] Skipped DISPLAY without a terminal parent: " + t.name, t);
                    continue;
                }

                Renderer renderer = t.GetComponent<Renderer>();
                if (renderer == null)
                {
                    Debug.LogWarning("[TerminalGlass] DISPLAY has no Renderer.", t);
                    continue;
                }

                Undo.RecordObject(renderer, "Apply Terminal Cyan Glass");
                Material[] materials = renderer.sharedMaterials;
                if (materials.Length == 0)
                    materials = new Material[] { glass };
                else
                    for (int i = 0; i < materials.Length; i++)
                        materials[i] = glass;

                renderer.sharedMaterials = materials;
                EditorUtility.SetDirty(renderer);
                changed++;
            }
        }

        EditorSceneManager.MarkSceneDirty(scene);
        if (EditorSceneManager.SaveScene(scene))
        {
            Debug.Log("[TerminalGlass] Applied cyan glass to " + changed +
                      " terminal displays. Material: " + MaterialPath);
        }
        else
        {
            Debug.LogError("[TerminalGlass] Scene save failed. Backup remains at: " + backupPath);
        }
    }

    private static void SetFloat(Material material, string property, float value)
    {
        if (material.HasProperty(property))
            material.SetFloat(property, value);
    }

    private static void SetColor(Material material, string property, Color value)
    {
        if (material.HasProperty(property))
            material.SetColor(property, value);
    }
}
