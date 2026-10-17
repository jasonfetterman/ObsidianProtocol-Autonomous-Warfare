using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class RulesOfEngagementHoloBuilder
{
    private static readonly Color Gold =
        new Color(1f, 190f / 255f, 40f / 255f, 1f);

    [MenuItem("Obsidian Protocol/Deployment/Build Rules Of Engagement Holo Screens")]
    public static void Build()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (!scene.IsValid())
        {
            Debug.LogError("[HOLO] Invalid active scene.");
            return;
        }

        RemoveExisting("[HOLO] RULES OF ENGAGEMENT - ROE");
        RemoveExisting("[HOLO] RULES OF ENGAGEMENT - PARAMETERS");

        CreateScreen(
            "[HOLO] RULES OF ENGAGEMENT - ROE",
            "RULES OF ENGAGEMENT",
            new Vector3(30.15f, 8.00f, 5.44f),
            new Vector3(0f, 0f, 0f),
            new string[]
            {
                "CURRENT ROE            STANDARD",
                "ENGAGEMENT AUTHORITY   COMMAND",
                "WEAPONS AUTHORIZATION  RESTRICTED",
                "HOSTILE IDENTIFICATION REQUIRED",
                "",
                "CIVILIAN PROTECTION    ACTIVE",
                "ESCALATION PROTOCOL    ENABLED",
                "",
                "ROE STATUS             READY"
            }
        );

        CreateScreen(
            "[HOLO] RULES OF ENGAGEMENT - PARAMETERS",
            "ENGAGEMENT PARAMETERS",
            new Vector3(23.95f, 8.00f, 5.44f),
            new Vector3(0f, 0f, 0f),
            new string[]
            {
                "WEAPONS STATE          RESTRICTED",
                "TARGET CONFIRMATION    REQUIRED",
                "FIRE AUTHORIZATION     COMMAND",
                "PURSUIT AUTHORITY      LIMITED",
                "",
                "MINIMUM FORCE          ENABLED",
                "ABORT CONDITIONS       ACTIVE",
                "OVERRIDE AUTHORITY     COMMAND",
                "",
                "COMPLIANCE STATUS      READY"
            }
        );

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[HOLO] Rules Of Engagement COMPLETE.");
    }

    private static void CreateScreen(
        string rootName,
        string title,
        Vector3 position,
        Vector3 rotation,
        string[] lines)
    {
        GameObject root = new GameObject(rootName);

        root.transform.position = position;
        root.transform.eulerAngles = rotation;
        root.transform.localScale = Vector3.one * 0.01f;

        Canvas canvas = root.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;

        RectTransform rect = root.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(580f, 240f);

        CanvasScaler scaler = root.AddComponent<CanvasScaler>();
        scaler.dynamicPixelsPerUnit = 10f;

        root.AddComponent<GraphicRaycaster>();

        CreateGlass(root.transform);
        CreateBorder(root.transform);

        CreateText(
            "TITLE",
            root.transform,
            title,
            new Vector2(0f, 92f),
            new Vector2(520f, 32f),
            24
        );

        GameObject scanline = CreateUI(
            "SCANLINE",
            root.transform,
            new Vector2(0f, 70f),
            new Vector2(520f, 1f)
        );

        scanline.AddComponent<Image>().color =
            new Color(Gold.r, Gold.g, Gold.b, 0.45f);

        float y = 45f;

        foreach (string line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                y -= 11f;
                continue;
            }

            CreateText(
                "DATA",
                root.transform,
                line,
                new Vector2(0f, y),
                new Vector2(510f, 18f),
                13
            );

            y -= 18f;

            if (y < -88f)
                break;
        }

        for (int i = -2; i <= 2; i++)
        {
            GameObject grid = CreateUI(
                "GRID LINE",
                root.transform,
                new Vector2(i * 100f, 0f),
                new Vector2(1f, 205f)
            );

            grid.AddComponent<Image>().color =
                new Color(Gold.r, Gold.g, Gold.b, 0.035f);
        }
    }

    private static void CreateGlass(Transform parent)
    {
        GameObject glass = CreateUI(
            "HOLO GLASS",
            parent,
            Vector2.zero,
            new Vector2(570f, 230f)
        );

        glass.AddComponent<Image>().color =
            new Color(
                Gold.r * 0.025f,
                Gold.g * 0.025f,
                Gold.b * 0.025f,
                0.72f
            );
    }

    private static void CreateBorder(Transform parent)
    {
        CreateBorderPart(parent, "BORDER TOP",
            new Vector2(0f, 112f), new Vector2(570f, 1.5f));

        CreateBorderPart(parent, "BORDER BOTTOM",
            new Vector2(0f, -112f), new Vector2(570f, 1.5f));

        CreateBorderPart(parent, "BORDER LEFT",
            new Vector2(-285f, 0f), new Vector2(1.5f, 225f));

        CreateBorderPart(parent, "BORDER RIGHT",
            new Vector2(285f, 0f), new Vector2(1.5f, 225f));
    }

    private static void CreateBorderPart(
        Transform parent,
        string name,
        Vector2 position,
        Vector2 size)
    {
        GameObject go = CreateUI(name, parent, position, size);

        go.AddComponent<Image>().color =
            new Color(Gold.r, Gold.g, Gold.b, 0.85f);
    }

    private static GameObject CreateUI(
        string name,
        Transform parent,
        Vector2 position,
        Vector2 size)
    {
        GameObject go = new GameObject(name);

        go.transform.SetParent(parent, false);

        RectTransform rect = go.AddComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        return go;
    }

    private static void CreateText(
        string name,
        Transform parent,
        string value,
        Vector2 position,
        Vector2 size,
        int fontSize)
    {
        GameObject go = CreateUI(name, parent, position, size);

        Text text = go.AddComponent<Text>();

        text.text = value;
        text.font =
            Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        text.fontSize = fontSize;
        text.alignment = TextAnchor.MiddleLeft;
        text.color =
            new Color(Gold.r, Gold.g, Gold.b, 0.92f);

        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;
    }

    private static void RemoveExisting(string name)
    {
        GameObject existing = GameObject.Find(name);

        if (existing != null)
        {
            Object.DestroyImmediate(existing);
            Debug.Log("[HOLO] Removed " + name);
        }
    }
}
