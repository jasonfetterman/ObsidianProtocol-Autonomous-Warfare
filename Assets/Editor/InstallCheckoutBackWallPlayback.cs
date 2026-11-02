using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public static class InstallCheckoutBackWallPlayback
{
    private const string ScenePath =
        "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity";
    private const string VideoPath =
        "Assets/Video/ObsidianProtocol_StoreScreen_Sample.mp4";
    private const string TexturePath =
        "Assets/Materials/StoreVideoScreens/CHECKOUT_BACK_WALL_FRESH_20261013_092701.renderTexture";

    [MenuItem("Obsidian Protocol/Store/INSTALL CHECKOUT VIDEO PLAYBACK FIX")]
    public static void Install()
    {
        Scene scene = EditorSceneManager.OpenScene(
            ScenePath, OpenSceneMode.Single);

        GameObject root = null;
        foreach (GameObject sceneRoot in scene.GetRootGameObjects())
        {
            Transform found = Find(sceneRoot.transform,
                "Checkout back wall feature");
            if (found != null)
            {
                root = found.gameObject;
                break;
            }
        }

        if (root == null)
            throw new System.Exception(
                "Checkout root not found in " + scene.path);

        Transform surface = Find(root.transform, "Video Surface");
        if (surface == null)
            throw new System.Exception("Video Surface is missing.");

        Renderer screenRenderer = surface.GetComponent<Renderer>();
        VideoPlayer player = root.GetComponent<VideoPlayer>();
        RenderTexture rt =
            AssetDatabase.LoadAssetAtPath<RenderTexture>(TexturePath);

        if (screenRenderer == null || player == null || rt == null)
            throw new System.Exception(
                "Required renderer, VideoPlayer, or Render Texture is missing.");

        if (AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(VideoPath) == null)
            throw new System.Exception("Video asset not found: " + VideoPath);

        CheckoutBackWallVideoPlayback playback =
            root.GetComponent<CheckoutBackWallVideoPlayback>();

        if (playback == null)
            playback = Undo.AddComponent<CheckoutBackWallVideoPlayback>(root);

        playback.outputTexture = rt;
        playback.screenRenderer = screenRenderer;
        playback.videoAssetPath = VideoPath;

        player.targetTexture = rt;
        EditorUtility.SetDirty(playback);
        EditorUtility.SetDirty(player);
        EditorUtility.SetDirty(screenRenderer);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();

        Debug.Log(
            "[CHECKOUT VIDEO FIX INSTALLED] Runtime prepare/play component " +
            "attached to " + root.name + ". Enter Play mode to test.");
    }

    private static Transform Find(Transform parent, string exactName)
    {
        if (parent.name == exactName)
            return parent;

        foreach (Transform child in parent)
        {
            Transform result = Find(child, exactName);
            if (result != null)
                return result;
        }

        return null;
    }
}
