using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using ObsidianProtocol.UI;

public static class BuildBattlefieldUnitRosterPager
{
    private const string MenuPath = "Obsidian Protocol/Battlefield/Build Five-Card Unit Roster";

    [MenuItem(MenuPath)]
    public static void Build()
    {
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if (!scene.IsValid() || !scene.isLoaded)
        {
            Debug.LogError("[UnitRosterPagerBuilder] No loaded scene.");
            return;
        }

        Transform miniMap = FindTransform(scene, "MiniMap");
        if (miniMap == null)
        {
            Debug.LogError("[UnitRosterPagerBuilder] Could not find MiniMap in the active scene. No changes made.");
            return;
        }

        var slotNames = new[] {
            "Selected_Units (1)", "Selected_Units (2)", "Selected_Units (3)",
            "Selected_Units (4)", "Selected_Units (5)"
        };
        var slots = new RectTransform[5];
        var labels = new Text[5];
        var images = new Image[5];

        for (int i = 0; i < slotNames.Length; i++)
        {
            Transform slot = miniMap.Find(slotNames[i]);
            if (slot == null)
            {
                Debug.LogError("[UnitRosterPagerBuilder] Missing " + slotNames[i] + ". No changes made.");
                return;
            }
            slots[i] = slot as RectTransform;
            images[i] = slot.GetComponent<Image>();
            if (slots[i] == null || images[i] == null)
            {
                Debug.LogError("[UnitRosterPagerBuilder] " + slotNames[i] + " is missing its expected RectTransform/Image. No changes made.");
                return;
            }
        }

        Undo.IncrementCurrentGroup();
        int group = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName("Build Five-Card Battlefield Unit Roster");

        var root = miniMap.Find("UNIT_ROSTER_PAGING");
        GameObject rootObject;
        if (root == null)
        {
            rootObject = new GameObject("UNIT_ROSTER_PAGING", typeof(RectTransform));
            Undo.RegisterCreatedObjectUndo(rootObject, "Create unit roster paging root");
            rootObject.transform.SetParent(miniMap, false);
            var rt = rootObject.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = Vector2.zero;
        }
        else rootObject = root.gameObject;

        var pager = rootObject.GetComponent<BattlefieldUnitRosterPager>();
        if (pager == null) pager = Undo.AddComponent<BattlefieldUnitRosterPager>(rootObject);

        for (int i = 0; i < 5; i++)
        {
            Undo.AddComponent<Button>(slots[i].gameObject);
            var button = slots[i].GetComponent<Button>();
            button.transition = Selectable.Transition.None;
            labels[i] = GetOrCreateLabel(slots[i], "UNIT_CARD_LABEL_" + (i + 1));
            images[i].raycastTarget = true;
        }

        var previous = GetOrCreateNavButton(rootObject.transform, "PREVIOUS_PAGE", new Vector2(-70f, -42f), "◀");
        var next = GetOrCreateNavButton(rootObject.transform, "NEXT_PAGE", new Vector2(70f, -42f), "▶");
        var pageLabel = GetOrCreateText(rootObject.transform, "PAGE_STATUS", new Vector2(0f, -42f), new Vector2(220f, 34f), "1 / 1     0 UNITS");
        pageLabel.alignment = TextAnchor.MiddleCenter;
        pageLabel.fontSize = 14;
        pageLabel.color = new Color(0.45f, 0.9f, 1f, 1f);

        var so = new SerializedObject(pager);
        so.FindProperty("cardRoots").arraySize = 5;
        so.FindProperty("cardLabels").arraySize = 5;
        so.FindProperty("cardImages").arraySize = 5;
        for (int i = 0; i < 5; i++)
        {
            so.FindProperty("cardRoots").GetArrayElementAtIndex(i).objectReferenceValue = slots[i];
            so.FindProperty("cardLabels").GetArrayElementAtIndex(i).objectReferenceValue = labels[i];
            so.FindProperty("cardImages").GetArrayElementAtIndex(i).objectReferenceValue = images[i];
        }
        so.FindProperty("previousButton").objectReferenceValue = previous;
        so.FindProperty("nextButton").objectReferenceValue = next;
        so.FindProperty("pageLabel").objectReferenceValue = pageLabel;
        so.ApplyModifiedProperties();

        EditorSceneManager.MarkSceneDirty(scene);
        Undo.CollapseUndoOperations(group);
        Debug.Log("[UnitRosterPagerBuilder] Built five selectable roster cards and paging controls. Review the scene, then save it.");
    }

    private static Transform FindTransform(UnityEngine.SceneManagement.Scene scene, string name)
    {
        foreach (var go in scene.GetRootGameObjects())
            foreach (var t in go.GetComponentsInChildren<Transform>(true))
                if (t.name == name) return t;
        return null;
    }

    private static Text GetOrCreateLabel(RectTransform parent, string name)
    {
        Transform existing = parent.Find(name);
        if (existing != null) return existing.GetComponent<Text>();
        var go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
        Undo.RegisterCreatedObjectUndo(go, "Create unit card label");
        go.transform.SetParent(parent, false);
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2(5f, 3f);
        rt.offsetMax = new Vector2(-5f, -3f);
        var text = go.GetComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 12;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = new Color(0.78f, 0.94f, 1f, 1f);
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Truncate;
        text.raycastTarget = false;
        return text;
    }

    private static Button GetOrCreateNavButton(Transform parent, string name, Vector2 position, string caption)
    {
        Transform existing = parent.Find(name);
        GameObject go = existing != null ? existing.gameObject : new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        if (existing == null)
        {
            Undo.RegisterCreatedObjectUndo(go, "Create roster page button");
            go.transform.SetParent(parent, false);
        }
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0f);
        rt.anchorMax = new Vector2(0.5f, 0f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = position;
        rt.sizeDelta = new Vector2(42f, 28f);
        var image = go.GetComponent<Image>();
        image.color = new Color(0.0f, 0.45f, 0.58f, 0.9f);
        var button = go.GetComponent<Button>();
        button.targetGraphic = image;
        var label = GetOrCreateLabel(rt, "LABEL");
        label.text = caption;
        label.fontSize = 16;
        label.color = Color.white;
        return button;
    }

    private static Text GetOrCreateText(Transform parent, string name, Vector2 position, Vector2 size, string value)
    {
        Transform existing = parent.Find(name);
        GameObject go = existing != null ? existing.gameObject : new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
        if (existing == null)
        {
            Undo.RegisterCreatedObjectUndo(go, "Create roster page label");
            go.transform.SetParent(parent, false);
        }
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0f);
        rt.anchorMax = new Vector2(0.5f, 0f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = position;
        rt.sizeDelta = size;
        var text = go.GetComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.text = value;
        text.raycastTarget = false;
        return text;
    }
}
