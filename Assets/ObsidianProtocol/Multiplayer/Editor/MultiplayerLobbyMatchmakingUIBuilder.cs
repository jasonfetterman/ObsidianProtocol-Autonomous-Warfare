using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public static class MultiplayerLobbyMatchmakingUIBuilder
{
    [MenuItem("Obsidian Protocol/Multiplayer/BUILD MATCHMAKING UI")]
    public static void Build()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (!scene.IsValid())
        {
            Debug.LogError("[MULTIPLAYER] No valid scene open.");
            return;
        }

        Canvas canvas = Object.FindFirstObjectByType<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("[MULTIPLAYER] Canvas not found.");
            return;
        }

        Transform existing =
            canvas.transform.Find("MATCHMAKING_PANEL");

        if (existing != null)
        {
            Object.DestroyImmediate(existing.gameObject);
        }

        GameObject panel =
            new GameObject(
                "MATCHMAKING_PANEL",
                typeof(RectTransform),
                typeof(Image));

        panel.transform.SetParent(canvas.transform, false);

        RectTransform rect =
            panel.GetComponent<RectTransform>();

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(900f, 620f);

        Image background =
            panel.GetComponent<Image>();

        background.color =
            new Color(0.015f, 0.02f, 0.03f, 0.97f);

        CreateText(
            panel.transform,
            "TITLE",
            "MATCHMAKING",
            42,
            new Vector2(0f, 245f),
            new Vector2(800f, 70f),
            TextAnchor.MiddleCenter,
            FontStyle.Bold);

        CreateText(
            panel.transform,
            "SUBTITLE",
            "SECURE OPERATIONS QUEUE",
            24,
            new Vector2(0f, 195f),
            new Vector2(800f, 45f),
            TextAnchor.MiddleCenter,
            FontStyle.Normal);

        CreateText(
            panel.transform,
            "MODE",
            "OPERATION TYPE",
            28,
            new Vector2(-260f, 120f),
            new Vector2(420f, 55f),
            TextAnchor.MiddleLeft,
            FontStyle.Bold);

        CreateText(
            panel.transform,
            "MODE_VALUE",
            "RANKED OPERATIONS",
            27,
            new Vector2(-260f, 65f),
            new Vector2(420f, 55f),
            TextAnchor.MiddleLeft,
            FontStyle.Normal);

        CreateText(
            panel.transform,
            "OPERATION",
            "OPERATION",
            28,
            new Vector2(-260f, 0f),
            new Vector2(420f, 55f),
            TextAnchor.MiddleLeft,
            FontStyle.Bold);

        CreateText(
            panel.transform,
            "OPERATION_VALUE",
            "STANDARD // 4 COMMANDERS",
            27,
            new Vector2(-260f, -55f),
            new Vector2(420f, 55f),
            TextAnchor.MiddleLeft,
            FontStyle.Normal);

        CreateText(
            panel.transform,
            "PLAYERS",
            "COMMANDERS",
            28,
            new Vector2(260f, 120f),
            new Vector2(420f, 55f),
            TextAnchor.MiddleLeft,
            FontStyle.Bold);

        CreateText(
            panel.transform,
            "PLAYER_VALUE",
            "4 / 8",
            30,
            new Vector2(260f, 65f),
            new Vector2(420f, 55f),
            TextAnchor.MiddleLeft,
            FontStyle.Bold);

        CreateText(
            panel.transform,
            "QUEUE",
            "QUEUE STATUS",
            28,
            new Vector2(260f, 0f),
            new Vector2(420f, 55f),
            TextAnchor.MiddleLeft,
            FontStyle.Bold);

        CreateText(
            panel.transform,
            "QUEUE_VALUE",
            "READY",
            30,
            new Vector2(260f, -55f),
            new Vector2(420f, 55f),
            TextAnchor.MiddleLeft,
            FontStyle.Bold);

        CreateButton(
            panel.transform,
            "JOIN_QUEUE_BUTTON",
            "JOIN QUEUE",
            new Vector2(0f, -175f),
            new Vector2(360f, 70f));

        CreateButton(
            panel.transform,
            "CLOSE_BUTTON",
            "CLOSE",
            new Vector2(0f, -260f),
            new Vector2(260f, 55f));

        panel.SetActive(false);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[MULTIPLAYER] MATCHMAKING UI BUILT.");
    }

    private static GameObject CreateText(
        Transform parent,
        string name,
        string value,
        int size,
        Vector2 position,
        Vector2 dimensions,
        TextAnchor alignment,
        FontStyle style)
    {
        GameObject obj =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Text));

        obj.transform.SetParent(parent, false);

        RectTransform rect =
            obj.GetComponent<RectTransform>();

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = dimensions;

        Text text =
            obj.GetComponent<Text>();

        text.text = value;
        text.fontSize = size;
        text.fontStyle = style;
        text.alignment = alignment;
        text.color = Color.white;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Overflow;

        return obj;
    }

    private static GameObject CreateButton(
        Transform parent,
        string name,
        string label,
        Vector2 position,
        Vector2 dimensions)
    {
        GameObject button =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image),
                typeof(Button));

        button.transform.SetParent(parent, false);

        RectTransform rect =
            button.GetComponent<RectTransform>();

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = dimensions;

        Image image =
            button.GetComponent<Image>();

        image.color =
            new Color(0.06f, 0.09f, 0.12f, 1f);

        CreateText(
            button.transform,
            "LABEL",
            label,
            28,
            Vector2.zero,
            dimensions,
            TextAnchor.MiddleCenter,
            FontStyle.Bold);

        return button;
    }
}
