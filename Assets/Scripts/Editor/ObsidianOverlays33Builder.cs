#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ObsidianProtocol.Editor
{
    public static class ObsidianOverlays33Builder
    {
        [MenuItem("Obsidian Protocol/UI/Build PHASE 33 - Overlays")]
        public static void Build()
        {
            GameObject hud = GameObject.Find("HUD");

            if (hud == null)
            {
                Debug.LogError("[UI] HUD GameObject not found.");
                return;
            }

            Transform overlays =
                GetOrCreate("Overlays", hud.transform);

            // =================================================
            // VISIBILITY
            // =================================================

            Transform visibility =
                GetOrCreate("Visibility", overlays);

            CreateNode(
                "OVER-001 Fog of War Overlay",
                visibility
            );

            // =================================================
            // SENSORS
            // =================================================

            Transform sensors =
                GetOrCreate("Sensors", overlays);

            CreateNode(
                "OVER-002 Sensor Coverage Overlay",
                sensors
            );

            // =================================================
            // COMBAT
            // =================================================

            Transform combat =
                GetOrCreate("Combat", overlays);

            CreateNode(
                "OVER-003 Threat Overlay",
                combat
            );

            CreateNode(
                "OVER-004 Range Overlay",
                combat
            );

            CreateNode(
                "OVER-005 Weapon Range Overlay",
                combat
            );

            // =================================================
            // MOVEMENT
            // =================================================

            Transform movement =
                GetOrCreate("Movement", overlays);

            CreateNode(
                "OVER-006 Movement Range Overlay",
                movement
            );

            CreateNode(
                "OVER-007 Path Overlay",
                movement
            );

            // =================================================
            // FORMATION
            // =================================================

            Transform formation =
                GetOrCreate("Formation", overlays);

            CreateNode(
                "OVER-008 Formation Overlay",
                formation
            );

            // =================================================
            // ZONES
            // =================================================

            Transform zones =
                GetOrCreate("Zones", overlays);

            CreateNode(
                "OVER-009 Defensive Zone Overlay",
                zones
            );

            CreateNode(
                "OVER-010 Objective Zone Overlay",
                zones
            );

            // =================================================
            // RESOURCES
            // =================================================

            Transform resources =
                GetOrCreate("Resources", overlays);

            CreateNode(
                "OVER-011 Resource Overlay",
                resources
            );

            // =================================================
            // LOGISTICS
            // =================================================

            Transform logistics =
                GetOrCreate("Logistics", overlays);

            CreateNode(
                "OVER-012 Logistics Overlay",
                logistics
            );

            // =================================================
            // AIRSPACE
            // =================================================

            Transform airspace =
                GetOrCreate("Airspace", overlays);

            CreateNode(
                "OVER-013 Airspace Overlay",
                airspace
            );

            // =================================================
            // NAVAL
            // =================================================

            Transform naval =
                GetOrCreate("Naval", overlays);

            CreateNode(
                "OVER-014 Naval Overlay",
                naval
            );

            // =================================================
            // COMMAND
            // =================================================

            Transform command =
                GetOrCreate("Command", overlays);

            CreateNode(
                "OVER-015 Command Network Overlay",
                command
            );

            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                hud.scene
            );

            Selection.activeGameObject =
                overlays.gameObject;

            Debug.Log(
                "[UI] PHASE 33 - OVERLAYS hierarchy built successfully."
            );
        }

        private static Transform GetOrCreate(
            string objectName,
            Transform parent
        )
        {
            Transform existing =
                parent.Find(objectName);

            if (existing != null)
            {
                return existing;
            }

            GameObject obj =
                new GameObject(objectName);

            obj.transform.SetParent(
                parent,
                false
            );

            return obj.transform;
        }

        private static void CreateNode(
            string objectName,
            Transform parent
        )
        {
            if (parent.Find(objectName) != null)
            {
                return;
            }

            GameObject obj =
                new GameObject(objectName);

            obj.transform.SetParent(
                parent,
                false
            );
        }
    }
}
#endif
