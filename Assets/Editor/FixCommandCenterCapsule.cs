using UnityEditor;

using UnityEditor.SceneManagement;using UnityEngine;

public static class FixCommandCenterCapsule
{
    [MenuItem("Obsidian Protocol/VR/Fix Desktop Player Capsule Height")]
    public static void Fix()
    {
        var player = GameObject.Find("DESKTOP PLAYER");
        if (player == null)
        {
            Debug.LogError("DESKTOP PLAYER not found.");
            return;
        }

        var filter = player.GetComponent<MeshFilter>();
        var renderer = player.GetComponent<MeshRenderer>();

        if (filter == null || renderer == null)
        {
            Debug.LogError("Player capsule mesh or renderer not found.");
            return;
        }

        var visual = new GameObject("Player Visual");
        Undo.RegisterCreatedObjectUndo(visual, "Fix Player Capsule Height");
        visual.transform.SetParent(player.transform, false);
        visual.transform.localPosition = new Vector3(0f, 0.9f, 0f);
        visual.transform.localRotation = Quaternion.identity;
        visual.transform.localScale = Vector3.one;

        visual.AddComponent<MeshFilter>().sharedMesh = filter.sharedMesh;
        visual.AddComponent<MeshRenderer>().sharedMaterials = renderer.sharedMaterials;

        Undo.DestroyObjectImmediate(filter);
        Undo.DestroyObjectImmediate(renderer);

        EditorSceneManager.MarkSceneDirty(
            EditorSceneManager.GetActiveScene());

        Debug.Log("Capsule visual raised 0.9 units. CharacterController unchanged. Save with Ctrl+S.");
    }
}

