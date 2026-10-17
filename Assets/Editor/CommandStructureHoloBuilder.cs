using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class CommandStructureHoloBuilder
{
    const string ScenePath =
        "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

    const string LeftName =
        "[HOLO] COMMAND STRUCTURE - COMMAND";

    const string RightName =
        "[HOLO] COMMAND STRUCTURE - ORGANIZATION";

    [MenuItem("Obsidian Protocol/Deployment/Build Command Structure Holo Screens")]
    public static void Build()
    {
        var scene = EditorSceneManager.OpenScene(
            ScenePath,
            OpenSceneMode.Single
        );

        DeleteExisting(LeftName);
        DeleteExisting(RightName);

        BuildScreen(
            LeftName,
            new Vector3(-5.59f, 8.00f, 5.44f),
            new Vector3(0f, 0f, 0f),
            "◆ COMMAND STRUCTURE",
            new[]
            {
                "COMMAND AUTHORITY // COMMANDER",
                "MISSION CONTROL // ACTIVE",
                "FORCE CONTROL // ENABLED",
                "AUTONOMY AUTHORITY // CONTROLLED",
                "ENGAGEMENT AUTHORITY // COMMAND",
                "SUPPORT AUTHORITY // LOGISTICS",
                "OVERRIDE AUTHORITY // COMMAND",
                "COMMAND STATUS // READY"
            }
        );

        BuildScreen(
            RightName,
            new Vector3(-11.79f, 8.00f, 5.44f),
            new Vector3(0f, 0f, 0f),
            "◆ COMMAND ORGANIZATION",
            new[]
            {
                "COMMANDER // PRIMARY",
                "COMMAND ELEMENT // ACTIVE",
                "UNIT COMMAND // ASSIGNED",
                "SUPPORT ELEMENT // ASSIGNED",
                "LOGISTICS ELEMENT // ASSIGNED",
                "RECON ELEMENT // ASSIGNED",
                "RESERVE ELEMENT // READY",
                "ORGANIZATION STATUS // READY"
            }
        );

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[HOLO] Command Structure COMPLETE.");
    }

    static void DeleteExisting(string name)
    {
        GameObject existing = GameObject.Find(name);

        if (existing != null)
            Object.DestroyImmediate(existing);
    }

    static void BuildScreen(
        string rootName,
        Vector3 position,
        Vector3 rotation,
        string title,
        string[] lines)
    {
        GameObject root = new GameObject(rootName);

        RectTransform rootRect =
            root.AddComponent<RectTransform>();

        root.transform.position = position;
        root.transform.rotation =
            Quaternion.Euler(rotation);
        root.transform.localScale =
            new Vector3(0.01f, 0.01f, 0.01f);

        Canvas canvas =
            root.AddComponent<Canvas>();

        canvas.renderMode =
            RenderMode.WorldSpace;

        canvas.sortingOrder = 50;

        CanvasScaler scaler =
            root.AddComponent<CanvasScaler>();

        scaler.dynamicPixelsPerUnit = 10f;

        rootRect.sizeDelta =
            new Vector2(580f, 240f);

        Image glass =
            CreateImage(
                root.transform,
                "HOLO GLASS",
                new Color(0.015f, 0.035f, 0.055f, 0.28f)
            );

        SetFull(glass.rectTransform);

        Color accent =
            new Color(
                80f / 255f,
                190f / 255f,
                255f / 255f,
                1f
            );

        CreateBorder(
            root.transform,
            "HOLO BORDER TOP",
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(0f, -2f),
            new Vector2(0f, 0f),
            accent
        );

        CreateBorder(
            root.transform,
            "HOLO BORDER BOTTOM",
            new Vector2(0f, 0f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
            new Vector2(0f, 2f),
            accent
        );

        CreateBorder(
            root.transform,
            "HOLO BORDER LEFT",
            new Vector2(0f, 0f),
            new Vector2(0f, 1f),
            new Vector2(0f, 0f),
            new Vector2(2f, 0f),
            accent
        );

        CreateBorder(
            root.transform,
            "HOLO BORDER RIGHT",
            new Vector2(1f, 0f),
            new Vector2(1f, 1f),
            new Vector2(-2f, 0f),
            new Vector2(0f, 0f),
            accent
        );

        CreateScanlines(root.transform, accent);
        CreateGrid(root.transform, accent);

        CreateText(
            root.transform,
            "TITLE",
            title,
            20,
            accent,
            new Vector2(24f, -12f),
            new Vector2(-24f, 0f),
            TextAnchor.UpperLeft
        );

        for (int i = 0; i < lines.Length; i++)
        {
            CreateText(
                root.transform,
                "DATA_" + i.ToString("00"),
                lines[i],
                13,
                new Color(
                    170f / 255f,
                    220f / 255f,
                    255f / 255f,
                    0.92f
                ),
                new Vector2(28f, -(48f + i * 21f)),
                new Vector2(-28f, -(30f + i * 21f)),
                TextAnchor.UpperLeft
            );
        }
    }

    static Image CreateImage(
        Transform parent,
        string name,
        Color color)
    {
        GameObject go =
            new GameObject(name);

        go.transform.SetParent(
            parent,
            false
        );

        Image image =
            go.AddComponent<Image>();

        image.color = color;
        image.raycastTarget = false;

        return image;
    }

    static void CreateBorder(
        Transform parent,
        string name,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 offsetMin,
        Vector2 offsetMax,
        Color color)
    {
        Image image =
            CreateImage(
                parent,
                name,
                color
            );

        RectTransform r =
            image.rectTransform;

        r.anchorMin = anchorMin;
        r.anchorMax = anchorMax;
        r.offsetMin = offsetMin;
        r.offsetMax = offsetMax;
    }

    static void CreateScanlines(
        Transform parent,
        Color accent)
    {
        for (int i = 0; i < 8; i++)
        {
            Image line =
                CreateImage(
                    parent,
                    "SCANLINE_" + i.ToString("00"),
                    new Color(
                        accent.r,
                        accent.g,
                        accent.b,
                        0.035f
                    )
                );

            RectTransform r =
                line.rectTransform;

            r.anchorMin =
                new Vector2(0f, 1f);

            r.anchorMax =
                new Vector2(1f, 1f);

            r.offsetMin =
                new Vector2(
                    8f,
                    -35f - i * 25f
                );

            r.offsetMax =
                new Vector2(
                    -8f,
                    -34f - i * 25f
                );
        }
    }

    static void CreateGrid(
        Transform parent,
        Color accent)
    {
        for (int i = 1; i < 8; i++)
        {
            Image vertical =
                CreateImage(
                    parent,
                    "GRID VERTICAL " + i,
                    new Color(
                        accent.r,
                        accent.g,
                        accent.b,
                        0.025f
                    )
                );

            RectTransform vr =
                vertical.rectTransform;

            float x = i / 8f;

            vr.anchorMin =
                new Vector2(x, 0f);

            vr.anchorMax =
                new Vector2(x, 1f);

            vr.offsetMin =
                new Vector2(-0.5f, 4f);

            vr.offsetMax =
                new Vector2(0.5f, -4f);
        }

        for (int i = 1; i < 5; i++)
        {
            Image horizontal =
                CreateImage(
                    parent,
                    "GRID HORIZONTAL " + i,
                    new Color(
                        accent.r,
                        accent.g,
                        accent.b,
                        0.025f
                    )
                );

            RectTransform hr =
                horizontal.rectTransform;

            float y = i / 5f;

            hr.anchorMin =
                new Vector2(0f, y);

            hr.anchorMax =
                new Vector2(1f, y);

            hr.offsetMin =
                new Vector2(4f, -0.5f);

            hr.offsetMax =
                new Vector2(-4f, 0.5f);
        }
    }

    static Text CreateText(
        Transform parent,
        string name,
        string value,
        int fontSize,
        Color color,
        Vector2 offsetMin,
        Vector2 offsetMax,
        TextAnchor alignment)
    {
        GameObject go =
            new GameObject(name);

        go.transform.SetParent(
            parent,
            false
        );

        Text text =
            go.AddComponent<Text>();

        text.text = value;
        text.fontSize = fontSize;
        text.color = color;
        text.alignment = alignment;
        text.horizontalOverflow =
            HorizontalWrapMode.Overflow;
        text.verticalOverflow =
            VerticalWrapMode.Overflow;
        text.raycastTarget = false;

        RectTransform r =
            go.GetComponent<RectTransform>();

        r.anchorMin =
            new Vector2(0f, 1f);

        r.anchorMax =
            new Vector2(1f, 1f);

        r.pivot =
            new Vector2(0.5f, 1f);

        r.offsetMin = offsetMin;
        r.offsetMax = offsetMax;

        return text;
    }

    static void SetFull(
        RectTransform r)
    {
        r.anchorMin =
            Vector2.zero;

        r.anchorMax =
            Vector2.one;

        r.offsetMin =
            Vector2.zero;

        r.offsetMax =
            Vector2.zero;
    }
}
