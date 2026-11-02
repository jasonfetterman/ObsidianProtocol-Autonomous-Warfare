using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class StrategicDeploymentSignBuilder
{
    private const string ScenePath = "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";
    private const string ScriptPath = "Assets/StrategicDeploymentSignAnimation.cs";
    private const string MaterialFolder = "Assets/Art/Materials/Holograms";
    private const string MaterialPath = MaterialFolder + "/MAT_HOLO_STRATEGIC_DEPLOYMENT_SIGN.mat";
    private const string SignName = "[SIGNAGE] STRATEGIC DEPLOYMENT MAP";

    [MenuItem("Obsidian Protocol/Deployment/Build Glowing Spinning Deployment Sign")]
    public static void Build()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            Debug.LogError("[DEPLOYMENT SIGN] Stop Play mode before building.");
            return;
        }

        if (!File.Exists(ScenePath) || !File.Exists(ScriptPath))
        {
            Debug.LogError("[DEPLOYMENT SIGN] Required scene or animation script is missing.");
            return;
        }

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;

        string backupFolder = "Backups/Deployment";
        Directory.CreateDirectory(backupFolder);
        string backupPath = backupFolder + "/SCN12_DEPLOYMENT_SignBackup_" +
                            DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".unity";
        File.Copy(ScenePath, backupPath, true);

        Scene scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
        GameObject sign = FindInScene(scene, SignName);

        if (sign == null)
        {
            Debug.LogError("[DEPLOYMENT SIGN] Sign object not found. Backup: " + backupPath);
            return;
        }

        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null)
        {
            Debug.LogError("[DEPLOYMENT SIGN] URP/Lit shader unavailable. Backup: " + backupPath);
            return;
        }

        if (!AssetDatabase.IsValidFolder(MaterialFolder))
            Directory.CreateDirectory(MaterialFolder);

        Material material = AssetDatabase.LoadAssetAtPath<Material>(MaterialPath);
        if (material == null)
        {
            material = new Material(shader);
            material.name = "MAT_HOLO_STRATEGIC_DEPLOYMENT_SIGN";
            AssetDatabase.CreateAsset(material, MaterialPath);
        }

        material.shader = shader;
        material.SetColor("_BaseColor", new Color(0.004f, 0.025f, 0.045f, 1f));
        material.SetColor("_Color", new Color(0.01f, 0.08f, 0.12f, 1f));
        material.SetColor("_EmissionColor", new Color(0.02f, 0.9f, 1f, 1f));
        material.EnableKeyword("_EMISSION");
        material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
        EditorUtility.SetDirty(material);
        AssetDatabase.SaveAssets();

        MeshRenderer renderer = sign.GetComponent<MeshRenderer>();
        TextMesh text = sign.GetComponent<TextMesh>();
        if (renderer == null || text == null)
        {
            Debug.LogError("[DEPLOYMENT SIGN] Expected TextMesh and MeshRenderer components. Backup: " + backupPath);
            return;
        }

        Undo.RecordObject(renderer, "Assign Deployment Sign Hologram Material");
        StrategicDeploymentSignAnimation animation =
            sign.GetComponent<StrategicDeploymentSignAnimation>();
        if (animation == null)
            animation = Undo.AddComponent<StrategicDeploymentSignAnimation>(sign);

        animation.enabled = true;
        EditorUtility.SetDirty(sign);
        EditorUtility.SetDirty(renderer);
        EditorUtility.SetDirty(animation);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();

        Debug.Log("[DEPLOYMENT SIGN BUILD SUCCESS] Scene=" + ScenePath +
                  " | Sign=" + sign.name +
                  " | Material=" + MaterialPath +
                  " | Animation=attached | Backup=" + backupPath);
    }

    private static GameObject FindInScene(Scene scene, string objectName)
    {
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            if (root.name == objectName)
                return root;

            foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
                if (child.name == objectName)
                    return child.gameObject;
        }

        return null;
    }
}


