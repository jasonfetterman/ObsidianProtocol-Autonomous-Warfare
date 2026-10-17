using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class DeploymentFormationControlBaseFix
{
    private const string ScenePath =
        "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

    [MenuItem("Obsidian Protocol/Deployment/FIX FORMATION CONTROL BASE")]
    public static void Build()
    {
        SceneAsset sceneAsset =
            AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath);

        if (sceneAsset == null)
        {
            Debug.LogError("FORMATION BASE FIX: Scene not found: " + ScenePath);
            return;
        }

        EditorSceneManager.OpenScene(
            ScenePath,
            OpenSceneMode.Single);

        GameObject station =
            GameObject.Find("[STATION] FORMATION CONTROL");

        if (station == null)
        {
            Debug.LogError(
                "FORMATION BASE FIX: [STATION] FORMATION CONTROL not found.");
            return;
        }

        Transform old =
            station.transform.Find("[STRUCTURE] BASE DECK");

        if (old != null)
            Object.DestroyImmediate(old.gameObject);

        GameObject baseObject =
            GameObject.CreatePrimitive(PrimitiveType.Cube);

        baseObject.name =
            "[STRUCTURE] BASE DECK";

        baseObject.transform.SetParent(
            station.transform,
            false);

        baseObject.transform.localPosition =
            new Vector3(-9f, 0.15f, 7f);

        baseObject.transform.localRotation =
            Quaternion.identity;

        baseObject.transform.localScale =
            new Vector3(13f, 0.08f, 5.5f);

        EditorSceneManager.SaveScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());

        Debug.Log(
            "FORMATION BASE FIX: BASE DECK CREATED at (-9, 0.15, 7) scale (13, 0.08, 5.5).");
    }
}
