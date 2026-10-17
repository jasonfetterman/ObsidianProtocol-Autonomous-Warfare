using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public static class DoctrineControlHoloBuilder
{
    private const string ScenePath =
        "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

    [MenuItem("Obsidian Protocol/Deployment/Build Doctrine Control Holo Screens")]
    public static void Build()
    {
        var scene = EditorSceneManager.OpenScene(ScenePath);

        RemoveExisting("[HOLO] DOCTRINE CONTROL");

        CreateHolo(
            "[HOLO] DOCTRINE CONTROL - DOCTRINE",
            new Vector3(12.16f, 8.00f, 5.44f),
            new Vector3(0f, 0f, 0f),
            "DOCTRINE CONTROL",
            new string[]
            {
                "CURRENT DOCTRINE  //  STANDARD",
                "COMMAND PHILOSOPHY  //  BALANCED",
                "AUTONOMY DOCTRINE  //  CONTROLLED",
                "FORCE DOCTRINE  //  PRESERVATION",
                "OPERATIONAL DOCTRINE  //  OBJECTIVE",
                "INITIATIVE  //  MODERATE",
                "ADAPTATION  //  ENABLED",
                "DOCTRINE STATUS  //  READY"
            }
        );

        CreateHolo(
            "[HOLO] DOCTRINE CONTROL - PARAMETERS",
            new Vector3(5.96f, 8.00f, 5.44f),
            new Vector3(0f, 0f, 0f),
            "DOCTRINE PARAMETERS",
            new string[]
            {
                "AGGRESSION  //  MODERATE",
                "INITIATIVE  //  MODERATE",
                "RISK POSTURE  //  BALANCED",
                "AUTONOMY THRESHOLD  //  CONTROLLED",
                "FORCE PRESERVATION  //  ENABLED",
                "ADAPTATION  //  ENABLED",
                "COMMAND OVERRIDE  //  ACTIVE",
                "PARAMETER STATUS  //  READY"
            }
        );

        EditorSceneManager.SaveScene(scene);

        Debug.Log("[HOLO] Doctrine Control COMPLETE.");
    }

    private static void CreateHolo(
        string objectName,
        Vector3 position,
        Vector3 rotation,
        string title,
        string[] lines)
    {
        GameObject root = new GameObject(objectName);

        root.transform.position = position;
        root.transform.rotation = Quaternion.Euler(rotation);
        root.transform.localScale = Vector3.one * 0.01f;

        Canvas canvas = root.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(580f, 240f);

        canvasRect.localPosition = Vector3.zero;
        canvasRect.localRotation = Quaternion.identity;
        canvasRect.localScale = Vector3.one;

        CanvasScaler scaler = root.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        scaler.scaleFactor = 1f;

        GraphicRaycaster raycaster = root.AddComponent<GraphicRaycaster>();

        CreateBackground(root.transform);
        CreateBorder(root.transform);
        CreateTitle(root.transform, title);
        CreateLines(root.transform, lines);

        Selection.activeGameObject = root;
    }

    private static void CreateBackground(Transform parent)
    {
        GameObject background = new GameObject("HOLO GLASS");
        background.transform.SetParent(parent, false);

        Image image = background.AddComponent<Image>();
        image.color = new Color(0.06f, 0.02f, 0.10f, 0.32f);

        RectTransform rect = image.rectTransform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    private static void CreateBorder(Transform parent)
    {
        CreateLine(parent, "BORDER TOP",
            new Vector2(0f, 118f),
            new Vector2(580f, 2f));

        CreateLine(parent, "BORDER BOTTOM",
            new Vector2(0f, -118f),
            new Vector2(580f, 2f));

        CreateLine(parent, "BORDER LEFT",
            new Vector2(-289f, 0f),
            new Vector2(2f, 236f));

        CreateLine(parent, "BORDER RIGHT",
            new Vector2(289f, 0f),
            new Vector2(2f, 236f));
    }

    private static void CreateLine(
        Transform parent,
        string name,
        Vector2 position,
        Vector2 size)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);

        Image image = obj.AddComponent<Image>();
        image.color = new Color(0.67f, 0.27f, 1f, 0.72f);

        RectTransform rect = image.rectTransform;
        rect.sizeDelta = size;
        rect.anchoredPosition = position;
    }

    private static void CreateTitle(Transform parent, string title)
    {
        GameObject obj = new GameObject("TITLE");
        obj.transform.SetParent(parent, false);

        TextMeshProUGUI text = obj.AddComponent<TextMeshProUGUI>();

        text.text = "◆ " + title;
        text.fontSize = 22f;
        text.fontStyle = FontStyles.Bold;
        text.alignment = TextAlignmentOptions.Left;
        text.color = new Color(0.67f, 0.27f, 1f, 0.95f);

        RectTransform rect = text.rectTransform;
        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(0.5f, 1f);
        rect.offsetMin = new Vector2(22f, -42f);
        rect.offsetMax = new Vector2(-22f, -12f);
    }

    private static void CreateLines(
        Transform parent,
        string[] lines)
    {
        float startY = 82f;
        float spacing = 23f;

        for (int i = 0; i < lines.Length; i++)
        {
            GameObject obj = new GameObject("DATA_" + i.ToString("00"));
            obj.transform.SetParent(parent, false);

            TextMeshProUGUI text = obj.AddComponent<TextMeshProUGUI>();

            text.text = lines[i];
            text.fontSize = 13f;
            text.alignment = TextAlignmentOptions.Left;
            text.color = new Color(0.78f, 0.62f, 1f, 0.90f);

            RectTransform rect = text.rectTransform;
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.pivot = new Vector2(0.5f, 1f);

            float y = startY - (i * spacing);

            rect.offsetMin = new Vector2(24f, y - 18f);
            rect.offsetMax = new Vector2(-24f, y);
        }
    }

    private static void RemoveExisting(string prefix)
    {
        GameObject[] roots = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject go in roots)
        {
            if (go.scene.IsValid() &&
                go.transform.parent == null &&
                go.name.StartsWith(prefix))
            {
                Object.DestroyImmediate(go);
            }
        }
    }
}
