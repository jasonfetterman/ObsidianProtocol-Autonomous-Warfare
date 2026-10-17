using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public static class CommandCenterInteractionBuilder
{
    private static readonly string[] Commands =
    {
        "COMMAND",
        "OPERATIONS",
        "FLEET",
        "DEPLOY",
        "LOGISTICS",
        "RESEARCH",
        "GARAGE",
        "FINANCE",
        "INTELLIGENCE",
        "STORE"
    };

    [MenuItem("Obsidian Protocol/Command Center/BUILD REAL INTERACTION PANELS")]
    public static void Build()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name != "Command_Center")
        {
            Debug.LogError("[COMMAND CENTER] Open Command_Center first.");
            return;
        }

        Canvas canvas = Object.FindFirstObjectByType<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("[COMMAND CENTER] Canvas not found.");
            return;
        }

        Transform oldRoot = canvas.transform.Find("COMMAND_CENTER_REAL_UI");

        if (oldRoot != null)
            Object.DestroyImmediate(oldRoot.gameObject);

        GameObject root = new GameObject(
            "COMMAND_CENTER_REAL_UI",
            typeof(RectTransform),
            typeof(CommandCenterRuntime)
        );

        root.transform.SetParent(canvas.transform, false);

        RectTransform rootRect = root.GetComponent<RectTransform>();
        rootRect.anchorMin = Vector2.zero;
        rootRect.anchorMax = Vector2.one;
        rootRect.offsetMin = Vector2.zero;
        rootRect.offsetMax = Vector2.zero;

        GameObject nav = CreateNav(root.transform);

        CommandCenterRuntime runtime =
            root.GetComponent<CommandCenterRuntime>();

        runtime.Setup();

        foreach (string command in Commands)
        {
            GameObject panel = CreatePanel(
                root.transform,
                command + "_REAL_PANEL",
                command
            );

            runtime.Register(command, panel);

            Button button = CreateButton(
                nav.transform,
                command + "_BUTTON",
                command
            );

            string captured = command;

            button.onClick.AddListener(() =>
            {
                runtime.Open(captured);
            });
        }

        runtime.CreateCloseConnections();

        EditorUtility.SetDirty(root);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[COMMAND CENTER] BUILT 10 GUARANTEED BUTTONS AND 10 PANELS."
        );
    }

    private static GameObject CreateNav(Transform parent)
    {
        GameObject nav = new GameObject(
            "COMMAND_CENTER_PRIMARY_NAV",
            typeof(RectTransform),
            typeof(Image)
        );

        nav.transform.SetParent(parent, false);

        RectTransform r = nav.GetComponent<RectTransform>();

        r.anchorMin = new Vector2(0.015f, 0.15f);
        r.anchorMax = new Vector2(0.20f, 0.85f);
        r.offsetMin = Vector2.zero;
        r.offsetMax = Vector2.zero;

        Image image = nav.GetComponent<Image>();
        image.color = new Color(0.015f, 0.025f, 0.04f, 0.96f);

        return nav;
    }

    private static GameObject CreatePanel(
        Transform parent,
        string objectName,
        string title
    )
    {
        GameObject panel = new GameObject(
            objectName,
            typeof(RectTransform),
            typeof(Image)
        );

        panel.transform.SetParent(parent, false);

        RectTransform r = panel.GetComponent<RectTransform>();

        r.anchorMin = new Vector2(0.22f, 0.10f);
        r.anchorMax = new Vector2(0.98f, 0.90f);
        r.offsetMin = Vector2.zero;
        r.offsetMax = Vector2.zero;

        Image image = panel.GetComponent<Image>();
        image.color = new Color(0.02f, 0.035f, 0.055f, 0.985f);
        image.raycastTarget = true;

        GameObject header = new GameObject(
            "TITLE",
            typeof(RectTransform),
            typeof(Text)
        );

        header.transform.SetParent(panel.transform, false);

        RectTransform hr = header.GetComponent<RectTransform>();

        hr.anchorMin = new Vector2(0.04f, 0.88f);
        hr.anchorMax = new Vector2(0.75f, 0.98f);
        hr.offsetMin = Vector2.zero;
        hr.offsetMax = Vector2.zero;

        Text titleText = header.GetComponent<Text>();
        titleText.text = title;
        titleText.fontSize = 30;
        titleText.fontStyle = FontStyle.Bold;
        titleText.alignment = TextAnchor.MiddleLeft;
        titleText.color = Color.white;

        GameObject body = new GameObject(
            "CONTENT",
            typeof(RectTransform),
            typeof(Text)
        );

        body.transform.SetParent(panel.transform, false);

        RectTransform br = body.GetComponent<RectTransform>();

        br.anchorMin = new Vector2(0.05f, 0.20f);
        br.anchorMax = new Vector2(0.95f, 0.84f);
        br.offsetMin = Vector2.zero;
        br.offsetMax = Vector2.zero;

        Text bodyText = body.GetComponent<Text>();

        bodyText.text =
            title +
            "\n\nCOMMAND CENTER SYSTEM\n\n" +
            "INTERFACE ONLINE\n\n" +
            "This panel is now connected to the Command Center interaction controller.";

        bodyText.fontSize = 20;
        bodyText.alignment = TextAnchor.UpperLeft;
        bodyText.color = new Color(0.78f, 0.84f, 0.92f);

        Button close = CreateButton(
            panel.transform,
            "CLOSE",
            "CLOSE"
        );

        RectTransform cr = close.GetComponent<RectTransform>();

        cr.anchorMin = new Vector2(0.78f, 0.04f);
        cr.anchorMax = new Vector2(0.94f, 0.14f);
        cr.offsetMin = Vector2.zero;
        cr.offsetMax = Vector2.zero;

        panel.SetActive(false);

        return panel;
    }

    private static Button CreateButton(
        Transform parent,
        string objectName,
        string label
    )
    {
        GameObject buttonObject = new GameObject(
            objectName,
            typeof(RectTransform),
            typeof(Image),
            typeof(Button)
        );

        buttonObject.transform.SetParent(parent, false);

        Button button = buttonObject.GetComponent<Button>();

        Image image = buttonObject.GetComponent<Image>();
        image.color = new Color(0.08f, 0.11f, 0.16f, 1f);

        Text text = new GameObject(
            "LABEL",
            typeof(RectTransform),
            typeof(Text)
        ).GetComponent<Text>();

        text.transform.SetParent(buttonObject.transform, false);

        RectTransform tr = text.GetComponent<RectTransform>();

        tr.anchorMin = Vector2.zero;
        tr.anchorMax = Vector2.one;
        tr.offsetMin = new Vector2(8, 2);
        tr.offsetMax = new Vector2(-8, -2);

        text.text = label;
        text.fontSize = 17;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;

        return button;
    }
}

