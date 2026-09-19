using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class MainMenuPopupWiringBuilder
{
    private const string ScenePath =
        "Assets/Scenes/SCN-01  MAIN MENU/[HUD] MAIN MENU HUD/SCN-01  MAIN MENU.unity";

    [MenuItem("Obsidian Protocol/Build/MAIN MENU - WIRE POPUPS")]
    public static void Build()
    {
        if (!System.IO.File.Exists(ScenePath))
        {
            Debug.LogError(
                "MAIN MENU POPUP WIRING: Scene not found: " +
                ScenePath);
            return;
        }

        var scene = EditorSceneManager.OpenScene(
            ScenePath,
            OpenSceneMode.Single);

        if (!scene.IsValid())
        {
            Debug.LogError(
                "MAIN MENU POPUP WIRING: Could not open scene.");
            return;
        }

        GameObject popupRoot =
            GameObject.Find("MAIN MENU POPUPS");

        if (popupRoot == null)
        {
            Debug.LogError(
                "MAIN MENU POPUP WIRING: MAIN MENU POPUPS does not exist. " +
                "Build the popup system first.");
            return;
        }

        GameObject patchNotes =
            GameObject.Find("MAIN MENU POPUPS/POPUP_PATCH_NOTES");

        GameObject networkError =
            GameObject.Find("MAIN MENU POPUPS/POPUP_NETWORK_ERROR");

        GameObject profileLogin =
            GameObject.Find("MAIN MENU POPUPS/POPUP_PROFILE_LOGIN");

        if (patchNotes == null ||
            networkError == null ||
            profileLogin == null)
        {
            Debug.LogError(
                "MAIN MENU POPUP WIRING: One or more popup objects are missing.");
            return;
        }

        WireButton(
            "BUTTON_PATCH_NOTES",
            patchNotes);

        WireButton(
            "BUTTON_NETWORK_ERROR",
            networkError);

        WireButton(
            "BUTTON_PROFILE_LOGIN",
            profileLogin);

        EnsureMainMenuPopupButtons();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "============================================================");

        Debug.Log(
            "OBSIDIAN PROTOCOL POPUP WIRING COMPLETE");

        Debug.Log(
            "PATCH NOTES BUTTON -> POPUP_PATCH_NOTES");

        Debug.Log(
            "NETWORK BUTTON -> POPUP_NETWORK_ERROR");

        Debug.Log(
            "PROFILE BUTTON -> POPUP_PROFILE_LOGIN");

        Debug.Log(
            "============================================================");
    }

    private static void WireButton(
        string buttonName,
        GameObject popup)
    {
        GameObject buttonObject =
            GameObject.Find(buttonName);

        if (buttonObject == null)
        {
            Debug.LogWarning(
                "Popup button not found: " +
                buttonName);
            return;
        }

        Button button =
            buttonObject.GetComponent<Button>();

        if (button == null)
        {
            Debug.LogWarning(
                buttonName +
                " exists but has no Button component.");
            return;
        }

        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(
            () =>
            {
                popup.SetActive(true);
            });

        Debug.Log(
            "Wired " +
            buttonName +
            " -> " +
            popup.name);
    }

    private static void EnsureMainMenuPopupButtons()
    {
        GameObject canvas =
            FindBestCanvas();

        if (canvas == null)
        {
            Debug.LogWarning(
                "No Canvas found. Popup buttons were not created.");
            return;
        }

        Transform parent =
            FindMenuParent(canvas.transform);

        if (parent == null)
        {
            parent = canvas.transform;
        }

        CreateButtonIfMissing(
            parent,
            "BUTTON_PATCH_NOTES",
            "PATCH NOTES",
            new Vector2(0, -330));

        CreateButtonIfMissing(
            parent,
            "BUTTON_NETWORK_ERROR",
            "NETWORK TEST",
            new Vector2(0, -395));

        CreateButtonIfMissing(
            parent,
            "BUTTON_PROFILE_LOGIN",
            "COMMANDER LOGIN",
            new Vector2(0, -460));
    }

    private static GameObject FindBestCanvas()
    {
        GameObject existing =
            GameObject.Find("Canvas_MainMenuHUD");

        if (existing != null)
        {
            return existing;
        }

        Canvas canvas =
            Object.FindFirstObjectByType<Canvas>();

        return canvas != null
            ? canvas.gameObject
            : null;
    }

    private static Transform FindMenuParent(
        Transform root)
    {
        string[] possibleNames =
        {
            "PANEL_CenterMenu",
            "Center Menu",
            "CENTER MENU",
            "MAIN MENU",
            "MENU"
        };

        foreach (string name in possibleNames)
        {
            Transform found =
                FindChildRecursive(root, name);

            if (found != null)
            {
                return found;
            }
        }

        return null;
    }

    private static Transform FindChildRecursive(
        Transform parent,
        string target)
    {
        if (parent.name == target)
        {
            return parent;
        }

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform result =
                FindChildRecursive(
                    parent.GetChild(i),
                    target);

            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    private static void CreateButtonIfMissing(
        Transform parent,
        string name,
        string label,
        Vector2 position)
    {
        GameObject existing =
            GameObject.Find(name);

        if (existing != null)
        {
            return;
        }

        GameObject buttonObject =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image),
                typeof(Button));

        buttonObject.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            buttonObject.GetComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0.5f, 0.5f);

        rect.anchorMax =
            new Vector2(0.5f, 0.5f);

        rect.pivot =
            new Vector2(0.5f, 0.5f);

        rect.sizeDelta =
            new Vector2(300, 50);

        rect.anchoredPosition =
            position;

        Image image =
            buttonObject.GetComponent<Image>();

        image.color =
            new Color(
                0.04f,
                0.07f,
                0.09f,
                0.96f);

        CreateLabel(
            buttonObject.transform,
            label);
    }

    private static void CreateLabel(
        Transform parent,
        string label)
    {
        GameObject textObject =
            new GameObject(
                "LABEL",
                typeof(RectTransform),
                typeof(Text));

        textObject.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            textObject.GetComponent<RectTransform>();

        rect.anchorMin =
            Vector2.zero;

        rect.anchorMax =
            Vector2.one;

        rect.offsetMin =
            Vector2.zero;

        rect.offsetMax =
            Vector2.zero;

        Text text =
            textObject.GetComponent<Text>();

        text.text =
            label;

        text.font =
            Resources.GetBuiltinResource<Font>(
                "LegacyRuntime.ttf");

        text.fontSize =
            18;

        text.fontStyle =
            FontStyle.Bold;

        text.alignment =
            TextAnchor.MiddleCenter;

        text.color =
            Color.white;
    }
}