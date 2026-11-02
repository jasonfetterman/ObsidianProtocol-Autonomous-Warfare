using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public static class RebuildCheckoutBackWallScreenFresh
{
    private const string ScenePath =
        "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity";

    private const string ObjectName = "Checkout back wall feature";
    private const string SurfaceName = "Video Surface";
    private const string VideoPath =
        "Assets/Video/ObsidianProtocol_StoreScreen_Sample.mp4";

    [MenuItem("Obsidian Protocol/Store/REBUILD CHECKOUT BACK WALL - FRESH")]
    public static void Rebuild()
    {
        if (!File.Exists(ScenePath))
            throw new Exception("Scene file missing: " + ScenePath);

        if (!File.Exists(VideoPath))
            throw new Exception("Video file missing: " + VideoPath);

        // Avoid accidentally discarding edits in another open scene.
        Scene current = SceneManager.GetActiveScene();
        if (current.IsValid() && current.isDirty &&
            current.path != ScenePath)
        {
            throw new Exception(
                "Save the currently edited scene first, then run the rebuild again.");
        }

        // Open the exact scene as the active scene, not additively.
        Scene scene = EditorSceneManager.OpenScene(
            ScenePath, OpenSceneMode.Single);

        if (!scene.IsValid() || !scene.isLoaded)
            throw new Exception("Failed to open scene: " + ScenePath);

        GameObject oldRoot = FindInScene(scene, ObjectName);

        if (oldRoot == null)
        {
            string roots = "";
            foreach (GameObject item in scene.GetRootGameObjects())
                roots += "\n - " + item.name;

            throw new Exception(
                "Checkout object not found after opening the exact scene." +
                "\nLoaded scene path: " + scene.path +
                "\nScene name: " + scene.name +
                "\nRoot objects:" + roots);
        }

        Transform oldSurface = FindChild(oldRoot.transform, SurfaceName);
        if (oldSurface == null)
            throw new Exception(
                "Found checkout object, but Video Surface is missing. " +
                "Nothing was deleted.");

        MeshFilter oldSurfaceFilter = oldSurface.GetComponent<MeshFilter>();
        MeshRenderer oldSurfaceRenderer = oldSurface.GetComponent<MeshRenderer>();

        if (oldSurfaceFilter == null || oldSurfaceFilter.sharedMesh == null ||
            oldSurfaceRenderer == null)
        {
            throw new Exception(
                "Checkout Video Surface is missing its mesh or renderer. " +
                "Nothing was deleted.");
        }

        // Capture the existing geometry, placement and appearance.
        Transform oldParent = oldRoot.transform.parent;
        Vector3 rootPosition = oldRoot.transform.localPosition;
        Quaternion rootRotation = oldRoot.transform.localRotation;
        Vector3 rootScale = oldRoot.transform.localScale;
        int rootLayer = oldRoot.layer;
        int rootSibling = oldRoot.transform.GetSiblingIndex();

        MeshFilter oldRootFilter = oldRoot.GetComponent<MeshFilter>();
        MeshRenderer oldRootRenderer = oldRoot.GetComponent<MeshRenderer>();
        Mesh rootMesh = oldRootFilter != null
            ? oldRootFilter.sharedMesh : null;
        Material[] rootMaterials = oldRootRenderer != null
            ? oldRootRenderer.sharedMaterials : null;

        Mesh screenMesh = oldSurfaceFilter.sharedMesh;
        Vector3 screenPosition = oldSurface.position;
        Quaternion screenRotation = oldSurface.rotation;
        Vector3 screenScale = oldSurface.lossyScale;
        int screenLayer = oldSurface.gameObject.layer;

        string folder = "Assets/Materials/StoreVideoScreens";
        if (!AssetDatabase.IsValidFolder(folder))
            throw new Exception("Required asset folder missing: " + folder);

        Shader shader = Shader.Find("Universal Render Pipeline/Unlit");
        if (shader == null)
            shader = Shader.Find("Unlit/Texture");
        if (shader == null)
            throw new Exception("Could not find a supported unlit shader.");

        string stamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string rtPath = folder + "/CHECKOUT_BACK_WALL_FRESH_" +
                        stamp + ".renderTexture";
        string matPath = folder + "/CHECKOUT_BACK_WALL_FRESH_" +
                         stamp + ".mat";

        // Create and save the fresh assets before touching the scene objects.
        RenderTexture rt = new RenderTexture(
            1920, 1080, 0, RenderTextureFormat.ARGB32);
        rt.name = "CHECKOUT_BACK_WALL_FRESH_" + stamp;
        rt.Create();
        AssetDatabase.CreateAsset(rt, rtPath);

        Material mat = new Material(shader);
        mat.name = "CHECKOUT_BACK_WALL_FRESH_" + stamp;
        if (mat.HasProperty("_BaseMap"))
            mat.SetTexture("_BaseMap", rt);
        if (mat.HasProperty("_MainTex"))
            mat.SetTexture("_MainTex", rt);
        mat.mainTexture = rt;
        AssetDatabase.CreateAsset(mat, matPath);
        AssetDatabase.SaveAssets();

        // Back up the scene file before the destructive replacement.
        string projectRoot = Directory.GetParent(Application.dataPath).FullName;
        string backupPath = Path.Combine(
            projectRoot, "SCN16_STORE.before_fresh_checkout.bak");

        if (!File.Exists(backupPath))
            File.Copy(ScenePath, backupPath);

        Undo.DestroyObjectImmediate(oldRoot);

        GameObject newRoot = new GameObject(ObjectName);
        newRoot.layer = rootLayer;
        newRoot.transform.SetParent(oldParent, false);
        newRoot.transform.localPosition = rootPosition;
        newRoot.transform.localRotation = rootRotation;
        newRoot.transform.localScale = rootScale;

        if (oldParent != null)
            newRoot.transform.SetSiblingIndex(
                Mathf.Clamp(rootSibling, 0, oldParent.childCount - 1));

        if (rootMesh != null)
        {
            newRoot.AddComponent<MeshFilter>().sharedMesh = rootMesh;
            MeshRenderer renderer = newRoot.AddComponent<MeshRenderer>();
            if (rootMaterials != null)
                renderer.sharedMaterials = rootMaterials;
        }

        GameObject surface = new GameObject(SurfaceName);
        surface.layer = screenLayer;
        surface.transform.SetParent(newRoot.transform, false);
        surface.transform.position = screenPosition;
        surface.transform.rotation = screenRotation;

        Vector3 parentScale = newRoot.transform.lossyScale;
        surface.transform.localScale = new Vector3(
            Mathf.Abs(parentScale.x) > 0.0001f
                ? screenScale.x / parentScale.x : screenScale.x,
            Mathf.Abs(parentScale.y) > 0.0001f
                ? screenScale.y / parentScale.y : screenScale.y,
            Mathf.Abs(parentScale.z) > 0.0001f
                ? screenScale.z / parentScale.z : screenScale.z);

        surface.AddComponent<MeshFilter>().sharedMesh = screenMesh;
        surface.AddComponent<MeshRenderer>().sharedMaterial = mat;

        VideoPlayer player = newRoot.AddComponent<VideoPlayer>();
        player.playOnAwake = true;
        player.isLooping = true;
        player.source = VideoSource.Url;
        player.url = new Uri(Path.GetFullPath(VideoPath)).AbsoluteUri;
        player.clip = null;
        player.renderMode = VideoRenderMode.RenderTexture;
        player.targetTexture = rt;
        player.targetMaterialRenderer = null;
        player.waitForFirstFrame = true;
        player.audioOutputMode = VideoAudioOutputMode.None;

        EditorUtility.SetDirty(newRoot);
        EditorUtility.SetDirty(surface);
        EditorUtility.SetDirty(player);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();

        player.Play();

        Debug.Log(
            "[CHECKOUT FRESH REBUILD SUCCESS] " +
            "Scene=" + scene.path +
            " | Video=" + player.url +
            " | RenderTexture=" + rtPath +
            " | Material=" + matPath +
            " | Backup=" + backupPath);
    }

    private static GameObject FindInScene(Scene scene, string exactName)
    {
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            Transform found = FindTransform(root.transform, exactName);
            if (found != null)
                return found.gameObject;
        }

        return null;
    }

    private static Transform FindChild(Transform parent, string exactName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == exactName)
                return child;

            Transform nested = FindChild(child, exactName);
            if (nested != null)
                return nested;
        }

        return null;
    }

    private static Transform FindTransform(Transform parent, string exactName)
    {
        if (parent.name == exactName)
            return parent;

        foreach (Transform child in parent)
        {
            Transform found = FindTransform(child, exactName);
            if (found != null)
                return found;
        }

        return null;
    }
}
