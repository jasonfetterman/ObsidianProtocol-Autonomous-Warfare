using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public static class OPAWVRCommandHUDBuilder
{
    private static readonly Color Background = new Color(0.012f, 0.025f, 0.055f, 0.98f);
    private static readonly Color Panel = new Color(0.025f, 0.055f, 0.095f, 0.98f);
    private static readonly Color Cyan = new Color(0.0f, 0.82f, 1.0f, 1f);
    private static readonly Color White = new Color(0.86f, 0.94f, 1f, 1f);
    private static readonly Color Muted = new Color(0.38f, 0.58f, 0.70f, 1f);

    [MenuItem("OPAW/VR Command/BUILD NEW COMMAND HUD")]
    public static void Build()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (!scene.IsValid() || !scene.isLoaded)
        {
            Debug.LogError("Open SCN20_VR_COMMAND before building the new HUD.");
            return;
        }

        Camera cam = null;
        GameObject cameraObject = GameObject.Find("VR COMMAND CAMERA");
        if (cameraObject != null) cam = cameraObject.GetComponent<Camera>();
        if (cam == null) cam = Camera.main;

        if (cam == null)
        {
            Debug.LogError("VR COMMAND CAMERA was not found. No scene changes were made.");
            return;
        }

        EditorSceneManager.SaveScene(scene);
        string backup = scene.path + ".before_new_command_hud";
        if (!File.Exists(backup)) File.Copy(scene.path, backup);

        foreach (GameObject obj in Object.FindObjectsOfType<GameObject>(true))
        {
            if (obj == null || !obj.scene.IsValid() || obj.scene != scene) continue;
            if (obj.name.StartsWith("[HUD]") || obj.name.StartsWith("[SCREEN]"))
                obj.SetActive(false);
        }

        Transform old = cam.transform.Find("OPAW_NEW_VR_COMMAND_HUD");
        if (old != null) Object.DestroyImmediate(old.gameObject);

        GameObject root = new GameObject("OPAW_NEW_VR_COMMAND_HUD", typeof(RectTransform), typeof(Canvas), typeof(GraphicRaycaster));
        SceneManager.MoveGameObjectToScene(root, scene);
        root.transform.SetParent(cam.transform, false);
        root.transform.localPosition = new Vector3(0f, -0.04f, 1.65f);
        root.transform.localRotation = Quaternion.identity;
        root.transform.localScale = Vector3.one * 0.001f;

        Canvas canvas = root.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = cam;
        canvas.sortingOrder = 300;
        RectTransform canvasRect = root.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(1600, 900);

        if (Object.FindObjectOfType<EventSystem>() == null)
        {
            GameObject events = new GameObject("OPAW VR HUD EVENT SYSTEM", typeof(EventSystem), typeof(StandaloneInputModule));
            SceneManager.MoveGameObjectToScene(events, scene);
        }

        RectTransform frame = Rect(root.transform, "MAIN FRAME", Vector2.zero, new Vector2(1560, 860));
        Image frameImage = frame.gameObject.AddComponent<Image>();
        frameImage.color = Background;

        RectTransform top = Rect(frame, "TOP COMMAND HEADER", new Vector2(0, 365), new Vector2(1510, 100));
        Image topImage = top.gameObject.AddComponent<Image>();
        topImage.color = Panel;
        AddText(top, "OP // AUTONOMOUS WARFARE", new Vector2(-390, 12), new Vector2(650, 55), 34, Cyan, FontStyles.Bold, TextAlignmentOptions.Left);
        AddText(top, "VIRTUAL COMMAND FACILITY  /  LIVE OPERATIONS", new Vector2(390, 12), new Vector2(650, 45), 21, White, FontStyles.Bold, TextAlignmentOptions.Right);
        AddText(top, "COMMAND NETWORK  ●  SYSTEMS ONLINE", new Vector2(0, -30), new Vector2(700, 28), 17, Muted, FontStyles.Normal, TextAlignmentOptions.Center);

        RectTransform nav = Rect(frame, "COMMAND NAVIGATION", new Vector2(-585, -5), new Vector2(270, 630));
        nav.gameObject.AddComponent<Image>().color = Panel;
        AddText(nav, "COMMAND MENU", new Vector2(0, 275), new Vector2(230, 45), 22, Cyan, FontStyles.Bold, TextAlignmentOptions.Center);

        RectTransform content = Rect(frame, "COMMAND CONTENT", new Vector2(150, -5), new Vector2(1160, 630));
        content.gameObject.AddComponent<Image>().color = Panel;
        AddText(content, "OPERATIONS OVERVIEW", new Vector2(0, 265), new Vector2(1050, 60), 32, Cyan, FontStyles.Bold, TextAlignmentOptions.Left);
        TMP_Text details = AddText(content,
            "VR COMMAND FACILITY\n\nSelect a command system from the navigation panel.\n\nThe physical command environment remains active.\nAll operational systems are managed through their existing scene components.",
            new Vector2(0, 15), new Vector2(1040, 390), 25, White, FontStyles.Normal, TextAlignmentOptions.TopLeft);

        string[] tabs = { "OVERVIEW", "FLEET COMMAND", "BATTLEFIELD", "INTELLIGENCE", "DEPLOYMENT", "UNIT INSPECTION", "SYSTEM STATUS" };
        string[] descriptions =
        {
            "VR COMMAND FACILITY\n\nCommand network overview\n\nFacility status: ONLINE\nVR environment: ACTIVE\nCommand interface: READY",
            "FLEET COMMAND\n\nFleet management interface\n\nSelect a physical command station to continue fleet operations.",
            "BATTLEFIELD COMMAND\n\nTactical operations interface\n\nBattlefield displays and tactical systems remain available in the facility.",
            "INTELLIGENCE SYSTEMS\n\nIntelligence operations interface\n\nMonitor operational intelligence and network information.",
            "DEPLOYMENT SYSTEMS\n\nDeployment command interface\n\nCoordinate unit deployment through the existing command systems.",
            "UNIT INSPECTION\n\nUnit status and inspection interface\n\nSelect a unit through the existing inspection station.",
            "SYSTEM STATUS\n\nVR command facility: ONLINE\nCommand display: ACTIVE\nLegacy HUD overlays: DISABLED\nPhysical VR environment: PRESERVED"
        };

        for (int i = 0; i < tabs.Length; i++)
        {
            int index = i;
            Button button = MakeButton(nav, tabs[i], new Vector2(0, 215 - i * 72), new Vector2(230, 58));
            button.onClick.AddListener(() =>
            {
                details.text = descriptions[index];
                Transform heading = content.Find("ACTIVE SECTION TITLE");
                if (heading != null)
                {
                    TMP_Text t = heading.GetComponent<TMP_Text>();
                    if (t != null) t.text = tabs[index];
                }
            });
        }

        AddText(content, "ACTIVE SECTION TITLE", new Vector2(0, 265), new Vector2(1050, 60), 32, Cyan, FontStyles.Bold, TextAlignmentOptions.Left);
        RectTransform bottom = Rect(frame, "BOTTOM STATUS BAR", new Vector2(0, -385), new Vector2(1510, 48));
        bottom.gameObject.AddComponent<Image>().color = Panel;
        AddText(bottom, "OPAW  //  VR COMMAND", new Vector2(-520, 0), new Vector2(400, 35), 17, Cyan, FontStyles.Bold, TextAlignmentOptions.Left);
        AddText(bottom, "AUTONOMOUS WARFARE NETWORK  //  COMMAND INTERFACE", new Vector2(250, 0), new Vector2(800, 35), 17, Muted, FontStyles.Normal, TextAlignmentOptions.Right);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Selection.activeGameObject = root;
        Debug.Log("NEW VR COMMAND HUD BUILT. Legacy HUD roots disabled. Physical facility and VR systems preserved. Backup: " + backup);
    }

    private static RectTransform Rect(Transform parent, string name, Vector2 position, Vector2 size)
    {
        GameObject go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
        return rect;
    }

    private static TMP_Text AddText(Transform parent, string value, Vector2 position, Vector2 size, float fontSize, Color color, FontStyles style, TextAlignmentOptions alignment)
    {
        RectTransform rect = Rect(parent, value == "ACTIVE SECTION TITLE" ? value : "TEXT " + parent.childCount, position, size);
        TMP_Text text = rect.gameObject.AddComponent<TextMeshProUGUI>();
        text.text = value;
        text.fontSize = fontSize;
        text.color = color;
        text.fontStyle = style;
        text.alignment = alignment;
        text.textWrappingMode = TextWrappingModes.Normal;
        text.raycastTarget = false;
        if (TMP_Settings.defaultFontAsset != null) text.font = TMP_Settings.defaultFontAsset;
        return text;
    }

    private static Button MakeButton(Transform parent, string label, Vector2 position, Vector2 size)
    {
        RectTransform rect = Rect(parent, "NAV " + label, position, size);
        Image image = rect.gameObject.AddComponent<Image>();
        image.color = new Color(0.035f, 0.09f, 0.14f, 1f);
        Button button = rect.gameObject.AddComponent<Button>();
        button.targetGraphic = image;
        ColorBlock colors = button.colors;
        colors.normalColor = new Color(0.035f, 0.09f, 0.14f, 1f);
        colors.highlightedColor = new Color(0.02f, 0.28f, 0.38f, 1f);
        colors.pressedColor = Cyan;
        colors.selectedColor = new Color(0.02f, 0.20f, 0.29f, 1f);
        button.colors = colors;
        AddText(rect, label, Vector2.zero, size - new Vector2(18, 8), 19, White, FontStyles.Bold, TextAlignmentOptions.Center);
        return button;
    }
}
