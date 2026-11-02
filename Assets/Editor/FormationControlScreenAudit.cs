using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class FormationControlScreenAudit
{
    [MenuItem("Obsidian Protocol/Deployment/Audit Formation Control Screens")]
    public static void Audit()
    {
        var scene = EditorSceneManager.OpenScene(
            "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity",
            OpenSceneMode.Single
        );

        GameObject console =
            GameObject.Find(
                "Console_Formation_ControlTerrminals"
            );

        if (console == null)
        {
            Debug.LogError(
                "[FORMATION CONTROL AUDIT] CONSOLE NOT FOUND"
            );
            return;
        }

        Debug.Log(
            "[FORMATION CONTROL AUDIT] CONSOLE FOUND | WORLD=" +
            console.transform.position +
            " | ROT=" +
            console.transform.eulerAngles
        );

        foreach (
            Transform t in
            console.GetComponentsInChildren<Transform>(true)
        )
        {
            if (!t.name.Contains("[SCREEN]"))
                continue;

            Debug.Log(
                "[FORMATION CONTROL AUDIT] " +
                t.name +
                " | WORLD=" + t.position +
                " | ROT=" + t.eulerAngles +
                " | LOSSY SCALE=" + t.lossyScale +
                " | PARENT=" +
                (t.parent == null ? "NONE" : t.parent.name)
            );
        }
    }
}
