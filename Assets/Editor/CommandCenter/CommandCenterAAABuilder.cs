using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public static class CommandCenterAAABuilder
{
    const string ScenePath = "Assets/Scenes/SCN02_COMMAND_CENTER/COMMAND CENTER HUD/Command_Center.unity";

    static readonly string[] Names = {
        "COMMAND_REAL_PANEL", "OPERATIONS_REAL_PANEL", "FLEET_REAL_PANEL",
        "DEPLOY_REAL_PANEL", "LOGISTICS_REAL_PANEL", "RESEARCH_REAL_PANEL",
        "GARAGE_REAL_PANEL", "FINANCE_REAL_PANEL", "INTELLIGENCE_REAL_PANEL",
        "STORE_REAL_PANEL"
    };

    static readonly string[] Labels = {
        "COMMAND", "OPERATIONS", "FLEET", "DEPLOYMENT", "LOGISTICS",
        "RESEARCH", "GARAGE", "FINANCE", "INTELLIGENCE", "STORE"
    };

    static readonly Color Background = new Color32(5, 10, 17, 255);
    static readonly Color Surface = new Color32(12, 23, 35, 255);
    static readonly Color Surface2 = new Color32(17, 32, 46, 255);
    static readonly Color Border = new Color32(37, 59, 75, 255);
    static readonly Color Cyan = new Color32(64, 203, 235, 255);
    static readonly Color White = new Color32(226, 237, 245, 255);
    static readonly Color Muted = new Color32(139, 160, 177, 255);

    [MenuItem("Obsidian Protocol/Command Center/BUILD PLAN B COMMAND CENTER")]
    public static void Build()
    {
        if (EditorSceneManager.GetActiveScene().path != ScenePath)
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

        var canvas = GameObject.Find("COMMAND_CENTER_HUD");
        if (canvas == null || canvas.GetComponent<Canvas>() == null)
        {
            Debug.LogError("[OPAW] Command Center Canvas not found.");
            return;
        }

        Transform realUI = Find(canvas.transform, "COMMAND_CENTER_REAL_UI");
        if (realUI == null)
        {
            Debug.LogError("[OPAW] Existing department container not found.");
            return;
        }

        var panels = new GameObject[Names.Length];
        for (int i = 0; i < Names.Length; i++)
        {
            Transform found = Find(canvas.transform, Names[i]);
            if (found == null)
            {
                Debug.LogError("[OPAW] Missing existing department: " + Names[i]);
                return;
            }
            panels[i] = found.gameObject;
        }

        string sceneBackup = ScenePath + ".backup_plan_b_" +
                             DateTime.Now.ToString("yyyyMMdd_HHmmss");
        File.Copy(ScenePath, sceneBackup, false);

        Undo.RegisterFullObjectHierarchyUndo(canvas, "Build Plan B Command Center");

        // Protect the original department objects before removing generated UI.
        foreach (var panel in panels)
            panel.transform.SetParent(realUI, false);

        DestroyGenerated(canvas.transform, "COMMAND_CENTER_AAA");
        DestroyGenerated(canvas.transform, "COMMAND_CENTER_REDESIGNED_UI");

        var root = NewUI("COMMAND_CENTER_AAA", canvas.transform);
        Stretch(root.GetComponent<RectTransform>());

        Panel(root.transform, "FULLSCREEN_BACKDROP",
            Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, Background);

        BuildHeader(root.transform);
        BuildNavigation(root.transform);

        var workspace = NewUI("COMMAND_WORKSPACE", root.transform);
        SetRect(workspace.GetComponent<RectTransform>(),
            new Vector2(0, 0), new Vector2(1, 1),
            new Vector2(274, 18), new Vector2(-22, -98));

        BuildOverview(workspace.transform);

        var nav = root.AddComponent<CommandCenterDashboardNavigation>();
        nav.overview = workspace.transform.Find("COMMAND_OVERVIEW").gameObject;

        var serialized = new SerializedObject(nav);
        var array = serialized.FindProperty("departments");
        array.arraySize = panels.Length;

        for (int i = 0; i < panels.Length; i++)
        {
            string buttonName = "NAV_" + Labels[i];
            CreateNavButton(root.transform, buttonName, Labels[i], i);

            var element = array.GetArrayElementAtIndex(i);
            element.FindPropertyRelative("buttonName").stringValue = buttonName;
            element.FindPropertyRelative("panel").objectReferenceValue = panels[i];

            panels[i].transform.SetParent(workspace.transform, false);
            var rect = panels[i].GetComponent<RectTransform>();
            if (rect != null) Stretch(rect);
            panels[i].SetActive(false);
        }

        serialized.ApplyModifiedPropertiesWithoutUndo();
        nav.overview.SetActive(true);
        realUI.gameObject.SetActive(true);

        string[] oldVisuals = {
            "BACKGROUND", "BOTTOM_NAVIGATION", "CENTER_CONTENT",
            "RIGHT_COMMAND_PANEL", "TOP_COMMAND_BAR"
        };

        foreach (string name in oldVisuals)
        {
            Transform old = DirectChild(canvas.transform, name);
            if (old != null) old.gameObject.SetActive(false);
        }

        Transform interaction = DirectChild(canvas.transform, "COMMAND_CENTER_FULL_INTERACTION");
        if (interaction != null)
        {
            var group = interaction.GetComponent<CanvasGroup>();
            if (group == null) group = interaction.gameObject.AddComponent<CanvasGroup>();
            group.alpha = 0;
            group.interactable = false;
            group.blocksRaycasts = false;
        }

        root.transform.SetAsLastSibling();
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());

        Debug.Log("[OPAW] PLAN B COMMAND CENTER BUILT. Scene backup: " + sceneBackup);
    }

    static void BuildHeader(Transform parent)
    {
        Panel(parent, "TOP_COMMAND_HEADER",
            new Vector2(0, 1), Vector2.one,
            new Vector2(0, -82), Vector2.zero, Surface);

        Panel(parent, "HEADER_BOTTOM_RULE",
            new Vector2(0, 1), Vector2.one,
            new Vector2(0, -84), new Vector2(0, -82), Cyan);

        TextLabel(parent, "HEADER_BRAND",
            "OBSIDIAN PROTOCOL", 17, Cyan, TextAnchor.MiddleLeft,
            new Vector2(0, 1), new Vector2(0, 1),
            new Vector2(26, -65), new Vector2(270, -16));

        TextLabel(parent, "HEADER_TITLE",
            "COMMAND CENTER", 23, White, TextAnchor.MiddleLeft,
            new Vector2(0, 1), new Vector2(0, 1),
            new Vector2(285, -65), new Vector2(620, -16));

        TextLabel(parent, "HEADER_CONTEXT",
            "AUTONOMOUS WARFARE  /  HEADQUARTERS",
            11, Muted, TextAnchor.MiddleRight,
            new Vector2(1, 1), new Vector2(1, 1),
            new Vector2(-430, -64), new Vector2(-26, -18));
    }

    static void BuildNavigation(Transform parent)
    {
        Panel(parent, "DEPARTMENT_NAVIGATION",
            Vector2.zero, new Vector2(0, 1),
            new Vector2(0, 0), new Vector2(258, -84), Surface);

        Panel(parent, "NAVIGATION_EDGE",
            new Vector2(0, 0), new Vector2(0, 1),
            new Vector2(257, 0), new Vector2(259, -84), Cyan);

        TextLabel(parent, "NAVIGATION_TITLE",
            "HEADQUARTERS", 12, Cyan, TextAnchor.MiddleLeft,
            new Vector2(0, 1), new Vector2(0, 1),
            new Vector2(22, -133), new Vector2(240, -94));

        TextLabel(parent, "NAVIGATION_SUBTITLE",
            "DEPARTMENT ACCESS", 10, Muted, TextAnchor.MiddleLeft,
            new Vector2(0, 1), new Vector2(0, 1),
            new Vector2(22, -159), new Vector2(240, -133));
    }

    static void BuildOverview(Transform parent)
    {
        var overview = NewUI("COMMAND_OVERVIEW", parent);
        Stretch(overview.GetComponent<RectTransform>());

        TextLabel(overview.transform, "OVERVIEW_EYEBROW",
            "HEADQUARTERS  /  OPERATIONAL PICTURE",
            11, Cyan, TextAnchor.MiddleLeft,
            new Vector2(0, 1), new Vector2(1, 1),
            new Vector2(12, -37), new Vector2(-12, -10));

        TextLabel(overview.transform, "OVERVIEW_TITLE",
            "Operations Overview", 27, White, TextAnchor.MiddleLeft,
            new Vector2(0, 1), new Vector2(1, 1),
            new Vector2(10, -78), new Vector2(-10, -39));

        TextLabel(overview.transform, "OVERVIEW_DESCRIPTION",
            "Headquarters systems and operational departments",
            12, Muted, TextAnchor.MiddleLeft,
            new Vector2(0, 1), new Vector2(1, 1),
            new Vector2(12, -106), new Vector2(-12, -81));

        Panel(overview.transform, "OVERVIEW_RULE",
            new Vector2(0, 1), new Vector2(1, 1),
            new Vector2(10, -113), new Vector2(-10, -111), Border);

        string[] titles = {
            "OPERATIONS OVERVIEW", "UNIT READINESS",
            "FLEET STATUS", "ACTIVE MISSIONS",
            "INTELLIGENCE REPORTS", "LOGISTICS",
            "RESEARCH & DEVELOPMENT", "FINANCE / RESOURCES",
            "DEPLOYMENT", "NEWS & ALERTS"
        };

        string[] descriptions = {
            "Access operational coordination and headquarters controls.",
            "Review unit condition and availability through fleet systems.",
            "Access fleet composition and existing unit interfaces.",
            "Review mission operations and current assignments.",
            "Access intelligence reports and available battlefield information.",
            "Manage logistics and access supply-related systems.",
            "Access research systems and technology progression.",
            "Review finance and available resource interfaces.",
            "Access deployment controls and unit assignment systems.",
            "Review available intelligence updates and operational notices."
        };

        for (int i = 0; i < titles.Length; i++)
        {
            int row = i / 2;
            bool right = (i % 2) == 1;
            float left = right ? 0.5f : 0f;
            float rightAnchor = right ? 1f : 0.5f;

            float top = 130 + row * 139;
            float bottom = top + 126;

            BuildInfoCard(overview.transform, "DASHBOARD_CARD_" + i,
                titles[i], descriptions[i],
                new Vector2(left, 1), new Vector2(rightAnchor, 1),
                new Vector2(right ? 9 : 10, -bottom),
                new Vector2(right ? -10 : -9, -top));
        }
    }

    static void BuildInfoCard(Transform parent, string name, string title,
        string description, Vector2 min, Vector2 max,
        Vector2 offsetMin, Vector2 offsetMax)
    {
        var card = Panel(parent, name, min, max, offsetMin, offsetMax, Surface);

        Panel(card.transform, "CARD_ACCENT",
            new Vector2(0, 0), new Vector2(0, 1),
            new Vector2(0, 0), new Vector2(3, 0), Cyan);

        Panel(card.transform, "CARD_TOP_RULE",
            new Vector2(0, 1), new Vector2(1, 1),
            new Vector2(3, 0), new Vector2(0, -1), Border);

        TextLabel(card.transform, "CARD_TITLE",
            title, 14, White, TextAnchor.MiddleLeft,
            new Vector2(0, 1), new Vector2(1, 1),
            new Vector2(19, -43), new Vector2(-16, -12));

        TextLabel(card.transform, "CARD_DESCRIPTION",
            description, 11, Muted, TextAnchor.UpperLeft,
            new Vector2(0, 0), new Vector2(1, 1),
            new Vector2(19, 17), new Vector2(-16, -49));

        TextLabel(card.transform, "CARD_ACCESS_HINT",
            "ACCESS THROUGH DEPARTMENT NAVIGATION",
            9, Cyan, TextAnchor.MiddleLeft,
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(19, 3), new Vector2(-12, 27));
    }

    static void CreateNavButton(Transform parent, string name, string label, int index)
    {
        var go = NewUI(name, parent);
        SetRect(go.GetComponent<RectTransform>(),
            new Vector2(0, 1), new Vector2(0, 1),
            new Vector2(12, -207 - index * 67),
            new Vector2(246, -151 - index * 67));

        var image = go.AddComponent<Image>();
        image.color = Surface2;

        var button = go.AddComponent<Button>();
        var colors = button.colors;
        colors.normalColor = Surface2;
        colors.highlightedColor = new Color32(29, 61, 79, 255);
        colors.pressedColor = new Color32(20, 91, 119, 255);
        colors.selectedColor = colors.highlightedColor;
        button.colors = colors;

        TextLabel(go.transform, "NAV_INDEX",
            (index + 1).ToString("00"), 11, Cyan, TextAnchor.MiddleLeft,
            Vector2.zero, Vector2.one,
            new Vector2(13, 0), new Vector2(43, 0));

        TextLabel(go.transform, "NAV_LABEL",
            label, 12, White, TextAnchor.MiddleLeft,
            Vector2.zero, Vector2.one,
            new Vector2(49, 0), new Vector2(-8, 0));
    }

    static GameObject Panel(Transform parent, string name,
        Vector2 min, Vector2 max, Vector2 offsetMin, Vector2 offsetMax, Color color)
    {
        var go = NewUI(name, parent);
        SetRect(go.GetComponent<RectTransform>(), min, max, offsetMin, offsetMax);
        go.AddComponent<Image>().color = color;
        return go;
    }

    static Text TextLabel(Transform parent, string name, string value,
        int size, Color color, TextAnchor alignment,
        Vector2 min, Vector2 max, Vector2 offsetMin, Vector2 offsetMax)
    {
        var go = NewUI(name, parent);
        SetRect(go.GetComponent<RectTransform>(), min, max, offsetMin, offsetMax);

        var text = go.AddComponent<Text>();
        text.text = value;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = size;
        text.color = color;
        text.alignment = alignment;
        text.raycastTarget = false;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Truncate;
        return text;
    }

    static GameObject NewUI(string name, Transform parent)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        return go;
    }

    static void Stretch(RectTransform rect)
    {
        SetRect(rect, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
    }

    static void SetRect(RectTransform rect, Vector2 min, Vector2 max,
        Vector2 offsetMin, Vector2 offsetMax)
    {
        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = offsetMin;
        rect.offsetMax = offsetMax;
    }

    static Transform Find(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name) return child;
            Transform result = Find(child, name);
            if (result != null) return result;
        }
        return null;
    }

    static Transform DirectChild(Transform parent, string name)
    {
        foreach (Transform child in parent)
            if (child.name == name) return child;
        return null;
    }

    static void DestroyGenerated(Transform parent, string name)
    {
        Transform existing = DirectChild(parent, name);
        if (existing != null)
            Undo.DestroyObjectImmediate(existing.gameObject);
    }
}
