using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public static class StoreRootVideoPlayerRepair
{
    private static readonly string[] Names =
    {
        "LEFT_SCREEN_1_DISPLAY",
        "LEFT_SCREEN_2_DISPLAY",
        "LEFT_SCREEN_3_DISPLAY",
        "RIGHT_SCREEN_4_DISPLAY",
        "RIGHT_SCREEN_5_DISPLAY",
        "RIGHT_SCREEN_6_DISPLAY"
    };

    [MenuItem("Obsidian Protocol/Store/REPAIR ALL SIX ROOT VIDEO PLAYERS")]
    private static void Repair()
    {
        const string scenePath = "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity";
        const string clipPath = "Assets/Video/ObsidianProtocol_StoreScreen_Sample.mp4";
        const string rtFolder = "Assets/Materials/StoreVideoScreens";

        Scene scene = SceneManager.GetActiveScene();

        if (!scene.IsValid() ||
            scene.path.Replace("\\", "/") != scenePath)
        {
            Debug.LogError("[STORE REPAIR] Open SCN16_STORE.unity first. Nothing changed.");
            return;
        }

        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            Debug.LogError("[STORE REPAIR] Exit Play Mode first. Nothing changed.");
            return;
        }

        VideoClip clip = AssetDatabase.LoadAssetAtPath<VideoClip>(clipPath);
        if (clip == null)
        {
            Debug.LogError("[STORE REPAIR] Sample VideoClip missing. Nothing changed: " + clipPath);
            return;
        }

        GameObject[] allObjects =
            Resources.FindObjectsOfTypeAll<GameObject>();

        GameObject[] roots = new GameObject[Names.Length];
        Transform[] quads = new Transform[Names.Length];
        VideoPlayer[] rootPlayers = new VideoPlayer[Names.Length];
        VideoPlayer[] quadPlayers = new VideoPlayer[Names.Length];
        RenderTexture[] textures = new RenderTexture[Names.Length];
        MeshRenderer[] renderers = new MeshRenderer[Names.Length];

        // Validate every target before modifying anything.
        for (int i = 0; i < Names.Length; i++)
        {
            foreach (GameObject candidate in allObjects)
            {
                if (candidate == null ||
                    candidate.scene != scene ||
                    candidate.name != Names[i])
                    continue;

                if (roots[i] != null)
                {
                    Debug.LogError("[STORE REPAIR] Duplicate display: " + Names[i] + ". Nothing changed.");
                    return;
                }

                roots[i] = candidate;
            }

            if (roots[i] == null)
            {
                Debug.LogError("[STORE REPAIR] Missing display: " + Names[i] + ". Nothing changed.");
                return;
            }

            Transform surface = roots[i].transform.Find("VIDEO DISPLAY SURFACE");
            quads[i] = surface != null ? surface.Find("VIDEO QUAD") : null;

            if (quads[i] == null)
            {
                Debug.LogError("[STORE REPAIR] Missing VIDEO QUAD under " + Names[i] + ". Nothing changed.");
                return;
            }

            renderers[i] = quads[i].GetComponent<MeshRenderer>();
            if (renderers[i] == null || renderers[i].sharedMaterial == null)
            {
                Debug.LogError("[STORE REPAIR] Missing quad renderer/material on " + Names[i] + ". Nothing changed.");
                return;
            }

            string rtPath = rtFolder + "/" + Names[i] + ".renderTexture";
            textures[i] = AssetDatabase.LoadAssetAtPath<RenderTexture>(rtPath);

            if (textures[i] == null)
            {
                Debug.LogError("[STORE REPAIR] Missing RenderTexture: " + rtPath + ". Nothing changed.");
                return;
            }

            Material mat = renderers[i].sharedMaterial;
            if (!mat.HasProperty("_BaseMap") && !mat.HasProperty("_MainTex"))
            {
                Debug.LogError("[STORE REPAIR] Unsupported material texture property on " + Names[i] + ". Nothing changed.");
                return;
            }
        }

        if (!EditorSceneManager.SaveScene(scene))
        {
            Debug.LogError("[STORE REPAIR] Could not save scene before backup. Nothing changed.");
            return;
        }

        string backup = scene.path + ".bak_root_video_" +
                        DateTime.Now.ToString("yyyyMMdd_HHmmss");
        File.Copy(scene.path, backup, false);

        for (int i = 0; i < Names.Length; i++)
        {
            // Use the display root as the sole active Video Player.
            rootPlayers[i] = roots[i].GetComponent<VideoPlayer>();
            if (rootPlayers[i] == null)
                rootPlayers[i] = Undo.AddComponent<VideoPlayer>(roots[i]);

            VideoPlayer rootPlayer = rootPlayers[i];
            rootPlayer.Stop();
            rootPlayer.source = VideoSource.VideoClip;
            rootPlayer.clip = clip;
            rootPlayer.targetTexture = textures[i];
            rootPlayer.renderMode = VideoRenderMode.RenderTexture;
            rootPlayer.playOnAwake = true;
            rootPlayer.isLooping = true;
            rootPlayer.waitForFirstFrame = true;
            rootPlayer.skipOnDrop = true;
            rootPlayer.audioOutputMode = VideoAudioOutputMode.None;
            rootPlayer.enabled = true;

            // Bind this screen's material to its own RenderTexture.
            Material mat = renderers[i].sharedMaterial;
            if (mat.HasProperty("_BaseMap"))
                mat.SetTexture("_BaseMap", textures[i]);
            if (mat.HasProperty("_MainTex"))
                mat.SetTexture("_MainTex", textures[i]);

            EditorUtility.SetDirty(mat);
            EditorUtility.SetDirty(rootPlayer);

            // Prevent two players from competing for the same screen texture.
            quadPlayers[i] = quads[i].GetComponent<VideoPlayer>();
            if (quadPlayers[i] != null)
            {
                quadPlayers[i].Stop();
                quadPlayers[i].enabled = false;
                EditorUtility.SetDirty(quadPlayers[i]);
            }

            Debug.Log("[STORE REPAIR] Configured " + Names[i] +
                      " | clip=" + clip.name +
                      " | texture=" + textures[i].name);
        }

        AssetDatabase.SaveAssets();
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[STORE REPAIR] All six root Video Players configured. Scene backup: " + backup);
    }
}
