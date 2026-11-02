using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class AcheronSpaceBackgroundBuilder
{
    private const string TexturePath =
        "Assets/Textures/AcheronSpaceBackground.png";

    private const string BackgroundName =
        "ACHERON_SPACE_BACKGROUND";

    [MenuItem("Obsidian Protocol/Acheron/ADD SPACE BACKGROUND")]
    public static void Build()
    {
        Texture2D texture =
            AssetDatabase.LoadAssetAtPath<Texture2D>(TexturePath);

        if (texture == null)
        {
            Debug.LogError(
                "ACHERON: Background not found at " + TexturePath
            );
            return;
        }

        Canvas canvas = Object.FindFirstObjectByType<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("ACHERON: No Canvas found.");
            return;
        }

        Transform old = canvas.transform.Find(BackgroundName);

        if (old != null)
            Object.DestroyImmediate(old.gameObject);

        GameObject background = new GameObject(
            BackgroundName,
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(RawImage)
        );

        background.transform.SetParent(canvas.transform, false);

        RectTransform rect =
            background.GetComponent<RectTransform>();

        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        RawImage image =
            background.GetComponent<RawImage>();

        image.texture = texture;
        image.color = Color.white;
        image.raycastTarget = false;

        background.transform.SetAsFirstSibling();

        EditorUtility.SetDirty(canvas.gameObject);

        Debug.Log(
            "ACHERON: SPACE BACKGROUND ADDED BEHIND HUD."
        );
    }
}
