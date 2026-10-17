using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Video;

public static class RebuildCheckoutBackWallScreen
{
    private const string VideoPath =
        "Assets/Video/ObsidianProtocol_StoreScreen_Sample.mp4";

    private const string TexturePath =
        "Assets/Materials/StoreVideoScreens/CHECKOUT_BACK_WALL_FEATURE.renderTexture";

    private const string MaterialPath =
        "Assets/Materials/StoreVideoScreens/CHECKOUT_BACK_WALL_DIRECT_VIDEO.mat";

    private static string GetHierarchyPath(Transform item)
    {
        string path = item.name;
        while (item.parent != null)
        {
            item = item.parent;
            path = item.name + "/" + path;
        }
        return path;
    }
    [MenuItem("Obsidian Protocol/Store/REBUILD CHECKOUT BACK WALL SCREEN")]
    public static void Rebuild()
    {
        if (!File.Exists(Path.GetFullPath(VideoPath)))
            throw new FileNotFoundException("Checkout MP4 not found.", VideoPath);

        const string targetScenePath =
            "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity";

        UnityEngine.SceneManagement.Scene targetScene =
            EditorSceneManager.GetSceneByPath(targetScenePath);

        if (!targetScene.IsValid() || !targetScene.isLoaded)
        {
            targetScene = EditorSceneManager.OpenScene(
                targetScenePath,
                UnityEditor.SceneManagement.OpenSceneMode.Additive);
        }

        if (!targetScene.IsValid() || !targetScene.isLoaded)
            throw new Exception("Could not load target scene: " + targetScenePath);

        GameObject root = null;

        foreach (GameObject candidate in
            Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (candidate == null ||
                candidate.name != "Checkout back wall feature")
                continue;

            if (candidate.scene.IsValid() &&
                candidate.scene.path == targetScenePath)
            {
                root = candidate;
                break;
            }
        }

        if (root == null)
            throw new Exception(
                "Could not resolve Checkout back wall feature in loaded scene: " +
                targetScenePath);

        Transform surface = null;
        Transform[] descendants = root.GetComponentsInChildren<Transform>(true);

        foreach (Transform candidate in descendants)
        {
            if (candidate.name == "Video Surface")
            {
                surface = candidate;
                break;
            }
        }

        if (surface == null)
        {
            string hierarchy = "";
            foreach (Transform candidate in descendants)
                hierarchy += "\n - " + candidate.name +
                    " [path: " + GetHierarchyPath(candidate) + "]";

            throw new Exception(
                "Could not find Video Surface under Checkout back wall feature." +
                "\nObjects found:" + hierarchy);
        }

        Renderer screenRenderer = surface.GetComponent<Renderer>();
        if (screenRenderer == null)
            throw new Exception("Video Surface has no Renderer.");

        // Refresh imported assets before resolving the checkout texture.
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

        string rtPath = TexturePath;
        RenderTexture rt =
            AssetDatabase.LoadAssetAtPath<RenderTexture>(TexturePath);

        if (rt == null)
        {
            string resolvedPath = AssetDatabase.GUIDToAssetPath(
                "e509d39765ad402c98bf89bb65f92afa");

            if (!string.IsNullOrEmpty(resolvedPath))
            {
                rtPath = resolvedPath;
                rt = AssetDatabase.LoadAssetAtPath<RenderTexture>(resolvedPath);
            }
        }

        if (rt == null)
        {
            UnityEngine.Object importedAsset =
                AssetDatabase.LoadMainAssetAtPath(TexturePath);

            string importedType = importedAsset == null
                ? "<null>"
                : importedAsset.GetType().FullName;

            throw new Exception(
                "Checkout RenderTexture failed to load." +
                "\nExact path: " + TexturePath +
                "\nGUID-resolved path: " +
                AssetDatabase.GUIDToAssetPath(
                    "e509d39765ad402c98bf89bb65f92afa") +
                "\nMain asset type: " + importedType +
                "\nCheck Unity Console for import errors.");
        }
        Shader shader = Shader.Find("Universal Render Pipeline/Unlit");
        if (shader == null)
            shader = Shader.Find("Unlit/Texture");
        if (shader == null)
            throw new Exception("Could not find a supported unlit shader.");

        Material mat = AssetDatabase.LoadAssetAtPath<Material>(MaterialPath);
        if (mat == null)
        {
            mat = new Material(shader);
            AssetDatabase.CreateAsset(mat, MaterialPath);
        }

        mat.shader = shader;
        if (mat.HasProperty("_BaseMap"))
            mat.SetTexture("_BaseMap", rt);
        if (mat.HasProperty("_MainTex"))
            mat.SetTexture("_MainTex", rt);
        mat.mainTexture = rt;
        EditorUtility.SetDirty(mat);

        screenRenderer.sharedMaterial = mat;

        VideoPlayer[] childPlayers = surface.GetComponentsInChildren<VideoPlayer>(true);
        foreach (VideoPlayer childPlayer in childPlayers)
        {
            if (childPlayer.gameObject != root)
            {
                childPlayer.Stop();
                childPlayer.enabled = false;
                childPlayer.targetTexture = null;
                EditorUtility.SetDirty(childPlayer);
            }
        }

        VideoPlayer player = root.GetComponent<VideoPlayer>();
        if (player == null)
            player = root.AddComponent<VideoPlayer>();

        player.Stop();
        player.source = VideoSource.Url;
        player.url = new Uri(Path.GetFullPath(VideoPath)).AbsoluteUri;
        player.clip = null;
        player.renderMode = VideoRenderMode.RenderTexture;
        player.targetTexture = rt;
        player.targetMaterialRenderer = null;
        player.playOnAwake = true;
        player.isLooping = true;
        player.waitForFirstFrame = true;
        player.audioOutputMode = VideoAudioOutputMode.None;
        player.enabled = true;

        EditorUtility.SetDirty(player);
        EditorUtility.SetDirty(screenRenderer);
        EditorUtility.SetDirty(root);
        EditorUtility.SetDirty(surface.gameObject);

        AssetDatabase.SaveAssets();
        EditorSceneManager.MarkSceneDirty(root.scene);
        EditorSceneManager.SaveScene(root.scene);

        Debug.Log(
            "[CHECKOUT VIDEO REBUILT] URL=" + player.url +
            " | RenderTexture=" + rt.name +
            " | Renderer=" + screenRenderer.name);
    }
}






