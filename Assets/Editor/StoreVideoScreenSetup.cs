using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System;
using System.IO;

public static class StoreVideoScreenSetup
{
    private static readonly string[] DisplayNames =
    {
        "LEFT_SCREEN_1_DISPLAY",
        "LEFT_SCREEN_2_DISPLAY",
        "LEFT_SCREEN_3_DISPLAY",
        "RIGHT_SCREEN_4_DISPLAY",
        "RIGHT_SCREEN_5_DISPLAY",
        "RIGHT_SCREEN_6_DISPLAY"
    };

    [MenuItem("Obsidian Protocol/Store/SET UP SIX DISPLAY VIDEOS")]
    private static void Prepare()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (!scene.IsValid() ||
            scene.path.Replace("\\", "/") != "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity")
        {
            Debug.LogError("Open Assets/Scenes/SCN16_STORE/SCN16_STORE.unity first. Nothing changed.");
            return;
        }

        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            Debug.LogError("Exit Play Mode first. Nothing changed.");
            return;
        }

        string[] clipGuids = AssetDatabase.FindAssets("t:VideoClip", new[] { "Assets/Video" });
        if (clipGuids.Length == 0)
        {
            Debug.LogError("No imported VideoClip found under Assets/Video. Nothing changed.");
            return;
        }

        VideoClip clip = null;
        foreach (string guid in clipGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string fileName = Path.GetFileNameWithoutExtension(path);
            if (fileName.IndexOf("sample", StringComparison.OrdinalIgnoreCase) >= 0 ||
                fileName.IndexOf("demo", StringComparison.OrdinalIgnoreCase) >= 0 ||
                fileName.IndexOf("test", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                clip = AssetDatabase.LoadAssetAtPath<VideoClip>(path);
                if (clip != null) break;
            }
        }

        if (clip == null && clipGuids.Length == 1)
            clip = AssetDatabase.LoadAssetAtPath<VideoClip>(AssetDatabase.GUIDToAssetPath(clipGuids[0]));

        if (clip == null)
        {
            string available = string.Join(", ", Array.ConvertAll(clipGuids, guid => AssetDatabase.GUIDToAssetPath(guid)));
            Debug.LogError("Could not identify a single sample video. Name the intended sample with 'sample', 'demo', or 'test', or leave only one VideoClip in Assets/Video. Found: " + available + ". Nothing changed.");
            return;
        }

        Transform[] displays = new Transform[DisplayNames.Length];
        for (int i = 0; i < DisplayNames.Length; i++)
        {
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            GameObject found = null;
            foreach (GameObject candidate in allObjects)
            {
                if (candidate.name != DisplayNames[i] || !candidate.scene.IsValid() || candidate.scene != scene)
                    continue;
                if (found != null)
                {
                    Debug.LogError("Multiple scene objects named " + DisplayNames[i] + ". Nothing changed.");
                    return;
                }
                found = candidate;
            }

            if (found == null)
            {
                Debug.LogError("Missing display " + DisplayNames[i] + ". Nothing changed.");
                return;
            }
            displays[i] = found.transform;
            if (displays[i].Find("VIDEO DISPLAY SURFACE") != null)
            {
                Debug.LogError("Video surface already exists on " + DisplayNames[i] + ". Nothing changed.");
                return;
            }
        }

        Shader shader = Shader.Find("Universal Render Pipeline/Unlit");
        if (shader == null) shader = Shader.Find("Unlit/Texture");
        if (shader == null)
        {
            Debug.LogError("No compatible unlit shader found. Nothing changed.");
            return;
        }

        if (!EditorSceneManager.SaveScene(scene))
        {
            Debug.LogError("Could not save scene before backup. Nothing changed.");
            return;
        }

        string backup = scene.path + ".bak_six_display_video_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
        File.Copy(scene.path, backup, false);

        const string materialFolder = "Assets/Materials/StoreVideoScreens";
        if (!AssetDatabase.IsValidFolder("Assets/Materials"))
            AssetDatabase.CreateFolder("Assets", "Materials");
        if (!AssetDatabase.IsValidFolder(materialFolder))
            AssetDatabase.CreateFolder("Assets/Materials", "StoreVideoScreens");

        for (int i = 0; i < displays.Length; i++)
        {
            Transform display = displays[i];
            bool left = i < 3;

            GameObject surface = new GameObject("VIDEO DISPLAY SURFACE");
            Undo.RegisterCreatedObjectUndo(surface, "Set up store display video");
            surface.transform.SetParent(display, false);
            surface.transform.localPosition = new Vector3(left ? 0.501f : -0.501f, 0f, 0f);
            surface.transform.localRotation = Quaternion.Euler(0f, left ? 90f : 270f, 0f);
            surface.transform.localScale = Vector3.one;

            GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quad.name = "VIDEO QUAD";
            Undo.RegisterCreatedObjectUndo(quad, "Create store video quad");
            quad.transform.SetParent(surface.transform, false);
            quad.transform.localPosition = Vector3.zero;
            quad.transform.localRotation = Quaternion.identity;
            quad.transform.localScale = Vector3.one;

            Collider collider = quad.GetComponent<Collider>();
            if (collider != null) Undo.DestroyObjectImmediate(collider);

            Material material = new Material(shader);
            material.name = "Store Video - " + display.name;
            if (material.HasProperty("_BaseColor")) material.SetColor("_BaseColor", Color.white);
            if (material.HasProperty("_Color")) material.SetColor("_Color", Color.white);
            string materialPath = AssetDatabase.GenerateUniqueAssetPath(materialFolder + "/" + display.name + ".mat");
            AssetDatabase.CreateAsset(material, materialPath);
            quad.GetComponent<MeshRenderer>().sharedMaterial = material;

            VideoPlayer player = quad.AddComponent<VideoPlayer>();
            player.source = VideoSource.VideoClip;
            player.clip = clip;
            player.playOnAwake = true;
            player.isLooping = true;
            player.waitForFirstFrame = true;
            player.skipOnDrop = true;
            player.audioOutputMode = VideoAudioOutputMode.None;
            player.renderMode = VideoRenderMode.MaterialOverride;
            player.targetMaterialRenderer = quad.GetComponent<MeshRenderer>();
            player.targetMaterialProperty = material.HasProperty("_BaseMap") ? "_BaseMap" : "_MainTex";

            EditorUtility.SetDirty(display.gameObject);
        }

        AssetDatabase.SaveAssets();
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("Configured all six Store display surfaces with looping video: " + AssetDatabase.GetAssetPath(clip) + ". Scene backup: " + backup);
    }
}
