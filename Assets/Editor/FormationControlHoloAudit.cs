using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class FormationControlHoloAudit
{
    [MenuItem("Obsidian Protocol/Deployment/Audit Formation Control Screens")]
    public static void Audit()
    {
        string scenePath = "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

        var scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        if (!scene.IsValid())
        {
            Debug.LogError("[FORMATION CONTROL AUDIT] Scene failed to open.");
            return;
        }

        GameObject console = GameObject.Find(
            "Console_Formation_ControlTerrminals"
        );

        if (console == null)
        {
            Debug.LogError(
                "[FORMATION CONTROL AUDIT] CONSOLE NOT FOUND: Console_Formation_ControlTerrminals"
            );
            return;
        }

        Debug.Log(
            $"[FORMATION CONTROL AUDIT] CONSOLE FOUND: {console.name} | " +
            $"WORLD={console.transform.position} | " +
            $"ROT={console.transform.eulerAngles}"
        );

        foreach (Transform child in console.GetComponentsInChildren<Transform>(true))
        {
            if (!child.name.Contains("[SCREEN]"))
                continue;

            Debug.Log(
                $"[FORMATION CONTROL AUDIT] {child.name} | " +
                $"WORLD={child.position} | " +
                $"ROTATION={child.eulerAngles} | " +
                $"LOSSY SCALE={child.lossyScale} | " +
                $"PARENT={child.parent?.name}"
            );
        }

        Debug.Log("[FORMATION CONTROL AUDIT] COMPLETE.");
    }
}
