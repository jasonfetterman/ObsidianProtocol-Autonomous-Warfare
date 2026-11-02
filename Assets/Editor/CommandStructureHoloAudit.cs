using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class CommandStructureHoloAudit
{
    [MenuItem("Obsidian Protocol/Deployment/Audit Command Structure Screens")]
    public static void Audit()
    {
        var scene = EditorSceneManager.OpenScene(
            "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity",
            OpenSceneMode.Single
        );

        string consoleName = "Console_Command_Structure_Terrminals";
        GameObject console = GameObject.Find(consoleName);

        if (console == null)
        {
            Debug.LogError("[COMMAND STRUCTURE AUDIT] CONSOLE NOT FOUND: " + consoleName);
            return;
        }

        Debug.Log(
            "[COMMAND STRUCTURE AUDIT] CONSOLE FOUND: " +
            consoleName +
            " | WORLD=" + console.transform.position +
            " | ROT=" + console.transform.eulerAngles
        );

        Transform[] children =
            console.GetComponentsInChildren<Transform>(true);

        foreach (Transform t in children)
        {
            if (!t.name.Contains("[SCREEN]"))
                continue;

            Renderer r = t.GetComponent<Renderer>();

            Debug.Log(
                "[COMMAND STRUCTURE AUDIT] " +
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
