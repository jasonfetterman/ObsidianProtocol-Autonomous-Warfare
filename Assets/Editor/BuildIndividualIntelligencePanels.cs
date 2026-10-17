using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using ObsidianProtocol.Game.Intelligence;

public static class BuildIndividualIntelligencePanels
{
    private static readonly string[] Panels =
    {
        "SENSORS",
        "THREAT ANALYSIS",
        "NETWORK",
        "INTELLIGENCE REPORTS",
        "CONTACTS",
        "SURVEILLANCE",
        "ANALYSIS"
    };

    [MenuItem("Obsidian Protocol/Fix/Build Individual Intelligence Panel Controls")]
    public static void Build()
    {
        GameObject hud = GameObject.Find("INTELLIGENCE HUD");
        if (hud == null) return;

        foreach (string panelName in Panels)
        {
            Transform panel = hud.transform.Find(panelName);
            if (panel == null) continue;

            var controller = panel.GetComponent<IntelligenceMiddlePanelController>();
            if (controller == null)
                controller = panel.gameObject.AddComponent<IntelligenceMiddlePanelController>();

            panel.gameObject.SetActive(false);

            GameObject button = new GameObject("BUTTON - CLOSE");
            button.transform.SetParent(panel, false);

            RectTransform rect = button.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(1, 1);
            rect.anchorMax = new Vector2(1, 1);
            rect.pivot = new Vector2(1, 1);
            rect.anchoredPosition = new Vector2(-15, -15);
            rect.sizeDelta = new Vector2(100, 35);

            button.AddComponent<Image>();
            Button ui = button.AddComponent<Button>();

            ui.onClick.RemoveAllListeners();
            UnityEventTools.AddPersistentListener(ui.onClick, controller.CloseThisPanel);
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

        Debug.Log("[INTELLIGENCE] Individual panel controls built.");
    }
}
