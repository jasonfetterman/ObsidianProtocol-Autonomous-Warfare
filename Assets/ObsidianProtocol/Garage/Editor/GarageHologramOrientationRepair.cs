using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public static class GarageHologramOrientationRepair
{
    [MenuItem("Obsidian Protocol/Garage/Stand All Hologram Signs Upright")]
    public static void Repair()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (!scene.IsValid() ||
            !scene.path.Replace("\\", "/").EndsWith("/SCN04_GARAGE.unity"))
        {
            EditorUtility.DisplayDialog(
                "Hologram Orientation",
                "Open SCN04_GARAGE first.", "OK");
            return;
        }

        GameObject root = GameObject.Find("GARAGE_HOLOGRAPHIC_SIGNAGE");
        if (root == null)
        {
            EditorUtility.DisplayDialog(
                "Hologram Orientation",
                "Hologram signage root not found.", "OK");
            return;
        }

        int count = 0;

        foreach (Transform sign in root.transform)
        {
            if (sign == null || !sign.name.StartsWith("HOLO_SIGN_"))
                continue;

            Undo.RecordObject(sign, "Stand Hologram Sign Upright");

            // Keep the existing location; orient the sign vertically
            // with its front facing toward the garage's forward direction.
            sign.rotation = Quaternion.Euler(0f, sign.eulerAngles.y, 0f);

            count++;
            EditorUtility.SetDirty(sign);
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("HOLOGRAM ORIENTATION: Set " + count + " signs upright.");
        EditorUtility.DisplayDialog(
            "Hologram Orientation",
            "Adjusted " + count + " signs. Positions and child objects were preserved.",
            "OK");
    }
}
