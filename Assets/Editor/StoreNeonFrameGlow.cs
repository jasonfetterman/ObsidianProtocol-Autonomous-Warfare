using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public static class StoreNeonFrameGlow
{
    [MenuItem("Obsidian Protocol/Store/MATCH SIX SCREEN FRAMES TO NEON GLOW")]
    private static void MatchFrames()
    {
        var scene = SceneManager.GetActiveScene();

        if (scene.path.Replace("\\", "/") !=
            "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity" ||
            EditorApplication.isPlaying)
        {
            Debug.LogError("Open SCN16_STORE outside Play Mode. Nothing changed.");
            return;
        }

        string[] labels = {
            "AIR UNITS wall header text",
            "GROUND UNITS wall header text",
            "SEA UNITS wall header text",
            "COMMANDERS wall header text",
            "EXPERIMENTAL UNITS wall header text",
            "SATELLITES wall header text"
        };

        string[] prefixes = {
            "LEFT_SCREEN_1_NEON_", "LEFT_SCREEN_2_NEON_",
            "LEFT_SCREEN_3_NEON_", "RIGHT_SCREEN_4_NEON_",
            "RIGHT_SCREEN_5_NEON_", "RIGHT_SCREEN_6_NEON_"
        };

        Transform FindObject(string name)
        {
            foreach (var go in scene.GetRootGameObjects())
                foreach (var t in go.GetComponentsInChildren<Transform>(true))
                    if (t.name == name) return t;
            return null;
        }

        Color[] colors = new Color[6];
        Transform[][] frameParts = new Transform[6][];

        for (int i = 0; i < 6; i++)
        {
            var label = FindObject(labels[i]);
            if (label == null)
            {
                Debug.LogError("Missing neon label: " + labels[i] + ". Nothing changed.");
                return;
            }

            var tmp = label.GetComponent<TMP_Text>();
            var labelRenderer = label.GetComponent<Renderer>();
            Material source = tmp != null ? tmp.fontMaterial :
                labelRenderer != null ? labelRenderer.sharedMaterial : null;

            if (source == null)
            {
                Debug.LogError("Cannot read neon material: " + labels[i] + ". Nothing changed.");
                return;
            }

            if (source.HasProperty("_GlowColor"))
                colors[i] = source.GetColor("_GlowColor");
            else if (source.HasProperty("_EmissionColor"))
                colors[i] = source.GetColor("_EmissionColor");
            else if (source.HasProperty("_FaceColor"))
                colors[i] = source.GetColor("_FaceColor");
            else if (source.HasProperty("_Color"))
                colors[i] = source.GetColor("_Color");
            else
            {
                Debug.LogError("Cannot read neon color: " + labels[i] + ". Nothing changed.");
                return;
            }

            var found = new System.Collections.Generic.List<Transform>();
            foreach (var go in scene.GetRootGameObjects())
                foreach (var t in go.GetComponentsInChildren<Transform>(true))
                    if (t.name.StartsWith(prefixes[i], StringComparison.Ordinal))
                        found.Add(t);

            if (found.Count == 0)
            {
                Debug.LogError("No frame pieces found with prefix " +
                    prefixes[i] + ". Nothing changed.");
                return;
            }

            frameParts[i] = found.ToArray();
        }

        string folder = "Assets/Materials/StoreNeonFrames";
        if (!AssetDatabase.IsValidFolder("Assets/Materials"))
            AssetDatabase.CreateFolder("Assets", "Materials");
        if (!AssetDatabase.IsValidFolder(folder))
            AssetDatabase.CreateFolder("Assets/Materials", "StoreNeonFrames");

        EditorSceneManager.SaveScene(scene);
        string backup = scene.path + ".bak_frame_glow_" +
            DateTime.Now.ToString("yyyyMMdd_HHmmss");
        if (File.Exists(backup))
        {
            Debug.LogError("Backup already exists. Nothing changed.");
            return;
        }
        File.Copy(scene.path, backup);

        int updated = 0;

        for (int i = 0; i < 6; i++)
        {
            foreach (var part in frameParts[i])
            {
                foreach (var r in part.GetComponentsInChildren<Renderer>(true))
                {
                    var original = r.sharedMaterial;
                    if (original == null) continue;

                    string path = folder + "/" + part.name + "_" +
                        r.gameObject.name + "_Glow.mat";
                    path = AssetDatabase.GenerateUniqueAssetPath(path);

                    var mat = new Material(original);
                    Color c = colors[i];

                    if (mat.HasProperty("_BaseColor"))
                        mat.SetColor("_BaseColor", c);
                    if (mat.HasProperty("_Color"))
                        mat.SetColor("_Color", c);
                    if (mat.HasProperty("_EmissionColor"))
                    {
                        mat.EnableKeyword("_EMISSION");
                        mat.SetColor("_EmissionColor", c * 2.5f);
                    }

                    AssetDatabase.CreateAsset(mat, path);
                    r.sharedMaterial = mat;
                    EditorUtility.SetDirty(r);
                    updated++;
                }
            }
        }

        AssetDatabase.SaveAssets();
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("Updated " + updated +
            " frame renderers. Backup: " + backup +
            ". Visible bloom still depends on the scene's post-processing.");
    }
}
