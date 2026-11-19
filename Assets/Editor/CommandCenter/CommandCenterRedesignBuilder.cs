using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public static class CommandCenterRedesignBuilder
{
    private const string ScenePath =
        "Assets/Scenes/SCN02_COMMAND_CENTER/COMMAND CENTER HUD/Command_Center.unity";

    private static readonly string[] PanelNames =
    {
        "COMMAND_REAL_PANEL",
        "OPERATIONS_REAL_PANEL",
        "FLEET_REAL_PANEL",
        "DEPLOY_REAL_PANEL",
        "LOGISTICS_REAL_PANEL",
        "RESEARCH_REAL_PANEL",
        "GARAGE_REAL_PANEL",
        "FINANCE_REAL_PANEL",
        "INTELLIGENCE_REAL_PANEL",
        "STORE_REAL_PANEL"
    };

    [MenuItem("Obsidian Protocol/Command Center/BUILD REDESIGNED COMMAND CENTER")]
    public static void Build()
    {
        if (EditorSceneManager.GetActiveScene().path != ScenePath)
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

        var canvasObject = GameObject.Find("COMMAND_CENTER_HUD");
        if (canvasObject == null || canvasObject.GetComponent<Canvas>() == null)
        {
            Debug.LogError("[COMMAND CENTER] Canvas not found. Nothing changed.");
            return;
        }

        var oldBuild = canvasObject.transform.Find("COMMAND_CENTER_REDESIGNED_UI");
        if (oldBuild != null)
            Undo.DestroyObjectImmediate(oldBuild.gameObject);

        var realUI = FindDeep(canvasObject.transform, "COMMAND_CENTER_REAL_UI");
        if (realUI == null)
        {
            Debug.LogError("[COMMAND CENTER] Existing department container not found.");
            return;
        }

        var panels = new GameObject[PanelNames.Length];
        for (int i = 0; i < PanelNames.Length; i++)
        {
            panels[i] = FindDeep(realUI, PanelNames[i]).gameObject;
            if (panels[i] == null)
            {
                Debug.LogError("[COMMAND CENTER] Missing panel: " + PanelNames[i] + ". Nothing changed.");
                return;
            }
        }

        var root = CreateUIObject("COMMAND_CENTER_REDESIGNED_UI", canvasObject.transform);
        Stretch(root.GetComponent<RectTransform>());

        var navigation = root.AddComponent<CommandCenterDashboardNavigation>();

        var overview = CreatePanel("COMMAND_OVERVIEW", root.transform,
            new Vector2(0, 0), new Vector2(1, 1),
            new Vector2(270, 0), new Vector2(0, -76),
            new Color32(9, 17, 27, 255));

        navigation.overview = overview;

        CreatePanel("TOP_COMMAND_HEADER", root.transform,
            new Vector2(0, 1), new Vector2(1, 1),
            new Vector2(0, -76), Vector2.zero,
            new Color32(13, 25, 38, 255));

        var header = CreateText("OBSIDIAN PROTOCOL  //  COMMAND CENTER",
            root.transform, 30, TextAnchor.MiddleLeft,
            new Color32(105, 220, 255, 255));
        SetRect(header.rectTransform, new Vector2(0, 1), new Vector2(1, 1),
            new Vector2(28, -76), new Vector2(-28, 0));

        var subtitle = CreateText("AUTONOMOUS WARFARE  |  COMMAND INTERFACE",
            root.transform, 13, TextAnchor.MiddleRight,
            new Color32(145, 166, 184, 255));
        SetRect(subtitle.rectTransform, new Vector2(0, 1), new Vector2(1, 1),
            new Vector2(0, -76), new Vector2(-30, 0));

        var sidebar = CreatePanel("DEPARTMENT_NAVIGATION", root.transform,
            new Vector2(0, 0), new Vector2(0, 1),
            new Vector2(270, 0), new Vector2(0, -76),
            new Color32(12, 23, 35, 255));

        var navTitle = CreateText("DEPARTMENTS",
            sidebar.transform, 16, TextAnchor.MiddleLeft,
            new Color32(105, 220, 255, 255));
        SetRect(navTitle.rectTransform, new Vector2(0, 1), new Vector2(1, 1),
            new Vector2(20, -62), new Vector2(-20, 0));

        string[] labels =
        {
            "COMMAND", "OPERATIONS", "FLEET", "DEPLOYMENT",
            "LOGISTICS", "RESEARCH", "GARAGE", "FINANCE",
            "INTELLIGENCE", "STORE"
        };

        var departments = new CommandCenterDashboardNavigation.Department[panels.Length];

        for (int i = 0; i < panels.Length; i++)
        {
            string buttonName = "NAV_" + labels[i].Replace(" ", "_");
            var button = CreateButton(buttonName, labels[i], sidebar.transform, i);
            int index = i;
            button.onClick.AddListener(() => { });

            departments[index] = new CommandCenterDashboardNavigation.Department
            {
                buttonName = buttonName,
                panel = panels[index]
            };
        }

        var serialized = new SerializedObject(navigation);
        var array = serialized.FindProperty("departments");
        array.arraySize = departments.Length;

        for (int i = 0; i < departments.Length; i++)
        {
            array.GetArrayElementAtIndex(i).FindPropertyRelative("buttonName").stringValue =
                departments[i].buttonName;
            array.GetArrayElementAtIndex(i).FindPropertyRelative("panel").objectReferenceValue =
                departments[i].panel;
        }
        serialized.ApplyModifiedPropertiesWithoutUndo();

        var welcome = CreateText("COMMAND OVERVIEW",
            overview.transform, 32, TextAnchor.MiddleLeft,
            new Color32(105, 220, 255, 255));
        SetRect(welcome.rectTransform, new Vector2(0, 1), new Vector2(1, 1),
            new Vector2(48, -150), new Vector2(-48, 0));

        var instruction = CreateText(
            "Select a department from the navigation rail to access its existing interface.",
            overview.transform, 18, TextAnchor.MiddleLeft,
            new Color32(190, 205, 218, 255));
        SetRect(instruction.rectTransform, new Vector2(0, 1), new Vector2(1, 1),
            new Vector2(48, -205), new Vector2(-48, 0));

        var line = CreatePanel("OVERVIEW_ACCENT_LINE", overview.transform,
            new Vector2(0, 1), new Vector2(0, 1),
            new Vector2(48, -230), new Vector2(430, 2),
            new Color32(40, 170, 210, 255));

        foreach (var panel in panels)
            panel.SetActive(false);

        overview.SetActive(true);

        string[] legacyVisuals =
        {
            "BACKGROUND", "BOTTOM_NAVIGATION", "CENTER_CONTENT",
            "RIGHT_COMMAND_PANEL", "TOP_COMMAND_BAR"
        };

        foreach (string name in legacyVisuals)
        {
            var oldObject = FindDirect(canvasObject.transform, name);
            if (oldObject != null)
                oldObject.SetActive(false);
        }

        root.transform.SetAsLastSibling();

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());

        Debug.Log("[COMMAND CENTER] Redesigned dashboard built. Ten existing department panels preserved.");
    }

    private static GameObject CreateUIObject(string name, Transform parent)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        return go;
    }

    private static GameObject CreatePanel(string name, Transform parent,
        Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin,
        Vector2 offsetMax, Color color)
    {
        var go = CreateUIObject(name, parent);
        var rect = go.GetComponent<RectTransform>();
        SetRect(rect, anchorMin, anchorMax, offsetMin, offsetMax);
        var image = go.AddComponent<Image>();
        image.color = color;
        return go;
    }

    private static Text CreateText(string value, Transform parent,
        int size, TextAnchor alignment, Color color)
    {
        var go = CreateUIObject("TEXT_" + value.Replace(" ", "_"), parent);
        var text = go.AddComponent<Text>();
        text.text = value;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = size;
        text.alignment = alignment;
        text.color = color;
        text.raycastTarget = false;
        return text;
    }

    private static Button CreateButton(string name, string label,
        Transform parent, int index)
    {
        var go = CreateUIObject(name, parent);
        var rect = go.GetComponent<RectTransform>();
        SetRect(rect, new Vector2(0, 1), new Vector2(1, 1),
            new Vector2(14, -112 - index * 66),
            new Vector2(-14, -60 - index * 66));

        var image = go.AddComponent<Image>();
        image.color = new Color32(24, 43, 59, 255);

        var button = go.AddComponent<Button>();
        var colors = button.colors;
        colors.normalColor = new Color32(24, 43, 59, 255);
        colors.highlightedColor = new Color32(34, 75, 96, 255);
        colors.pressedColor = new Color32(20, 115, 145, 255);
        colors.selectedColor = colors.highlightedColor;
        button.colors = colors;

        var text = CreateText(label, go.transform, 15,
            TextAnchor.MiddleLeft, new Color32(215, 230, 240, 255));
        SetRect(text.rectTransform, Vector2.zero, Vector2.one,
            new Vector2(16, 0), new Vector2(-8, 0));

        return button;
    }

    private static void Stretch(RectTransform rect)
    {
        SetRect(rect, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
    }

    private static void SetRect(RectTransform rect, Vector2 min, Vector2 max,
        Vector2 offsetMin, Vector2 offsetMax)
    {
        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = offsetMin;
        rect.offsetMax = offsetMax;
    }

    private static Transform FindDeep(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name) return child;
            var found = FindDeep(child, name);
            if (found != null) return found;
        }
        return null;
    }

    private static GameObject FindDirect(Transform parent, string name)
    {
        foreach (Transform child in parent)
            if (child.name == name) return child.gameObject;
        return null;
    }
}


