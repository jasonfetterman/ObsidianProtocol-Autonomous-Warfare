using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ObsidianProtocol.Game.UI.Battlefield;

public static class BuildAAATacticalMapViewport
{
    private const string ScenePath =
        "Assets/Scenes/SCN03_BATTLEFIELD_AAA/SCN03_BATTLEFIELD_AAA.unity";
    private const string AssetFolder = "Assets/Art/UI/TacticalMap";
    private const string RenderTexturePath =
        AssetFolder + "/RT_AAA_TacticalMap.renderTexture";

    [MenuItem("OPAW/AAA Battlefield/Repair Live Tactical Map")]
    public static void Build()
    {
        if (EditorApplication.isCompiling)
        {
            Debug.LogWarning("[OPAW Tactical Map] Wait for compilation to finish.");
            return;
        }

        if (!File.Exists(ScenePath))
        {
            Debug.LogError("[OPAW Tactical Map] Scene not found: " + ScenePath);
            return;
        }

        Scene scene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene(
            ScenePath, UnityEditor.SceneManagement.OpenSceneMode.Single);

        GameObject mapField = GameObject.Find("[MAP FIELD]");
        GameObject cameraObject = GameObject.Find("[CAMERA] AAA TACTICAL MAP");

        if (mapField == null || cameraObject == null)
        {
            Debug.LogError(
                "[OPAW Tactical Map] Required existing map field or camera is missing. No repair performed.");
            return;
        }

        Camera mapCamera = cameraObject.GetComponent<Camera>();
        if (mapCamera == null)
        {
            Debug.LogError("[OPAW Tactical Map] Existing tactical camera has no Camera component.");
            return;
        }

        Directory.CreateDirectory(AssetFolder);
        AssetDatabase.Refresh();

        RenderTexture rt =
            AssetDatabase.LoadAssetAtPath<RenderTexture>(RenderTexturePath);

        if (rt == null)
        {
            rt = mapCamera.targetTexture;
        }

        if (rt == null)
        {
            Debug.LogError(
                "[OPAW Tactical Map] Existing RenderTexture could not be found. " +
                "Stopped rather than silently replacing the camera's texture.");
            return;
        }

        // Reuse the existing texture and overhead camera.
        mapCamera.targetTexture = rt;
        mapCamera.orthographic = true;
        mapCamera.orthographicSize = 500f;
        mapCamera.nearClipPlane = 0.3f;
        mapCamera.farClipPlane = 1200f;
        mapCamera.transform.position = new Vector3(0f, 800f, 0f);
        mapCamera.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        mapCamera.allowHDR = false;
        mapCamera.allowMSAA = false;

        // Reuse one of the existing RawImages; remove duplicate siblings only.
        RawImage[] rawImages = mapField.GetComponentsInChildren<RawImage>(true);
        RawImage viewport = null;

        foreach (RawImage image in rawImages)
        {
            if (image == null) continue;

            if (viewport == null)
                viewport = image;
            else if (image.transform.parent == mapField.transform)
            {
                Undo.DestroyObjectImmediate(image.gameObject);
            }
        }

        if (viewport == null)
        {
            var go = new GameObject(
                "[RAW IMAGE] AAA TACTICAL MAP",
                typeof(RectTransform), typeof(CanvasRenderer), typeof(RawImage));

            Undo.RegisterCreatedObjectUndo(go, "Create Tactical Map Viewport");
            go.transform.SetParent(mapField.transform, false);
            viewport = go.GetComponent<RawImage>();
        }

        viewport.texture = rt;
        viewport.color = Color.white;
        viewport.raycastTarget = false;
        viewport.uvRect = new Rect(0f, 0f, 1f, 1f);

        RectTransform viewportRect = viewport.rectTransform;
        viewportRect.anchorMin = Vector2.zero;
        viewportRect.anchorMax = Vector2.one;
        viewportRect.offsetMin = new Vector2(4f, 4f);
        viewportRect.offsetMax = new Vector2(-4f, -4f);
        viewportRect.localScale = Vector3.one;

        // Keep the map background transparent so the live texture is visible.
        Image background = mapField.GetComponent<Image>();
        if (background != null)
        {
            Color color = background.color;
            color.a = 0f;
            background.color = color;
            EditorUtility.SetDirty(background);
        }

        // Attach marker logic to the retained RawImage, not the background.
        LiveTacticalMap liveMap = viewport.GetComponent<LiveTacticalMap>();
        if (liveMap == null)
            liveMap = Undo.AddComponent<LiveTacticalMap>(viewport.gameObject);

        SerializedObject so = new SerializedObject(liveMap);
        so.FindProperty("worldMin").vector2Value = new Vector2(-500f, -500f);
        so.FindProperty("worldMax").vector2Value = new Vector2(500f, 500f);
        so.ApplyModifiedPropertiesWithoutUndo();

        GameObject placeholder = GameObject.Find("[MAP PLACEHOLDER]");
        if (placeholder != null && placeholder.activeSelf)
        {
            Undo.RecordObject(placeholder, "Hide Tactical Map Placeholder");
            placeholder.SetActive(false);
            EditorUtility.SetDirty(placeholder);
        }

        EditorUtility.SetDirty(mapCamera);
        EditorUtility.SetDirty(viewport);
        EditorUtility.SetDirty(mapField);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);

        if (!UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene))
        {
            Debug.LogError("[OPAW Tactical Map] Scene save failed.");
            return;
        }

        AssetDatabase.SaveAssets();

        Debug.Log(
            "[OPAW Tactical Map] Repair complete. Existing camera and RenderTexture reused; " +
            "one RawImage retained; live unit-marker script attached; bounds set to X/Z -500..500. " +
            "Tactical grid and gameplay camera were not intentionally changed.");
    }
}
