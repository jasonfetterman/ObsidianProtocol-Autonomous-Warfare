using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public static class BuildIntelligencePanelLaunchers
{
    static readonly string[] Panels = {
        "SENSORS","THREAT ANALYSIS","NETWORK",
        "INTELLIGENCE REPORTS","CONTACTS","SURVEILLANCE","ANALYSIS"
    };

    [MenuItem("Obsidian Protocol/Fix/Create Intelligence Panel Launchers")]
    public static void Build()
    {
        GameObject hud = GameObject.Find("INTELLIGENCE HUD");
        if (hud == null) return;

        foreach (string name in Panels)
        {
            Transform panel = hud.transform.Find(name);
            if (panel == null) continue;

            panel.gameObject.SetActive(false);

            string buttonName = "OPEN - " + name;
            if (hud.transform.Find(buttonName) != null) continue;

            GameObject go = new GameObject(buttonName);
            go.transform.SetParent(hud.transform, false);

            RectTransform r = go.AddComponent<RectTransform>();
            r.sizeDelta = new Vector2(190, 42);

            Image img = go.AddComponent<Image>();
            img.color = new Color(.12f,.12f,.12f,.95f);

            Button b = go.AddComponent<Button>();

            GameObject textGo = new GameObject("TEXT");
            textGo.transform.SetParent(go.transform, false);

            RectTransform tr = textGo.AddComponent<RectTransform>();
            tr.anchorMin = Vector2.zero;
            tr.anchorMax = Vector2.one;
            tr.offsetMin = Vector2.zero;
            tr.offsetMax = Vector2.zero;

            TextMeshProUGUI text = textGo.AddComponent<TextMeshProUGUI>();
            text.text = "OPEN " + name;
            text.alignment = TextAlignmentOptions.Center;
            text.fontSize = 14;

            r.anchoredPosition = new Vector2(
                -110,
                300 - ArrayIndex(name) * 50);
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

        Debug.Log("[INTELLIGENCE] Seven panel launchers created.");
    }

    static int ArrayIndex(string name)
    {
        for (int i = 0; i < Panels.Length; i++)
            if (Panels[i] == name) return i;
        return 0;
    }
}
