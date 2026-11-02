using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class AuditBattlefieldCommandUnits
{
    private const string ScenePath =
        "Assets/Scenes/SCN03 _BATTLEFIELD/[HUD] BATTLEFIELD HUD/Battlefield_HUD.unity";

    [MenuItem("Obsidian Protocol/Battlefield/AUDIT COMMAND UNITS")]
    public static void Audit()
    {
        EditorSceneManager.OpenScene(ScenePath);

        CommandUnit[] commandUnits =
            Object.FindObjectsByType<CommandUnit>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);

        Debug.Log(
            "[COMMAND UNIT AUDIT] COUNT = " +
            commandUnits.Length);

        foreach (CommandUnit commandUnit in commandUnits)
        {
            Debug.Log(
                "[COMMAND UNIT AUDIT] FOUND = " +
                commandUnit.gameObject.name +
                " | ACTIVE=" +
                commandUnit.gameObject.activeSelf +
                " | POSITION=" +
                commandUnit.transform.position +
                " | COMMANDING=" +
                commandUnit.CommandedUnits.Count);

            Component brain =
                commandUnit.GetComponent<
                    ObsidianProtocol.Game.Command.Autonomy.ArchiveCommandBrain>();

            Debug.Log(
                "[COMMAND UNIT AUDIT] ARCHIVE BRAIN = " +
                (brain != null ? "YES" : "NO"));
        }
    }
}
