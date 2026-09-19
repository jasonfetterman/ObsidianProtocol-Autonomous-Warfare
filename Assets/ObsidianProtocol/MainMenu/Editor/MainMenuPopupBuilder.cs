using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class MainMenuPopupBuilder
{
    private const string ScenePath =
        "Assets/Scenes/SCN-01  MAIN MENU/[HUD] MAIN MENU HUD/SCN-01  MAIN MENU.unity";

    private const string PopupRootName = "MAIN MENU POPUPS";

    [MenuItem("Obsidian Protocol/Build/MAIN MENU - POPUPS")]
    public static void Build()
    {
        if (!System.IO.File.Exists(ScenePath))
        {
            Debug.LogError("MAIN MENU POPUP BUILDER: Target scene does not exist: " + ScenePath);
            return;
        }

        Scene scene = EditorSceneManager.OpenScene(
            ScenePath,
            OpenSceneMode.Single);

        if (!scene.IsValid())
        {
            Debug.LogError("MAIN MENU POPUP BUILDER: Could not open target scene.");
            return;
        }

        RemoveOldPopupRoot();

        EnsureEventSystem();

        GameObject root = CreateCanvasRoot();

        CreatePatchNotes(root.transform);
        CreateNetworkError(root.transform);
        CreateProfileLogin(root.transform);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("============================================================");
        Debug.Log("OBSIDIAN PROTOCOL MAIN MENU POPUPS BUILT");
        Debug.Log("PATCH NOTES");
        Debug.Log("NETWORK ERROR");
        Debug.Log("PROFILE LOGIN");
        Debug.Log("Existing EXIT CONFIRMATION was preserved.");
        Debug.Log("Scene: " + ScenePath);
        Debug.Log("============================================================");
    }

    private static void RemoveOldPopupRoot()
    {
        GameObject old = GameObject.Find(PopupRootName);

        if (old != null)
        {
            Object.DestroyImmediate(old);
        }
    }

    private static void EnsureEventSystem()
    {
        EventSystem existing = Object.FindFirstObjectByType<EventSystem>();

        if (existing == null)
        {
            GameObject es = new GameObject(
                "EVENT SYSTEM",
                typeof(EventSystem),
                typeof(InputSystemUIInputModule));

            Undo.RegisterCreatedObjectUndo(es, "Create Event System");
        }
        else
        {
            StandaloneInputModule oldModule =
                existing.GetComponent<StandaloneInputModule>();

            if (oldModule != null)
            {
                Object.DestroyImmediate(oldModule);
            }

            if (existing.GetComponent<InputSystemUIInputModule>() == null)
            {
                existing.gameObject.AddComponent<InputSystemUIInputModule>();
            }
        }
    }

    private static GameObject CreateCanvasRoot()
    {
        GameObject root = new GameObject(
            PopupRootName,
            typeof(RectTransform),
            typeof(Canvas),
            typeof(CanvasScaler),
            typeof(GraphicRaycaster));

        Canvas canvas = root.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 500;

        CanvasScaler scaler = root.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        return root;
    }

    private static GameObject CreateWindow(
        Transform parent,
        string name,
        string title,
        string body)
    {
        GameObject panel = new GameObject(
            name,
            typeof(RectTransform),
            typeof(Image));

        panel.transform.SetParent(parent, false);

        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(760, 470);
        rect.anchoredPosition = Vector2.zero;

        Image image = panel.GetComponent<Image>();
        image.color = new Color(0.025f, 0.035f, 0.045f, 0.985f);

        CreateBorder(panel.transform);
        CreateTitle(panel.transform, title);
        CreateBody(panel.transform, body);

        return panel;
    }

    private static void CreateBorder(Transform parent)
    {
        GameObject border = new GameObject(
            "WINDOW BORDER",
            typeof(RectTransform),
            typeof(Image));

        border.transform.SetParent(parent, false);

        RectTransform rect = border.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = new Vector2(3, 3);
        rect.offsetMax = new Vector2(-3, -3);

        Image image = border.GetComponent<Image>();
        image.color = new Color(0.12f, 0.18f, 0.23f, 0.95f);

        border.transform.SetAsFirstSibling();
    }

    private static void CreateTitle(Transform parent, string title)
    {
        GameObject obj = new GameObject(
            "TITLE",
            typeof(RectTransform),
            typeof(Text));

        obj.transform.SetParent(parent, false);

        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 1);
        rect.anchorMax = new Vector2(0.5f, 1);
        rect.pivot = new Vector2(0.5f, 1);
        rect.sizeDelta = new Vector2(620, 65);
        rect.anchoredPosition = new Vector2(0, -35);

        Text text = obj.GetComponent<Text>();
        text.text = title;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 30;
        text.fontStyle = FontStyle.Bold;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
    }

    private static void CreateBody(Transform parent, string body)
    {
        GameObject obj = new GameObject(
            "BODY",
            typeof(RectTransform),
            typeof(Text));

        obj.transform.SetParent(parent, false);

        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(620, 220);
        rect.anchoredPosition = new Vector2(0, 15);

        Text text = obj.GetComponent<Text>();
        text.text = body;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 19;
        text.alignment = TextAnchor.UpperCenter;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        text.color = new Color(0.78f, 0.84f, 0.88f);
    }

    private static void CreateCloseButton(
        Transform parent,
        string label)
    {
        GameObject obj = new GameObject(
            "BUTTON_CLOSE",
            typeof(RectTransform),
            typeof(Image),
            typeof(Button));

        obj.transform.SetParent(parent, false);

        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0);
        rect.anchorMax = new Vector2(0.5f, 0);
        rect.pivot = new Vector2(0.5f, 0);
        rect.sizeDelta = new Vector2(210, 52);
        rect.anchoredPosition = new Vector2(0, 35);

        Image image = obj.GetComponent<Image>();
        image.color = new Color(0.08f, 0.11f, 0.14f, 1f);

        Button button = obj.GetComponent<Button>();

        GameObject textObj = new GameObject(
            "LABEL",
            typeof(RectTransform),
            typeof(Text));

        textObj.transform.SetParent(obj.transform, false);

        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        Text text = textObj.GetComponent<Text>();
        text.text = label;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 18;
        text.fontStyle = FontStyle.Bold;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;

        button.onClick.AddListener(() =>
        {
            parent.gameObject.SetActive(false);
        });
    }

    private static void CreatePatchNotes(Transform parent)
    {
        GameObject window = CreateWindow(
            parent.transform,
            "POPUP_PATCH_NOTES",
            "PATCH NOTES",
            "OBSIDIAN PROTOCOL\n\n" +
            "COMMAND SYSTEM UPDATE\n\n" +
            "• Main Menu command architecture updated\n" +
            "• Autonomous warfare systems integrated\n" +
            "• Garage / Fleet / Operations navigation expanded\n" +
            "• HUD and popup framework online\n\n" +
            "SYSTEM STATUS: OPERATIONAL");

        CreateCloseButton(window.transform, "CLOSE");
        window.SetActive(false);
    }

    private static void CreateNetworkError(Transform parent)
    {
        GameObject window = CreateWindow(
            parent.transform,
            "POPUP_NETWORK_ERROR",
            "NETWORK ERROR",
            "COMMUNICATION WITH COMMAND NETWORK FAILED.\n\n" +
            "The connection to the Obsidian Protocol service " +
            "could not be established.\n\n" +
            "Check network connectivity and attempt the operation again.");

        CreateCloseButton(window.transform, "ACKNOWLEDGE");
        window.SetActive(false);
    }

    private static void CreateProfileLogin(Transform parent)
    {
        GameObject window = CreateWindow(
            parent.transform,
            "POPUP_PROFILE_LOGIN",
            "COMMANDER PROFILE LOGIN",
            "COMMANDER AUTHENTICATION REQUIRED.\n\n" +
            "Profile services are not currently connected.\n\n" +
            "LOCAL COMMANDER PROFILE\n" +
            "STATUS: READY\n\n" +
            "Online authentication can be connected to the " +
            "multiplayer/profile backend later.");

        CreateCloseButton(window.transform, "CLOSE");
        window.SetActive(false);
    }
}