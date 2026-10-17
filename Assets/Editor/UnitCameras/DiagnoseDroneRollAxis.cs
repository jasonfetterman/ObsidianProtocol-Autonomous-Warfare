using ObsidianProtocol.Game.AirWarfare;
using UnityEditor;
using UnityEngine;

public static class DiagnoseDroneRollAxis
{
    [MenuItem("Obsidian Protocol/Unit Cameras/Diagnose Drone Roll Axis")]
    private static void Run()
    {
        var drivers = Resources.FindObjectsOfTypeAll<ScoutEyeFlightBehaviour>();
        int found = 0;

        foreach (var driver in drivers)
        {
            if (driver == null ||
                !driver.gameObject.scene.IsValid() ||
                !driver.gameObject.scene.isLoaded)
                continue;

            found++;
            Transform root = driver.transform;

            Debug.Log(
                $"[DRONE AXIS] ROOT={root.name} " +
                $"Scene={root.gameObject.scene.name} " +
                $"Rotation={root.eulerAngles} " +
                $"Forward={root.forward} Right={root.right} Up={root.up}",
                driver);

            Transform[] all = root.GetComponentsInChildren<Transform>(true);

            foreach (Transform t in all)
            {
                var renderer = t.GetComponent<Renderer>();
                var meshFilter = t.GetComponent<MeshFilter>();

                if (t != root &&
                    renderer == null &&
                    meshFilter == null &&
                    t.childCount == 0)
                    continue;

                string meshSize = meshFilter != null &&
                    meshFilter.sharedMesh != null
                    ? meshFilter.sharedMesh.bounds.size.ToString("F3")
                    : "n/a";

                Vector3 localForward = root.InverseTransformDirection(t.forward);
                Vector3 localRight = root.InverseTransformDirection(t.right);
                Vector3 localUp = root.InverseTransformDirection(t.up);

                Debug.Log(
                    $"[DRONE AXIS] CHILD={GetPath(t, root)} " +
                    $"LocalPos={t.localPosition} LocalRot={t.localEulerAngles} " +
                    $"ForwardInRoot={localForward} " +
                    $"RightInRoot={localRight} UpInRoot={localUp} " +
                    $"MeshBounds={meshSize}",
                    t);
            }
        }

        if (found == 0)
            Debug.LogWarning(
                "[DRONE AXIS] No ScoutEyeFlightBehaviour found in loaded scenes.");

        Debug.Log($"[DRONE AXIS] Diagnostic finished. Drone roots found: {found}");
    }

    private static string GetPath(Transform t, Transform root)
    {
        string path = t.name;
        while (t.parent != null && t.parent != root)
        {
            t = t.parent;
            path = t.name + "/" + path;
        }

        return t.parent == root ? root.name + "/" + path : path;
    }
}
