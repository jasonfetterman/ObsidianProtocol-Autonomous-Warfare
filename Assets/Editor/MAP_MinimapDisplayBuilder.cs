using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public static class MAP_MinimapDisplayBuilder
{
    private const string ScenePath =
        "Assets/Scenes/SCN03_BATTLEFIELD_AAA/SCN03_BATTLEFIELD_AAA.unity";

    private const string RenderTexturePath =
        "Assets/UI/HUD/Battlefield/BattleHUD/Battle_Minimap_RT.renderTexture";

    [MenuItem("Obsidian Protocol/Battlefield/BUILD MINIMAP DISPLAY")]
    public static void Build()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.path != ScenePath)
        {
            EditorUtility.DisplayDialog(
                "Wrong Scene",
                "Open SCN03_BATTLEFIELD_AAA before building the minimap.",
                "OK");
            return;
        }

        GameObject panel = GameObject.Find("Minimap_Panel");

        if (panel == null)
        {
            EditorUtility.DisplayDialog(
                "Minimap Panel Missing",
                "Could not find Minimap_Panel in the active scene.",
                "OK");
            return;
        }

        Camera minimapCamera = GameObject.Find("Minimap_Camera")?.GetComponent<Camera>();
        if (minimapCamera != null)
        {
            Undo.RecordObject(minimapCamera, "Configure Minimap Camera");
            minimapCamera.useOcclusionCulling = false;
            EditorUtility.SetDirty(minimapCamera);
        }

        RenderTexture renderTexture =
            AssetDatabase.LoadAssetAtPath<RenderTexture>(RenderTexturePath);

        if (renderTexture == null)
        {
            EditorUtility.DisplayDialog(
                "RenderTexture Missing",
                "Could not load Battle_Minimap_RT.renderTexture.",
                "OK");
            return;
        }

        Transform existing = panel.transform.Find("MINIMAP_RENDER_DISPLAY");

        GameObject displayObject;

        if (existing != null)
        {
            displayObject = existing.gameObject;
        }
        else
        {
            displayObject = new GameObject(
                "MINIMAP_RENDER_DISPLAY",
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(RawImage));

            Undo.RegisterCreatedObjectUndo(
                displayObject,
                "Create Minimap Render Display");

            displayObject.transform.SetParent(panel.transform, false);
        }

        RectTransform rect = displayObject.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        rect.localScale = Vector3.one;
        rect.localRotation = Quaternion.identity;

        RawImage rawImage = displayObject.GetComponent<RawImage>();
        rawImage.texture = renderTexture;
        rawImage.color = Color.white;
        rawImage.raycastTarget = false;
        rawImage.uvRect = new Rect(0f, 0f, 1f, 1f);

        displayObject.transform.SetAsFirstSibling();

        EditorUtility.SetDirty(displayObject);
        EditorSceneManager.MarkSceneDirty(scene);

        Selection.activeGameObject = displayObject;

        Debug.Log(
            "[Minimap] RenderTexture display built and connected successfully.");

        EditorUtility.DisplayDialog(
            "Minimap Display Built",
            "The minimap RawImage is now connected to Battle_Minimap_RT.",
            "OK");
    }
}

