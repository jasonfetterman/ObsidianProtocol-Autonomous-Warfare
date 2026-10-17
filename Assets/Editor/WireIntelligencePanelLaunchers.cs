using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using ObsidianProtocol.Game.Intelligence;

public static class WireIntelligencePanelLaunchers
{
    [MenuItem("Obsidian Protocol/Fix/Wire Panel Launchers")]
    public static void Build()
    {
        GameObject hud = GameObject.Find("INTELLIGENCE HUD");
        if (hud == null) return;

        var controller = hud.GetComponent<IntelligenceMiddlePanelController>();
        if (controller == null) controller = hud.AddComponent<IntelligenceMiddlePanelController>();

        Wire("OPEN - SENSORS", controller.OpenSensors);
        Wire("OPEN - THREAT ANALYSIS", controller.OpenThreatAnalysis);
        Wire("OPEN - NETWORK", controller.OpenNetwork);
        Wire("OPEN - INTELLIGENCE REPORTS", controller.OpenIntelligenceReports);
        Wire("OPEN - CONTACTS", controller.OpenContacts);
        Wire("OPEN - SURVEILLANCE", controller.OpenSurveillance);
        Wire("OPEN - ANALYSIS", controller.OpenAnalysis);

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

        Debug.Log("[INTELLIGENCE] Seven panel launchers wired.");
    }

    static void Wire(string name, UnityEngine.Events.UnityAction action)
    {
        GameObject go = GameObject.Find(name);
        if (go == null) return;

        Button button = go.GetComponent<Button>();
        if (button == null) return;

        button.onClick.RemoveAllListeners();
        UnityEventTools.AddPersistentListener(button.onClick, action);
    }
}
