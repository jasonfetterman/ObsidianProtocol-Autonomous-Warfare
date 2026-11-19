using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class BlackspireClearPreplacedUnits
{
    [MenuItem("Obsidian Protocol/Battlefield/CLEAR PRE-PLACED BLACKSPIRE UNITS")]
    private static void ClearUnits()
    {
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

        if (!scene.IsValid() || !scene.isLoaded ||
            !scene.name.Contains("SCN03_BATTLEFIELD_AAA"))
        {
            EditorUtility.DisplayDialog(
                "Wrong Scene",
                "Open SCN03_BATTLEFIELD_AAA before running this tool.",
                "OK");
            return;
        }

        if (!EditorUtility.DisplayDialog(
            "Clear Pre-Placed Units",
            "Remove all children from BLACKSPIRE_FIELD_UNITS and BLACKSPIRE_AIR_UNITS?\n\nThe containers and all other scene objects will remain.",
            "Clear Units",
            "Cancel"))
            return;

        string[] containerNames =
        {
            "BLACKSPIRE_FIELD_UNITS",
            "BLACKSPIRE_AIR_UNITS"
        };

        int removed = 0;

        foreach (string containerName in containerNames)
        {
            GameObject container = GameObject.Find(containerName);

            if (container == null)
            {
                Debug.LogWarning("Container not found: " + containerName);
                continue;
            }

            while (container.transform.childCount > 0)
            {
                Transform child = container.transform.GetChild(0);
                Undo.DestroyObjectImmediate(child.gameObject);
                removed++;
            }
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("BLACKSPIRE RESET: Removed " + removed +
                  " pre-placed unit instances. Containers preserved.");
        EditorUtility.DisplayDialog(
            "Blackspire Reset Complete",
            "Removed " + removed + " pre-placed unit instances.\nThe battlefield environment and containers were preserved.",
            "OK");
    }
}
