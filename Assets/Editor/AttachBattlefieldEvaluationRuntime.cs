using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using ObsidianProtocol.Game.AI.Commanders;

public static class AttachBattlefieldEvaluationRuntime
{
    [MenuItem("Obsidian Protocol/Fix/Attach Battlefield Evaluation Runtime")]
    public static void Attach()
    {
        BattlefieldEvaluationRuntime existing =
            Object.FindFirstObjectByType<BattlefieldEvaluationRuntime>();

        if (existing != null)
        {
            Debug.Log("[BATTLEFIELD EVALUATION] Already attached.");
            return;
        }

        GameObject root =
            new GameObject("[SYSTEM] BATTLEFIELD EVALUATION RUNTIME");

        root.AddComponent<BattlefieldEvaluationRuntime>();

        Scene scene = SceneManager.GetActiveScene();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[BATTLEFIELD EVALUATION] Runtime attached to active scene.");
    }
}
