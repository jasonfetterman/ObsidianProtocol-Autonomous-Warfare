using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using ObsidianProtocol.Game.Intelligence;

public static class WireIntelligencePanelOpeners
{
    [MenuItem("Obsidian Protocol/Fix/Wire Intelligence Panel Openers")]
    public static void Build()
    {
        GameObject hud = GameObject.Find("INTELLIGENCE HUD");
        if (hud == null) return;

        var controller = hud.GetComponent<IntelligenceMiddlePanelController>();
        if (controller == null) controller = hud.AddComponent<IntelligenceMiddlePanelController>();

        Wire("BUTTON - SENSOR CONTROL", controller.OpenSensors);
        Wire("BUTTON - ANALYZE CONTACT", controller.OpenContacts);
        Wire("BUTTON - ANALYZE DATA", controller.OpenAnalysis);
        Wire("BUTTON - NETWORK CONTROL", controller.OpenNetwork);
        Wire("BUTTON - OPEN REPORT", controller.OpenIntelligenceReports);
        Wire("BUTTON - VIEW FEED", controller.OpenSurveillance);

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

        Debug.Log("[INTELLIGENCE] Panel openers wired.");
    }

    private static void Wire(string name, UnityEngine.Events.UnityAction action)
    {
        GameObject button = GameObject.Find(name);
        if (button == null) return;

        Button ui = button.GetComponent<Button>();
        if (ui == null) return;

        ui.onClick.RemoveAllListeners();
        UnityEventTools.AddPersistentListener(ui.onClick, action);
    }
}
