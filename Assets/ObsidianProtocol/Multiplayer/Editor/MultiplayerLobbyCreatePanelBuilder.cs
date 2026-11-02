using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class MultiplayerLobbyCreatePanelBuilder
{
    [MenuItem("Obsidian Protocol/Multiplayer/BUILD CREATE MATCH LOBBY PANEL")]
    public static void Build()
    {
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("[MULTIPLAYER] Canvas not found.");
            return;
        }

        Transform existing = canvas.transform.Find("CREATE_MATCH_LOBBY_PANEL");

        if (existing != null)
            Object.DestroyImmediate(existing.gameObject);

        GameObject panel = new GameObject(
            "CREATE_MATCH_LOBBY_PANEL",
            typeof(RectTransform),
            typeof(Image)
        );

        panel.transform.SetParent(canvas.transform, false);

        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(900f, 650f);
        rect.anchoredPosition = Vector2.zero;

        Image background = panel.GetComponent<Image>();
        background.color = new Color(0.025f, 0.035f, 0.05f, 0.98f);

        CreateText(
            panel.transform,
            "TITLE",
            "CREATE MATCH LOBBY",
            new Vector2(0f, 250f),
            new Vector2(760f, 70f),
            32,
            TextAnchor.MiddleCenter
        );

        CreateText(
            panel.transform,
            "SUBTITLE",
            "PRIVATE COMMANDER GROUP",
            new Vector2(0f, 195f),
            new Vector2(760f, 45f),
            18,
            TextAnchor.MiddleCenter
        );

        CreateText(
            panel.transform,
            "LOBBY_NAME",
            "LOBBY NAME",
            new Vector2(-250f, 105f),
            new Vector2(300f, 45f),
            20,
            TextAnchor.MiddleLeft
        );

        CreateText(
            panel.transform,
            "LOBBY_NAME_VALUE",
            "COMMANDER-01'S LOBBY",
            new Vector2(100f, 105f),
            new Vector2(420f, 45f),
            18,
            TextAnchor.MiddleLeft
        );

        CreateText(
            panel.transform,
            "PRIVACY",
            "PRIVACY",
            new Vector2(-250f, 35f),
            new Vector2(300f, 45f),
            20,
            TextAnchor.MiddleLeft
        );

        CreateText(
            panel.transform,
            "PRIVACY_VALUE",
            "INVITE ONLY",
            new Vector2(100f, 35f),
            new Vector2(420f, 45f),
            18,
            TextAnchor.MiddleLeft
        );

        CreateText(
            panel.transform,
            "OPERATION",
            "OPERATION",
            new Vector2(-250f, -35f),
            new Vector2(300f, 45f),
            20,
            TextAnchor.MiddleLeft
        );

        CreateText(
            panel.transform,
            "OPERATION_VALUE",
            "STANDARD OPERATION",
            new Vector2(100f, -35f),
            new Vector2(420f, 45f),
            18,
            TextAnchor.MiddleLeft
        );

        CreateText(
            panel.transform,
            "CAPACITY",
            "PLAYER CAPACITY",
            new Vector2(-250f, -105f),
            new Vector2(300f, 45f),
            20,
            TextAnchor.MiddleLeft
        );

        CreateText(
            panel.transform,
            "CAPACITY_VALUE",
            "8 COMMANDERS",
            new Vector2(100f, -105f),
            new Vector2(420f, 45f),
            18,
            TextAnchor.MiddleLeft
        );

        CreateButton(
            panel.transform,
            "CREATE_BUTTON",
            "CREATE",
            new Vector2(-130f, -225f),
            new Vector2(240f, 65f)
        );

        CreateButton(
            panel.transform,
            "CLOSE_BUTTON",
            "CANCEL",
            new Vector2(150f, -225f),
            new Vector2(240f, 65f)
        );

        panel.SetActive(false);

        EditorUtility.SetDirty(canvas.gameObject);

        Debug.Log("[MULTIPLAYER] CREATE MATCH LOBBY PANEL BUILT.");
    }

    private static Text CreateText(
        Transform parent,
        string name,
        string value,
        Vector2 position,
        Vector2 size,
        int fontSize,
        TextAnchor alignment)
    {
        GameObject obj = new GameObject(
            name,
            typeof(RectTransform),
            typeof(Text)
        );

        obj.transform.SetParent(parent, false);

        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = size;
        rect.anchoredPosition = position;

        Text text = obj.GetComponent<Text>();
        text.text = value;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = fontSize;
        text.alignment = alignment;
        text.color = Color.white;
        text.raycastTarget = false;

        return text;
    }

    private static Button CreateButton(
        Transform parent,
        string name,
        string label,
        Vector2 position,
        Vector2 size)
    {
        GameObject obj = new GameObject(
            name,
            typeof(RectTransform),
            typeof(Image),
            typeof(Button)
        );

        obj.transform.SetParent(parent, false);

        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = size;
        rect.anchoredPosition = position;

        Image image = obj.GetComponent<Image>();
        image.color = new Color(0.08f, 0.12f, 0.18f, 1f);

        Button button = obj.GetComponent<Button>();
        button.targetGraphic = image;

        CreateText(
            obj.transform,
            "LABEL",
            label,
            Vector2.zero,
            size,
            20,
            TextAnchor.MiddleCenter
        );

        return button;
    }
}
