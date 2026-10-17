using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class StoreWallLayoutCorrection
{
    private const string ScenePath = "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity";

    [MenuItem("Obsidian Protocol/Store/Correct Side Wall Displays")]
    public static void Apply()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogError("Stop Play mode before correcting the Store.");
            return;
        }

        Scene scene = SceneManager.GetActiveScene();
        if (scene.path != ScenePath)
        {
            Debug.LogError("Open SCN16_STORE before running this correction. No changes made.");
            return;
        }

        Transform facility = FindTransform("STORE_FACILITY");
        GameObject airScreen = FindObject("AIRFRAME SYSTEMS rear display screen");
        GameObject groundScreen = FindObject("GROUND SYSTEMS rear display screen");
        GameObject airHeader = FindObject("AIRFRAME SYSTEMS screen header");
        GameObject groundHeader = FindObject("GROUND SYSTEMS screen header");
        GameObject checkout = FindObject("Checkout back wall feature");

        if (facility == null || airScreen == null || groundScreen == null ||
            airHeader == null || groundHeader == null || checkout == null)
        {
            Debug.LogError("Required Store objects were not found. No changes made.");
            return;
        }

        string backup = ScenePath + ".bak_wall_layout_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
        File.Copy(ScenePath, backup, false);

        Undo.IncrementCurrentGroup();
        int undoGroup = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName("Correct Store Side Wall Displays");

        try
        {
            string[] categories = {
                "AIR UNITS", "GROUND UNITS", "SEA UNITS",
                "COMMANDERS", "EXPERIMENTAL UNITS", "SATELLITES"
            };

            GameObject[] screens = new GameObject[6];
            GameObject[] headers = new GameObject[6];

            screens[0] = airScreen;
            screens[1] = groundScreen;
            headers[0] = airHeader;
            headers[1] = groundHeader;

            for (int i = 2; i < 6; i++)
            {
                screens[i] = UnityEngine.Object.Instantiate(airScreen, facility);
                headers[i] = UnityEngine.Object.Instantiate(airHeader, facility);
                Undo.RegisterCreatedObjectUndo(screens[i], "Create " + categories[i] + " screen");
                Undo.RegisterCreatedObjectUndo(headers[i], "Create " + categories[i] + " header");
            }

            for (int i = 0; i < 6; i++)
            {
                bool left = i < 3;
                int slot = i % 3;
                float z = new float[] { -10f, 2f, 14f }[slot];
                float x = left ? -18.12f : 18.12f;
                float yaw = left ? 90f : 270f;

                Undo.RecordObject(screens[i].transform, "Position Store screen");
                Undo.RecordObject(headers[i].transform, "Position Store header");
                Undo.RecordObject(screens[i], "Rename Store screen");
                Undo.RecordObject(headers[i], "Rename Store header");

                screens[i].name = categories[i] + " wall screen";
                screens[i].transform.SetParent(facility, false);
                screens[i].transform.localPosition = new Vector3(x, 5.15f, z);
                screens[i].transform.localRotation = Quaternion.Euler(0f, yaw, 0f);
                screens[i].transform.localScale = new Vector3(4.4f, 3.2f, 0.16f);

                headers[i].name = categories[i] + " wall header";
                headers[i].transform.SetParent(facility, false);
                headers[i].transform.localPosition = new Vector3(x, 7.0f, z);
                headers[i].transform.localRotation = Quaternion.Euler(0f, yaw, 0f);
                headers[i].transform.localScale = new Vector3(4.4f, 0.22f, 0.12f);

                EnsureHeaderLabel(headers[i], categories[i], facility, x, yaw, z);
            }

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            Undo.CollapseUndoOperations(undoGroup);

            Debug.Log("Store side-wall correction saved. Backup: " + backup +
                      ". Checkout feature screen was not moved. Front entrance wall sections were preserved.");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            Debug.LogError("Correction did not complete. Scene backup: " + backup +
                           ". Use Undo or restore the backup if needed.");
        }
    }

    private static void EnsureHeaderLabel(GameObject header, string label, Transform facility, float x, float yaw, float z)
    {
        string labelName = label + " wall header text";
        GameObject textObject = FindObject(labelName);

        if (textObject == null)
        {
            textObject = new GameObject(labelName);
            Undo.RegisterCreatedObjectUndo(textObject, "Create " + label + " header text");
        }

        textObject.transform.SetParent(facility, false);
        textObject.transform.localPosition = new Vector3(x + (x < 0 ? 0.09f : -0.09f), 7.0f, z);
        textObject.transform.localRotation = Quaternion.Euler(0f, yaw, 0f);
        textObject.transform.localScale = Vector3.one;

        TextMesh mesh = textObject.GetComponent<TextMesh>();
        if (mesh == null) mesh = Undo.AddComponent<TextMesh>(textObject);
        mesh.text = label;
        mesh.anchor = TextAnchor.MiddleCenter;
        mesh.alignment = TextAlignment.Center;
        mesh.characterSize = 0.13f;
        mesh.fontSize = 48;
        mesh.color = new Color(0.1f, 0.95f, 1f, 1f);
    }

    private static Transform FindTransform(string objectName)
    {
        GameObject go = FindObject(objectName);
        return go != null ? go.transform : null;
    }

    private static GameObject FindObject(string objectName)
    {
        foreach (GameObject go in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (go.name == objectName && go.scene.IsValid())
                return go;
        }
        return null;
    }
}
