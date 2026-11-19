using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public static class OPAWSettingsAAABuilder
{
    private const string ScenePath =
        "Assets/Scenes/SCN14_SETTINGS/[HUD] SETTINGS HUD/Settings.unity";

    private const string FrameName = "__OPAW_SETTINGS_BOX_FRAME";

    private static readonly Color PanelColor =
        new Color(0.012f, 0.030f, 0.045f, 0.99f);

    private static readonly Color NavigationColor =
        new Color(0.025f, 0.070f, 0.105f, 0.99f);

    private static readonly Color ContentColor =
        new Color(0.018f, 0.043f, 0.064f, 0.99f);

    private static readonly Color CardColor =
        new Color(0.032f, 0.075f, 0.105f, 0.99f);

    private static readonly Color ButtonColor =
        new Color(0.045f, 0.105f, 0.145f, 0.99f);

    private static readonly Color TextColor =
        new Color(0.88f, 0.93f, 0.96f, 1f);

    private static readonly Color SecondaryTextColor =
        new Color(0.58f, 0.70f, 0.76f, 1f);

    [MenuItem("Obsidian Protocol/Settings/REBUILD COMMAND CENTER STYLE")]
    public static void Build()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (!scene.IsValid() || scene.path != ScenePath)
            scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

        if (!scene.IsValid())
        {
            Debug.LogError("Settings scene could not be opened: " + ScenePath);
            return;
        }

        Undo.IncrementCurrentGroup();
        Undo.SetCurrentGroupName("Rebuild Settings Command Center UI");

        List<GameObject> objects = GetSceneObjects(scene);

        RemovePreviousFrames(objects);

        int panelsStyled = 0;
        int buttonsStyled = 0;

        // Actual Settings page surfaces. These are box backgrounds,
        // not text objects and not text labels.
        foreach (GameObject go in objects)
        {
            if (go == null || IsTextObject(go))
                continue;

            string n = go.name.ToUpperInvariant();

            if (!IsSettingsSurface(n))
                continue;

            if (go.GetComponent<UnityEngine.UI.Selectable>() != null)
                continue;

            RectTransform rect = go.GetComponent<RectTransform>();
            if (rect == null)
                continue;

            Image image = go.GetComponent<Image>();

            if (image == null)
                image = Undo.AddComponent<Image>(go);

            image.color = SurfaceColor(n);
            image.raycastTarget = false;

            AddFrame(go, 1.45f, 15f,
                new Color(0.19f, 0.66f, 0.81f, 0.62f));

            panelsStyled++;
        }

        // Style real interactive button boxes only.
        foreach (GameObject go in objects)
        {
            if (go == null || IsTextObject(go))
                continue;

            UnityEngine.UI.Selectable selectable =
                go.GetComponent<UnityEngine.UI.Selectable>();

            if (selectable == null)
                continue;

            bool supported =
                selectable is Button ||
                selectable is Dropdown ||
                selectable is TMP_Dropdown;

            if (!supported)
                continue;

            RectTransform rect = go.GetComponent<RectTransform>();
            if (rect == null)
                continue;

            float width = rect.rect.width;
            float height = rect.rect.height;

            if (width < 85f || height < 28f)
                continue;

            Image background = selectable.targetGraphic as Image;

            if (background == null)
                background = go.GetComponent<Image>();

            if (background == null && selectable.targetGraphic == null)
            {
                background = Undo.AddComponent<Image>(go);
                selectable.targetGraphic = background;
            }

            if (background != null)
                background.color = ButtonColor;

            OPAWSettingsBoxFrameGraphic frame =
                AddFrame(go, 1.35f, 9f,
                    new Color(0.20f, 0.70f, 0.85f, 0.72f));

            OPAWSettingsBoxHighlight highlight =
                go.GetComponent<OPAWSettingsBoxHighlight>();

            if (highlight == null)
                highlight = Undo.AddComponent<OPAWSettingsBoxHighlight>(go);

            highlight.Bind(frame, background);

            ColorBlock colors = selectable.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = new Color(0.78f, 0.91f, 0.98f, 1f);
            colors.pressedColor = new Color(0.58f, 0.80f, 0.90f, 1f);
            colors.selectedColor = new Color(0.84f, 0.95f, 1f, 1f);
            colors.disabledColor = new Color(0.42f, 0.48f, 0.52f, 0.55f);
            colors.colorMultiplier = 1f;
            colors.fadeDuration = 0.12f;
            selectable.colors = colors;

            buttonsStyled++;
        }

        // Keep typography clean and readable. No text outlines or text frames.
        foreach (GameObject go in objects)
        {
            if (go == null)
                continue;

            TMP_Text tmp = go.GetComponent<TMP_Text>();
            if (tmp == null)
                continue;

            string n = go.name.ToUpperInvariant();

            if (n.Contains("SUBTITLE") ||
                n.Contains("DESCRIPTION") ||
                n.Contains("HINT") ||
                n.Contains("CAPTION"))
            {
                tmp.color = SecondaryTextColor;
            }
            else
            {
                tmp.color = TextColor;
            }
        }

        EditorSceneManager.MarkSceneDirty(scene);

        if (EditorSceneManager.SaveScene(scene))
        {
            Debug.Log(
                "SETTINGS COMMAND CENTER REBUILD COMPLETE. " +
                "Panel boxes: " + panelsStyled +
                " | Interactive boxes: " + buttonsStyled +
                " | Old text-attached frames removed."
            );
        }
        else
        {
            Debug.LogError("Settings scene could not be saved.");
        }
    }

    private static List<GameObject> GetSceneObjects(Scene scene)
    {
        List<GameObject> result = new List<GameObject>();

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            Transform[] transforms =
                root.GetComponentsInChildren<Transform>(true);

            foreach (Transform t in transforms)
            {
                if (t != null && !result.Contains(t.gameObject))
                    result.Add(t.gameObject);
            }
        }

        return result;
    }

    private static void RemovePreviousFrames(List<GameObject> objects)
    {
        foreach (GameObject go in objects)
        {
            if (go == null)
                continue;

            for (int i = go.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = go.transform.GetChild(i);

                if (child != null && child.name == FrameName)
                    Undo.DestroyObjectImmediate(child.gameObject);
            }

            Component[] components = go.GetComponents<Component>();

            foreach (Component component in components)
            {
                if (component == null)
                    continue;

                string typeName = component.GetType().Name;

                if (typeName == "OPAWNeonFrame" ||
                    typeName == "OPAWSettingsBoxHighlight")
                {
                    Undo.DestroyObjectImmediate(component);
                }
            }
        }
    }

    private static bool IsTextObject(GameObject go)
    {
        if (go.GetComponent<TMP_Text>() != null)
            return true;

        if (go.GetComponent<Text>() != null)
            return true;

        string n = go.name.ToUpperInvariant();

        return n.Contains("TEXT") ||
               n.Contains("LABEL") ||
               n.Contains("TITLE") ||
               n.Contains("VALUE") ||
               n.Contains("SUBTITLE") ||
               n.Contains("DESCRIPTION") ||
               n.Contains("GLYPH") ||
               n.Contains("ICON") ||
               n.Contains("DIVIDER") ||
               n.Contains("RULE");
    }

    private static bool IsSettingsSurface(string n)
    {
        string[] exact =
        {
            "SETTINGS_NAVIGATION",
            "CONTENT_SURFACE",
            "HEADER_SURFACE",
            "SETTINGS_CARD",
            "GENERAL_SETTINGS",
            "DISPLAY_SETTINGS",
            "AUDIO_SETTINGS",
            "CONTROL_SETTINGS",
            "INTERFACE_SETTINGS",
            "ACCESSIBILITY_SETTINGS",
            "HUD_PREVIEW",
            "AUDIO",
            "ACCESSIBILITY"
        };

        foreach (string name in exact)
        {
            if (n == name)
                return true;
        }

        bool namedAsSurface =
            n.Contains("PANEL") ||
            n.Contains("SURFACE") ||
            n.Contains("CARD") ||
            n.StartsWith("OPTION_");

        if (!namedAsSurface)
            return false;

        return true;
    }

    private static Color SurfaceColor(string n)
    {
        if (n.Contains("NAVIGATION"))
            return NavigationColor;

        if (n.Contains("CONTENT"))
            return ContentColor;

        if (n.Contains("CARD"))
            return CardColor;

        if (n.Contains("HEADER"))
            return new Color(0.025f, 0.060f, 0.085f, 0.99f);

        if (n.StartsWith("OPTION_"))
            return ButtonColor;

        return PanelColor;
    }

    private static OPAWSettingsBoxFrameGraphic AddFrame(
        GameObject target,
        float thickness,
        float cornerCut,
        Color frameColor)
    {
        Transform existing = target.transform.Find(FrameName);

        if (existing != null)
            Undo.DestroyObjectImmediate(existing.gameObject);

        GameObject frameObject = new GameObject(
            FrameName,
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(OPAWSettingsBoxFrameGraphic)
        );

        Undo.RegisterCreatedObjectUndo(frameObject, "Create Settings Box Frame");

        RectTransform frameRect =
            frameObject.GetComponent<RectTransform>();

        frameRect.SetParent(target.transform, false);
        frameRect.anchorMin = Vector2.zero;
        frameRect.anchorMax = Vector2.one;
        frameRect.offsetMin = Vector2.zero;
        frameRect.offsetMax = Vector2.zero;
        frameRect.localScale = Vector3.one;
        frameRect.SetAsLastSibling();

        OPAWSettingsBoxFrameGraphic graphic =
            frameObject.GetComponent<OPAWSettingsBoxFrameGraphic>();

        graphic.thickness = thickness;
        graphic.cornerCut = cornerCut;
        graphic.color = frameColor;
        graphic.raycastTarget = false;

        graphic.SetAllDirty();

        return graphic;
    }
}
