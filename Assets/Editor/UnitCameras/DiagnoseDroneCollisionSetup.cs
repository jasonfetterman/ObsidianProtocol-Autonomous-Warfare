using ObsidianProtocol.Game.AirWarfare;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public static class DiagnoseDroneCollisionSetup
{
    [MenuItem("Obsidian Protocol/Unit Cameras/Diagnose Drone Collision Setup")]
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
            Rigidbody body = driver.GetComponent<Rigidbody>();
            Collider[] colliders = driver.GetComponentsInChildren<Collider>(true);

            Debug.Log(
                $"[DRONE COLLISION] ROOT={driver.name} " +
                $"Scene={driver.gameObject.scene.name} " +
                $"Layer={LayerMask.LayerToName(driver.gameObject.layer)} " +
                $"Rigidbody={(body != null)} " +
                $"Kinematic={(body != null && body.isKinematic)} " +
                $"DetectCollisions={(body != null && body.detectCollisions)} " +
                $"Mode={(body != null ? body.collisionDetectionMode.ToString() : "NONE")} " +
                $"ColliderCount={colliders.Length}", driver);

            foreach (Collider c in colliders)
            {
                if (c == null) continue;

                Debug.Log(
                    $"[DRONE COLLISION] UNIT_COLLIDER={GetPath(c.transform, driver.transform)} " +
                    $"Type={c.GetType().Name} Enabled={c.enabled} " +
                    $"Trigger={c.isTrigger} Active={c.gameObject.activeInHierarchy} " +
                    $"Layer={LayerMask.LayerToName(c.gameObject.layer)} " +
                    $"BoundsSize={c.bounds.size}", c);
            }
        }

        int worldCount = 0;
        foreach (var scene in GetLoadedScenes())
        {
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                Collider[] all = root.GetComponentsInChildren<Collider>(true);

                foreach (Collider c in all)
                {
                    if (c == null || !c.enabled || !c.gameObject.activeInHierarchy)
                        continue;

                    Rigidbody owner = c.attachedRigidbody;
                    bool isDrone = owner != null &&
                        owner.GetComponent<ScoutEyeFlightBehaviour>() != null;

                    if (isDrone) continue;
                    worldCount++;

                    Vector3 size = c.bounds.size;
                    bool largeSurface = size.x > 20f || size.z > 20f;
                    bool namedSurface = Regex.IsMatch(
                        c.name, "terrain|ground|map|floor|surface|landscape",
                        RegexOptions.IgnoreCase);

                    if (largeSurface || namedSurface || c is TerrainCollider)
                    {
                        Debug.Log(
                            $"[WORLD COLLIDER] Scene={scene.name} " +
                            $"Object={GetPath(c.transform, root.transform)} " +
                            $"Type={c.GetType().Name} Trigger={c.isTrigger} " +
                            $"Layer={LayerMask.LayerToName(c.gameObject.layer)} " +
                            $"BoundsSize={size}", c);
                    }
                }
            }
        }

        Debug.Log(
            $"[DRONE COLLISION] Diagnostic complete. " +
            $"Drone roots found={found}; active non-drone colliders={worldCount}. " +
            "No scene objects were changed.");
    }

    private static System.Collections.Generic.List<Scene> GetLoadedScenes()
    {
        var scenes = new System.Collections.Generic.List<Scene>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.isLoaded) scenes.Add(scene);
        }
        return scenes;
    }

    private static string GetPath(Transform item, Transform root)
    {
        string path = item.name;
        while (item.parent != null && item.parent != root)
        {
            item = item.parent;
            path = item.name + "/" + path;
        }
        return item.parent == root ? root.name + "/" + path : path;
    }
}
