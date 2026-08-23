#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ObsidianProtocol.Editor
{
    public static class ObsidianVR30Builder
    {
        [MenuItem("Obsidian Protocol/UI/Build PHASE 30 - VR Operator")]
        public static void Build()
        {
            GameObject hud = GameObject.Find("HUD");

            if (hud == null)
            {
                Debug.LogError("[UI] HUD GameObject not found.");
                return;
            }

            // IMPORTANT:
            // VR Operator is intentionally separate from the normal RTS HUD.
            Transform vr = GetOrCreate("VR Operator", hud.transform);

            // =================================================
            // OPERATOR
            // =================================================

            Transform operatorRoot =
                GetOrCreate("Operator", vr);

            CreateNode("VR-001 Operator Entry", operatorRoot);
            CreateNode("VR-002 Operator Selection", operatorRoot);
            CreateNode("VR-003 Machine Compatibility", operatorRoot);
            CreateNode("VR-004 Enter Machine", operatorRoot);
            CreateNode("VR-005 Exit Machine", operatorRoot);

            // =================================================
            // COCKPIT
            // =================================================

            Transform cockpit =
                GetOrCreate("Cockpit", vr);

            CreateNode("VR-006 Cockpit HUD", cockpit);
            CreateNode("VR-007 Vehicle Status", cockpit);
            CreateNode("VR-008 Health", cockpit);
            CreateNode("VR-009 Energy", cockpit);
            CreateNode("VR-010 Fuel", cockpit);
            CreateNode("VR-011 Ammunition", cockpit);

            // =================================================
            // WEAPONS
            // =================================================

            Transform weapons =
                GetOrCreate("Weapons", vr);

            CreateNode("VR-012 Targeting", weapons);
            CreateNode("VR-013 Weapon Selection", weapons);

            // =================================================
            // SENSORS
            // =================================================

            Transform sensors =
                GetOrCreate("Sensors", vr);

            CreateNode("VR-014 Sensor Controls", sensors);
            CreateNode("VR-015 Radar", sensors);
            CreateNode("VR-016 Thermal", sensors);
            CreateNode("VR-017 Lidar", sensors);

            // =================================================
            // TACTICAL
            // =================================================

            Transform tactical =
                GetOrCreate("Tactical", vr);

            CreateNode("VR-018 Tactical Map", tactical);

            // =================================================
            // COMMUNICATIONS
            // =================================================

            Transform communications =
                GetOrCreate("Communications", vr);

            CreateNode("VR-019 Communications", communications);
            CreateNode("VR-020 Squad Status", communications);
            CreateNode("VR-022 Operator Transfer", communications);

            // =================================================
            // EMERGENCY
            // =================================================

            Transform emergency =
                GetOrCreate("Emergency", vr);

            CreateNode("VR-021 Emergency Systems", emergency);

            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                hud.scene
            );

            Selection.activeGameObject =
                vr.gameObject;

            Debug.Log(
                "[UI] PHASE 30 - VR OPERATOR hierarchy built successfully."
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