public class CommandCenterRuntime : MonoBehaviour
{
    private readonly Dictionary<string, GameObject> panels =
        new Dictionary<string, GameObject>();

    private GameObject activePanel;

    public void Setup()
    {
        panels.Clear();
        activePanel = null;
    }

    public void Register(string key, GameObject panel)
    {
        panels[key] = panel;
        panel.SetActive(false);
    }

    public void Open(string key)
    {
        foreach (KeyValuePair<string, GameObject> item in panels)
        {
            if (item.Value != null)
                item.Value.SetActive(item.Key == key);
        }

        if (panels.TryGetValue(key, out GameObject selected))
            activePanel = selected;

        Debug.Log("[COMMAND CENTER] OPENED: " + key);
    }

    public void Close()
    {
        foreach (GameObject panel in panels.Values)
        {
            if (panel != null)
                panel.SetActive(false);
        }

        activePanel = null;

        Debug.Log("[COMMAND CENTER] PANEL CLOSED");
    }

    public void CreateCloseConnections()
    {
        foreach (GameObject panel in panels.Values)
        {
            if (panel == null)
                continue;

            Transform close =
                panel.transform.Find("CLOSE");

            if (close == null)
                continue;

            Button button =
                close.GetComponent<Button>();

            if (button == null)
                continue;

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(Close);
        }
    }
}

