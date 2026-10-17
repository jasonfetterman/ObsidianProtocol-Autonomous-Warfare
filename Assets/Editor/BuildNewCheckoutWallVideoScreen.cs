using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public static class BuildNewCheckoutWallVideoScreen
{
    private const string ScreenName = "CHECKOUT_WALL_VIDEO_SCREEN_NEW";
    private const string VideoAsset = "Assets/Video/ObsidianProtocol_StoreScreen_Sample.mp4";
    private const string MaterialAsset = "Assets/Materials/StoreVideoScreens/CHECKOUT_WALL_VIDEO_FRESH.mat";

    [MenuItem("Obsidian Protocol/Store/BUILD NEW CHECKOUT WALL VIDEO SCREEN")]
    public static void Build()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (!scene.IsValid() || !scene.isLoaded ||
            !scene.path.Replace('\\', '/').EndsWith("SCN16_STORE/SCN16_STORE.unity"))
        {
            Debug.LogError("[CHECKOUT SCREEN BUILD] Open SCN16_STORE.unity first. No scene was changed.");
            return;
        }

        string videoPath = Path.GetFullPath(VideoAsset);
        if (!File.Exists(videoPath))
        {
            Debug.LogError("[CHECKOUT SCREEN BUILD] Video file not found: " + videoPath);
            return;
        }

        GameObject anchor = null;
        int bestScore = 0;

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            foreach (Transform t in root.GetComponentsInChildren<Transform>(true))
            {
                string n = t.name.ToLowerInvariant();
                int score = 0;
                if (n.Contains("checkout")) score += 100;
                if (n.Contains("cashier")) score += 80;
                if (n.Contains("register")) score += 60;
                if (n.Contains("counter")) score += 40;

                // Never use a video screen itself as the placement anchor.
                if (n.Contains("screen") || n.Contains("display") ||
                    n.Contains("video") || n.Contains("neon"))
                    score = 0;

                if (score > bestScore)
                {
                    bestScore = score;
                    anchor = t.gameObject;
                }
            }
        }

        if (anchor == null)
        {
            Debug.LogError(
                "[CHECKOUT SCREEN BUILD] No checkout/cashier/register/counter placement anchor was found. " +
                "Nothing was created; existing displays remain untouched.");
            return;
        }

        Bounds bounds = new Bounds(anchor.transform.position, Vector3.one);
        bool foundRenderer = false;
        foreach (Renderer r in anchor.GetComponentsInChildren<Renderer>(true))
        {
            if (!foundRenderer) { bounds = r.bounds; foundRenderer = true; }
            else bounds.Encapsulate(r.bounds);
        }

        if (!foundRenderer)
        {
            Debug.LogError("[CHECKOUT SCREEN BUILD] The checkout anchor has no renderer to measure. Nothing was created.");
            return;
        }

        const float screenWidth = 3.2f;
        const float screenHeight = screenWidth * 9f / 16f;
        Vector3 center = bounds.center;
        Vector3 position = new Vector3(center.x, bounds.max.y + screenHeight * 0.65f, center.z);

        // Face the screen toward the interior of the store.
        Vector3 interior = Vector3.zero;
        int rootCount = 0;
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            if (root == anchor || root.transform.IsChildOf(anchor.transform)) continue;
            interior += root.transform.position;
            rootCount++;
        }
        if (rootCount > 0) interior /= rootCount;
        Vector3 facing = interior - position;
        facing.y = 0f;
        if (facing.sqrMagnitude < 0.001f) facing = -anchor.transform.forward;
        Quaternion rotation = Quaternion.LookRotation(facing.normalized, Vector3.up) * Quaternion.Euler(0f, 90f, 0f);

        string materialDir = Path.GetDirectoryName(MaterialAsset);
        Directory.CreateDirectory(materialDir);

        Shader shader = Shader.Find("Universal Render Pipeline/Unlit");
        if (shader == null) shader = Shader.Find("Unlit/Texture");
        if (shader == null) shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");
        if (shader == null)
        {
            Debug.LogError("[CHECKOUT SCREEN BUILD] No compatible material shader was found. Nothing was created.");
            return;
        }

        // Only replace this builder's own generated material asset.
        Material material = AssetDatabase.LoadAssetAtPath<Material>(MaterialAsset);
        if (material == null)
        {
            material = new Material(shader);
            AssetDatabase.CreateAsset(material, MaterialAsset);
        }
        else
        {
            material.shader = shader;
        }

        string textureProperty = material.HasProperty("_BaseMap") ? "_BaseMap" :
                                 material.HasProperty("_MainTex") ? "_MainTex" : null;
        if (textureProperty == null)
        {
            Debug.LogError("[CHECKOUT SCREEN BUILD] Material has no supported video texture property. Nothing was created.");
            return;
        }

        // Remove only a previous copy made by this builder.
        GameObject previous = GameObject.Find(ScreenName);
        if (previous != null) Undo.DestroyObjectImmediate(previous);

        GameObject frame = GameObject.CreatePrimitive(PrimitiveType.Cube);
        frame.name = ScreenName;
        Undo.RegisterCreatedObjectUndo(frame, "Build checkout video screen");
        frame.transform.SetPositionAndRotation(position, rotation);
        frame.transform.localScale = new Vector3(screenWidth + 0.16f, screenHeight + 0.16f, 0.12f);
        frame.GetComponent<Renderer>().sharedMaterial = new Material(shader);
        frame.GetComponent<Renderer>().sharedMaterial.color = new Color(0.025f, 0.03f, 0.04f);
        Collider frameCollider = frame.GetComponent<Collider>();
        if (frameCollider != null) Object.DestroyImmediate(frameCollider);

        GameObject surface = GameObject.CreatePrimitive(PrimitiveType.Quad);
        surface.name = "Checkout Video Surface";
        Undo.RegisterCreatedObjectUndo(surface, "Build checkout video surface");
        surface.transform.SetParent(frame.transform, false);
        surface.transform.localPosition = new Vector3(0f, 0f, 0.51f);
        surface.transform.localRotation = Quaternion.identity;
        surface.transform.localScale = new Vector3(screenWidth / (screenWidth + 0.16f), screenHeight / (screenHeight + 0.16f), 1f);

        Collider surfaceCollider = surface.GetComponent<Collider>();
        if (surfaceCollider != null) Object.DestroyImmediate(surfaceCollider);
        surface.GetComponent<Renderer>().sharedMaterial = material;

        VideoPlayer player = frame.AddComponent<VideoPlayer>();
        player.playOnAwake = true;
        player.isLooping = true;
        player.source = VideoSource.Url;
        player.url = new System.Uri(videoPath).AbsoluteUri;
        player.renderMode = VideoRenderMode.MaterialOverride;
        player.targetMaterialRenderer = surface.GetComponent<Renderer>();
        player.targetMaterialProperty = textureProperty;
        player.audioOutputMode = VideoAudioOutputMode.None;
        player.waitForFirstFrame = true;

        EditorUtility.SetDirty(material);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();

        Debug.Log(
            "[CHECKOUT SCREEN BUILD SUCCESS] Created " + ScreenName +
            " above anchor '" + anchor.name + "'." +
            " Video=" + player.url +
            " | Surface=" + surface.name +
            " | Material=" + MaterialAsset +
            " | Scene=" + scene.path);
    }
}


