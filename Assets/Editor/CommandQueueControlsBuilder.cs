using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class CommandQueueControlsBuilder
{
    [MenuItem("Obsidian Protocol/Command Center/BUILD COMMAND QUEUE CONTROLS")]
    public static void Build()
    {
        GameObject queue = System.Array.Find(UnityEngine.Object.FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None), x => x.name == "COMMAND_QUEUE");

        if (queue == null)
        {
            Debug.LogError("[COMMAND QUEUE] COMMAND_QUEUE not found.");
            return;
        }

        Transform old = queue.transform.Find("QUEUE_CONTROLS");
        if (old != null)
            Object.DestroyImmediate(old.gameObject);

        GameObject controls = new GameObject(
            "QUEUE_CONTROLS",
            typeof(RectTransform)
        );

        controls.transform.SetParent(queue.transform, false);

        RectTransform cr = controls.GetComponent<RectTransform>();
        cr.anchorMin = new Vector2(0, 0);
        cr.anchorMax = new Vector2(1, 0);
        cr.pivot = new Vector2(.5f, 0);
        cr.anchoredPosition = new Vector2(0, 8);
        cr.sizeDelta = new Vector2(-24, 42);

        CreateButton(controls.transform, "VIEW", 0);
        CreateButton(controls.transform, "APPROVE", 1);
        CreateButton(controls.transform, "REJECT", 2);
        CreateButton(controls.transform, "CLEAR", 3);

        EditorUtility.SetDirty(queue);
        EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );
        EditorSceneManager.SaveScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );

        Debug.Log("[COMMAND QUEUE] CONTROLS BUILT.");
    }

    private static void CreateButton(
        Transform parent,
        string label,
        int index)
    {
        GameObject buttonObject = new GameObject(
            label,
            typeof(RectTransform),
            typeof(Image),
            typeof(Button)
        );

        buttonObject.transform.SetParent(parent, false);

        RectTransform r = buttonObject.GetComponent<RectTransform>();

        r.anchorMin = new Vector2(index * .25f, 0);
        r.anchorMax = new Vector2((index + 1) * .25f, 1);
        r.offsetMin = new Vector2(4, 2);
        r.offsetMax = new Vector2(-4, -2);

        Image image = buttonObject.GetComponent<Image>();
        image.color = new Color(.08f, .11f, .14f, 1f);

        GameObject textObject = new GameObject(
            "TEXT",
            typeof(RectTransform),
            typeof(Text)
        );

        textObject.transform.SetParent(buttonObject.transform, false);

        RectTransform tr = textObject.GetComponent<RectTransform>();
        tr.anchorMin = Vector2.zero;
        tr.anchorMax = Vector2.one;
        tr.offsetMin = Vector2.zero;
        tr.offsetMax = Vector2.zero;

        Text text = textObject.GetComponent<Text>();
        text.text = label;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 14;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
    }
}
