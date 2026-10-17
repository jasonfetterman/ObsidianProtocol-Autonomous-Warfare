using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Linq;

public static class AddVideoToExistingCheckoutScreen
{
    private const string VideoPath =
        "Assets/Video/ObsidianProtocol_StoreScreen_Sample.mp4";

    [MenuItem("Obsidian Protocol/Store/ADD VIDEO TO EXISTING CHECKOUT SCREEN")]
    public static void AddVideo()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (!scene.IsValid() || !scene.isLoaded ||
            !scene.path.Replace('\\', '/').EndsWith("SCN16_STORE/SCN16_STORE.unity"))
        {
            Debug.LogError("[CHECKOUT VIDEO] Open SCN16_STORE first.");
            return;
        }

        GameObject screen = GameObject.Find("CHECKOUT_WALL_VIDEO_SCREEN_NEW");
        Transform surface = screen != null
            ? screen.transform.Find("Checkout Video Surface") : null;

        if (surface == null)
        {
            Debug.LogError("[CHECKOUT VIDEO] Checkout screen or surface not found.");
            return;
        }

        Renderer targetRenderer = surface.GetComponent<Renderer>();
        if (targetRenderer == null || targetRenderer.sharedMaterial == null)
        {
            Debug.LogError("[CHECKOUT VIDEO] Surface renderer/material missing.");
            return;
        }

        VideoClip clip = AssetDatabase.LoadAssetAtPath<VideoClip>(VideoPath);
        if (clip == null)
        {
            Debug.LogError("[CHECKOUT VIDEO] Video clip not found: " + VideoPath);
            return;
        }

        VideoPlayer[] players =
            Object.FindObjectsByType<VideoPlayer>(
                FindObjectsInactive.Include, FindObjectsSortMode.None);

        VideoPlayer source = players
            .Where(p => p != null && p.gameObject != surface.gameObject &&
                        p.clip == clip && p.targetTexture != null)
            .FirstOrDefault();

        if (source == null)
            source = players.FirstOrDefault(
                p => p != null && p.gameObject != surface.gameObject &&
                     p.clip == clip);

        Material template = source != null &&
                            source.targetMaterialRenderer != null &&
                            source.targetMaterialRenderer.sharedMaterial != null
            ? source.targetMaterialRenderer.sharedMaterial
            : targetRenderer.sharedMaterial;

        Material newMaterial = new Material(template);
        string materialPath = AssetDatabase.GenerateUniqueAssetPath(
            "Assets/Video/CheckoutVideoScreen_Material.mat");

        AssetDatabase.CreateAsset(newMaterial, materialPath);
        AssetDatabase.SaveAssets();

        targetRenderer.sharedMaterial = newMaterial;

        string property = newMaterial.HasProperty("_BaseMap") ? "_BaseMap" :
                          newMaterial.HasProperty("_MainTex") ? "_MainTex" :
                          newMaterial.HasProperty("_UnlitColorMap") ? "_UnlitColorMap" : null;

        if (property == null)
        {
            Debug.LogError("[CHECKOUT VIDEO] Material shader has no supported video texture property.");
            return;
        }

        if (newMaterial.HasProperty("_BaseColor"))
            newMaterial.SetColor("_BaseColor", Color.white);
        if (newMaterial.HasProperty("_Color"))
            newMaterial.SetColor("_Color", Color.white);

        VideoPlayer local = surface.GetComponent<VideoPlayer>();

        if (source != null && source.targetTexture != null)
        {
            if (local != null)
                Undo.DestroyObjectImmediate(local);

            newMaterial.SetTexture(property, source.targetTexture);
            EditorUtility.SetDirty(newMaterial);
            EditorUtility.SetDirty(targetRenderer);
        }
        else
        {
            if (local == null)
                local = Undo.AddComponent<VideoPlayer>(surface.gameObject);
            else
                Undo.RecordObject(local, "Connect checkout video");

            local.source = VideoSource.VideoClip;
            local.clip = clip;
            local.playOnAwake = true;
            local.isLooping = true;
            local.waitForFirstFrame = true;
            local.renderMode = VideoRenderMode.MaterialOverride;
            local.targetTexture = null;
            local.targetMaterialRenderer = targetRenderer;
            local.targetMaterialProperty = property;
            local.audioOutputMode = VideoAudioOutputMode.None;
            EditorUtility.SetDirty(local);
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("[CHECKOUT VIDEO] Checkout screen connected. Screen transforms and other TV objects were not changed.");
    }
}
