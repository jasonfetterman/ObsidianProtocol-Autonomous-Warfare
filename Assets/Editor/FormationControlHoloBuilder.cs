using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class FormationControlHoloBuilder
{
    const string ScenePath =
        "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

    const string LeftName =
        "[HOLO] FORMATION CONTROL - FORMATION";

    const string RightName =
        "[HOLO] FORMATION CONTROL - PARAMETERS";

    static readonly Color HoloColor =
        new Color(40f / 255f, 230f / 255f, 200f / 255f, 1f);

    [MenuItem("Obsidian Protocol/Deployment/Build Formation Control Holo Screens")]
    public static void Build()
    {
        var scene = EditorSceneManager.OpenScene(
            ScenePath,
            OpenSceneMode.Single
        );

        if (!scene.IsValid())
        {
            Debug.LogError("[HOLO] Formation Control scene failed to open.");
            return;
        }

        RemoveExisting(LeftName);
        RemoveExisting(RightName);

        BuildScreen(
            LeftName,
            new Vector3(-24.10f, 8.00f, 5.44f),
            new[]
            {
                "FORMATION CONTROL // STANDARD",
                "FORMATION TYPE // LINE",
                "FRONT ELEMENT // ASSIGNED",
                "CENTER ELEMENT // ASSIGNED",
                "REAR ELEMENT // ASSIGNED",
                "UNIT SPACING // STANDARD",
                "MOVEMENT COHESION // ENABLED",
                "FORMATION STATUS // READY"
            }
        );

        BuildScreen(
            RightName,
            new Vector3(-30.30f, 8.00f, 5.44f),
            new[]
            {
                "UNIT SPACING // STANDARD",
                "MOVEMENT SPEED // FORMATION",
                "TURN COHESION // ENABLED",
                "REACTION DISTANCE // STANDARD",
                "ENGAGEMENT ARC // FRONT",
                "RESERVE POSITION // REAR",
                "AUTONOMY ADJUSTMENT // CONTROLLED",
                "FORMATION STATUS // READY"
            }
        );

        EditorSceneManager.SaveScene(scene);

        Debug.Log("[HOLO] Formation Control COMPLETE.");
    }

    static void RemoveExisting(string objectName)
    {
        GameObject existing = GameObject.Find(objectName);

        if (existing != null)
            Object.DestroyImmediate(existing);
    }

    static void BuildScreen(
        string objectName,
        Vector3 worldPosition,
        string[] lines)
    {
        GameObject root = new GameObject(objectName);

        root.transform.position = worldPosition;
        root.transform.rotation = Quaternion.identity;
        root.transform.localScale =
            new Vector3(0.01f, 0.01f, 0.01f);

        Canvas canvas = root.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        RectTransform canvasRect =
            root.GetComponent<RectTransform>();

        canvasRect.sizeDelta =
            new Vector2(580f, 240f);

        CanvasScaler scaler =
            root.AddComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ConstantPixelSize;

        CreateGlass(root.transform);
        CreateBorder(root.transform, "TOP", 0f, 116f, 580f, 2f);
        CreateBorder(root.transform, "BOTTOM", 0f, -116f, 580f, 2f);
        CreateBorder(root.transform, "LEFT", -289f, 0f, 2f, 240f);
        CreateBorder(root.transform, "RIGHT", 289f, 0f, 2f, 240f);

        for (int i = 0; i < 8; i++)
        {
            float y = 86f - (i * 23f);

            CreateScanline(
                root.transform,
                "SCANLINE_" + i.ToString("00"),
                y
            );
        }

        for (int i = 1; i <= 7; i++)
        {
            float x = -217.5f + (i * 54.375f);

            CreateGridVertical(
                root.transform,
                "GRID VERTICAL " + i,
                x
            );
        }

        for (int i = 1; i <= 4; i++)
        {
            float y = -96f + (i * 38.4f);

            CreateGridHorizontal(
                root.transform,
                "GRID HORIZONTAL " + i,
                y
            );
        }

        CreateText(
            root.transform,
            "TITLE",
            "FORMATION CONTROL",
            new Vector2(0f, 94f),
            22,
            FontStyle.Bold
        );

        for (int i = 0; i < lines.Length; i++)
        {
            CreateText(
                root.transform,
                "DATA_" + i.ToString("00"),
                lines[i],
                new Vector2(-65f, 65f - (i * 22f)),
                14,
                FontStyle.Normal
            );
        }

        EditorUtility.SetDirty(root);
    }

    static void CreateGlass(Transform parent)
    {
        GameObject go = new GameObject("HOLO GLASS");

        go.transform.SetParent(parent, false);

        RectTransform rect =
            go.AddComponent<RectTransform>();

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(580f, 240f);

        Image image = go.AddComponent<Image>();

        image.color =
            new Color(
                HoloColor.r,
                HoloColor.g,
                HoloColor.b,
                0.035f
            );

        image.raycastTarget = false;
    }

    static void CreateBorder(
        Transform parent,
        string name,
        float x,
        float y,
        float width,
        float height)
    {
        GameObject go = new GameObject(
            "HOLO BORDER " + name
        );

        go.transform.SetParent(parent, false);

        RectTransform rect =
            go.AddComponent<RectTransform>();

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(x, y);
        rect.sizeDelta = new Vector2(width, height);

        Image image = go.AddComponent<Image>();

        image.color =
            new Color(
                HoloColor.r,
                HoloColor.g,
                HoloColor.b,
                0.65f
            );

        image.raycastTarget = false;
    }

    static void CreateScanline(
        Transform parent,
        string name,
        float y)
    {
        GameObject go = new GameObject(name);

        go.transform.SetParent(parent, false);

        RectTransform rect =
            go.AddComponent<RectTransform>();

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(0f, y);
        rect.sizeDelta = new Vector2(570f, 1f);

        Image image = go.AddComponent<Image>();

        image.color =
            new Color(
                HoloColor.r,
                HoloColor.g,
                HoloColor.b,
                0.07f
            );

        image.raycastTarget = false;
    }

    static void CreateGridVertical(
        Transform parent,
        string name,
        float x)
    {
        GameObject go = new GameObject(name);

        go.transform.SetParent(parent, false);

        RectTransform rect =
            go.AddComponent<RectTransform>();

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(x, 0f);
        rect.sizeDelta = new Vector2(1f, 220f);

        Image image = go.AddComponent<Image>();

        image.color =
            new Color(
                HoloColor.r,
                HoloColor.g,
                HoloColor.b,
                0.035f
            );

        image.raycastTarget = false;
    }

    static void CreateGridHorizontal(
        Transform parent,
        string name,
        float y)
    {
        GameObject go = new GameObject(name);

        go.transform.SetParent(parent, false);

        RectTransform rect =
            go.AddComponent<RectTransform>();

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(0f, y);
        rect.sizeDelta = new Vector2(560f, 1f);

        Image image = go.AddComponent<Image>();

        image.color =
            new Color(
                HoloColor.r,
                HoloColor.g,
                HoloColor.b,
                0.035f
            );

        image.raycastTarget = false;
    }

    static void CreateText(
        Transform parent,
        string name,
        string value,
        Vector2 position,
        int fontSize,
        FontStyle style)
    {
        GameObject go = new GameObject(name);

        go.transform.SetParent(parent, false);

        RectTransform rect =
            go.AddComponent<RectTransform>();

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(position.x + 35f, position.y);
        rect.sizeDelta = new Vector2(430f, 24f);

        Text text = go.AddComponent<Text>();

        text.text = value;
        text.font = Resources.GetBuiltinResource<Font>(
            "LegacyRuntime.ttf"
        );
        text.fontSize = fontSize;
        text.fontStyle = style;
        text.alignment = TextAnchor.MiddleLeft;
        text.horizontalOverflow =
            HorizontalWrapMode.Overflow;
        text.verticalOverflow =
            VerticalWrapMode.Overflow;

        text.color =
            new Color(
                HoloColor.r,
                HoloColor.g,
                HoloColor.b,
                0.92f
            );

        text.raycastTarget = false;
    }
}



