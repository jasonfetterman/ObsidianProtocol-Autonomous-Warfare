using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public static class UnlockPhysicalGarage
{
    [MenuItem("Obsidian Protocol/Scene Editing/UNLOCK PHYSICAL GARAGE")]
    public static void Run()
    {
        var scene = SceneManager.GetActiveScene();

        var candidates = Resources.FindObjectsOfTypeAll<GameObject>()
            .Where(go => go.scene == scene &&
                         go.name.IndexOf("garage",
                             StringComparison.OrdinalIgnoreCase) >= 0 &&
                         !go.name.ToLowerInvariant().Contains("button") &&
                         !go.name.ToLowerInvariant().Contains("panel") &&
                         !go.name.ToLowerInvariant().Contains("hud"))
            .OrderBy(go => go.name.Equals("GARAGE",
                StringComparison.OrdinalIgnoreCase) ? 0 : 1)
            .ToArray();

        if (candidates.Length == 0)
        {
            Debug.LogError("No physical Garage object found in the active scene.");
            return;
        }

        foreach (var go in candidates)
        {
            go.SetActive(true);
            SceneVisibilityManager.instance.Show(go, true);
            SceneVisibilityManager.instance.EnablePicking(go, true);
        }

        var target = candidates[0];
        Selection.activeGameObject = target;
        EditorGUIUtility.PingObject(target);

        var view = SceneView.lastActiveSceneView;
        if (view != null)
        {
            view.FrameSelected();
            view.Focus();
        }

        Debug.Log("Garage candidates found: " + candidates.Length +
                  ". Selected: " + target.name +
                  ". Check the Hierarchy if multiple candidates exist.");
    }
}
