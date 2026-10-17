#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

[InitializeOnLoad]
public static class SetupEventSystem
{
    [InitializeOnLoadMethod]
    private static void EnsureEventSystem()
    {
        EditorApplication.delayCall += () =>
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;

            if (Object.FindAnyObjectByType<EventSystem>() == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<InputSystemUIInputModule>();
                Undo.RegisterCreatedObjectUndo(eventSystem, "Create EventSystem");
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(eventSystem.scene);
            }
        };
    }
}
#endif
