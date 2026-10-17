using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public static class StoreVideoPlaybackRepair
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

    [MenuItem("Obsidian Protocol/Store/REPAIR SIX STORE VIDEO PLAYBACK")]
    private static void Repair()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (!scene.IsValid() || scene.path.Replace("\\", "/") != "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity")
        {
            Debug.LogError("Open SCN16_STORE.unity first. Nothing changed.");
            return;
        }
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            Debug.LogError("Exit Play Mode first. Nothing changed.");
            return;
        }

        const string clipPath = "Assets/Video/ObsidianProtocol_StoreScreen_Sample.mp4";
        VideoClip clip = AssetDatabase.LoadAssetAtPath<VideoClip>(clipPath);
        if (clip == null)
        {
            Debug.LogError("Sample video is not imported as a VideoClip: " + clipPath);
            return;
        }

        Transform[] displays = new Transform[DisplayNames.Length];
        MeshRenderer[] renderers = new MeshRenderer[DisplayNames.Length];
        VideoPlayer[] players = new VideoPlayer[DisplayNames.Length];
        Material[] materials = new Material[DisplayNames.Length];
        string[] textureProperties = new string[DisplayNames.Length];
        GameObject[] sceneObjects = Array.ConvertAll(Resources.FindObjectsOfTypeAll<GameObject>(), x => x);

        for (int i = 0; i < DisplayNames.Length; i++)
        {
            GameObject found = null;
            foreach (GameObject candidate in sceneObjects)
            {
                if (candidate == null || candidate.scene != scene || candidate.name != DisplayNames[i]) continue;
                if (found != null)
                {
                    Debug.LogError("Duplicate display: " + DisplayNames[i] + ". Nothing changed.");
                    return;
                }
                found = candidate;
            }
            if (found == null)
            {
                Debug.LogError("Missing display: " + DisplayNames[i] + ". Nothing changed.");
                return;
            }
            displays[i] = found.transform;
            Transform surface = displays[i].Find("VIDEO DISPLAY SURFACE");
            Transform quad = surface != null ? surface.Find("VIDEO QUAD") : null;
            if (quad == null)
            {
                Debug.LogError("Missing VIDEO QUAD on " + DisplayNames[i] + ". Nothing changed.");
                return;
            }
            renderers[i] = quad.GetComponent<MeshRenderer>();
            players[i] = quad.GetComponent<VideoPlayer>();
            if (renderers[i] == null || players[i] == null || renderers[i].sharedMaterial == null)
            {
                Debug.LogError("Missing renderer, material, or VideoPlayer on " + DisplayNames[i] + ". Nothing changed.");
                return;
            }
            materials[i] = renderers[i].sharedMaterial;
            textureProperties[i] = materials[i].HasProperty("_BaseMap") ? "_BaseMap" :
                materials[i].HasProperty("_MainTex") ? "_MainTex" : null;
            if (textureProperties[i] == null)
            {
                Debug.LogError("Material has no supported video texture property on " + DisplayNames[i] + ". Nothing changed.");
                return;
            }
        }

        if (!EditorSceneManager.SaveScene(scene))
        {
            Debug.LogError("Could not save scene before backup. Nothing changed.");
            return;
        }
        string backup = scene.path + ".bak_video_repair_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
        File.Copy(scene.path, backup, false);

        const string folder = "Assets/Materials/StoreVideoScreens";
        if (!AssetDatabase.IsValidFolder("Assets/Materials")) AssetDatabase.CreateFolder("Assets", "Materials");
        if (!AssetDatabase.IsValidFolder(folder)) AssetDatabase.CreateFolder("Assets/Materials", "StoreVideoScreens");

        for (int i = 0; i < DisplayNames.Length; i++)
        {
            bool left = i < 3;
            Transform surface = displays[i].Find("VIDEO DISPLAY SURFACE");
            surface.localPosition = new Vector3(left ? -0.501f : 0.501f, 0f, 0f);
            surface.localRotation = Quaternion.Euler(0f, left ? 270f : 90f, 0f);
            Transform quad = surface.Find("VIDEO QUAD");
            quad.localPosition = Vector3.zero;
            quad.localRotation = Quaternion.identity;
            string rtPath = folder + "/" + DisplayNames[i] + ".renderTexture";
            RenderTexture rt = AssetDatabase.LoadAssetAtPath<RenderTexture>(rtPath);
            if (rt == null)
            {
                rt = new RenderTexture(1920, 1080, 0, RenderTextureFormat.ARGB32);
                rt.name = DisplayNames[i] + " Video RenderTexture";
                rt.useMipMap = false;
                rt.autoGenerateMips = false;
                AssetDatabase.CreateAsset(rt, rtPath);
            }
            if (!rt.IsCreated()) rt.Create();
            materials[i].SetTexture(textureProperties[i], rt);
            EditorUtility.SetDirty(materials[i]);
            players[i].Stop();
            players[i].source = VideoSource.VideoClip;
            players[i].clip = clip;
            players[i].targetTexture = rt;
            players[i].renderMode = VideoRenderMode.RenderTexture;
            players[i].playOnAwake = true;
            players[i].isLooping = true;
            players[i].waitForFirstFrame = true;
            players[i].skipOnDrop = true;
            players[i].audioOutputMode = VideoAudioOutputMode.None;
            players[i].isLooping = true;
            players[i].enabled = true;
            StoreVideoRuntimePlayback.Ensure(players[i]);
            EditorUtility.SetDirty(players[i]);
        }

        AssetDatabase.SaveAssets();
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("Repaired video rendering for all six Store displays using " + clipPath + ". Backup: " + backup);
    }
}

[InitializeOnLoad]
public static class StoreVideoRuntimePlayback
{
    private static readonly System.Collections.Generic.HashSet<VideoPlayer> Registered =
        new System.Collections.Generic.HashSet<VideoPlayer>();

    static StoreVideoRuntimePlayback()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    public static void Ensure(VideoPlayer player)
    {
        if (player == null || !Registered.Add(player))
            return;

        player.playOnAwake = false;
        player.prepareCompleted += OnPrepared;
        player.errorReceived += OnError;
        EditorUtility.SetDirty(player);
    }

    private static void OnPrepared(VideoPlayer player)
    {
        Debug.Log("[STORE VIDEO] Prepared " + player.name + " | active=" + player.isActiveAndEnabled + " | size=" + player.width + "x" + player.height);
        if (player.isActiveAndEnabled) player.Play();
    }

    private static void OnError(VideoPlayer player, string message)
    {
        Debug.LogError("[STORE VIDEO] " +
            (player != null ? player.name : "Unknown player") +
            ": " + message);
    }

    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (state != PlayModeStateChange.EnteredPlayMode)
            return;

        VideoPlayer[] players = Resources.FindObjectsOfTypeAll<VideoPlayer>();
        foreach (VideoPlayer player in players)
        {
            if (player == null || !player.gameObject.scene.IsValid())
                continue;

            if (player.gameObject.scene.path.Replace("\\", "/") !=
                "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity")
                continue;

            Ensure(player);
            Debug.Log("[STORE VIDEO] Preparing " + player.name + " | active=" + player.isActiveAndEnabled + " | clip=" + (player.clip != null ? player.clip.name : "NULL") + " | targetTexture=" + (player.targetTexture != null ? player.targetTexture.name : "NULL"));
            player.Stop();
            player.Prepare();
        }
    }
}