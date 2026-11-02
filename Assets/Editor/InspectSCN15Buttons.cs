using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class InspectSCN15Buttons
{
    [MenuItem("Obsidian Protocol/Debug/Inspect SCN15 Buttons")]
    public static void Inspect()
    {
        var scene = SceneManager.GetActiveScene();
        Debug.Log("===== SCN15 BUTTON INSPECTION =====");

        foreach (var b in Object.FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (b.name != "CANCEL" && b.name != "CONFIRM EXIT")
                continue;

            Debug.Log(
                $"BUTTON: {b.name} | active={b.gameObject.activeSelf} | " +
                $"interactable={b.interactable} | listeners={b.onClick.GetPersistentEventCount()}"
            );

            for (int i = 0; i < b.onClick.GetPersistentEventCount(); i++)
            {
                Debug.Log(
                    $"  CLICK[{i}] target={b.onClick.GetPersistentTarget(i)} " +
                    $"method={b.onClick.GetPersistentMethodName(i)}"
                );
            }
        }

        Debug.Log("===== END SCN15 BUTTON INSPECTION =====");
    }
}
