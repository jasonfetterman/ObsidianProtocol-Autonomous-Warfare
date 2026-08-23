#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ObsidianProtocol.Editor
{
    public static class ObsidianTutorial31Builder
    {
        [MenuItem("Obsidian Protocol/UI/Build PHASE 31 - Tutorial")]
        public static void Build()
        {
            GameObject hud = GameObject.Find("HUD");

            if (hud == null)
            {
                Debug.LogError("[UI] HUD GameObject not found.");
                return;
            }

            Transform tutorial =
                GetOrCreate("Tutorial", hud.transform);

            // =================================================
            // ONBOARDING
            // =================================================

            Transform onboarding =
                GetOrCreate("Onboarding", tutorial);

            CreateNode(
                "TUT-001 First Launch Onboarding",
                onboarding
            );

            // =================================================
            // CONTROLS
            // =================================================

            Transform controls =
                GetOrCreate("Controls", tutorial);

            CreateNode(
                "TUT-002 Basic Controls",
                controls
            );

            CreateNode(
                "TUT-003 Camera Tutorial",
                controls
            );

            // =================================================
            // RTS
            // =================================================

            Transform rts =
                GetOrCreate("RTS", tutorial);

            CreateNode(
                "TUT-004 Unit Selection Tutorial",
                rts
            );

            CreateNode(
                "TUT-005 Command Tutorial",
                rts
            );

            CreateNode(
                "TUT-007 Squad Tutorial",
                rts
            );

            // =================================================
            // AUTONOMY
            // =================================================

            Transform autonomy =
                GetOrCreate("Autonomy", tutorial);

            CreateNode(
                "TUT-006 Intent Tutorial",
                autonomy
            );

            CreateNode(
                "TUT-008 Autonomy Tutorial",
                autonomy
            );

            // =================================================
            // COMBAT
            // =================================================

            Transform combat =
                GetOrCreate("Combat", tutorial);

            CreateNode(
                "TUT-009 Combat Tutorial",
                combat
            );

            // =================================================
            // LOGISTICS
            // =================================================

            Transform logistics =
                GetOrCreate("Logistics", tutorial);

            CreateNode(
                "TUT-010 Logistics Tutorial",
                logistics
            );

            // =================================================
            // DEPLOYMENT
            // =================================================

            Transform deployment =
                GetOrCreate("Deployment", tutorial);

            CreateNode(
                "TUT-011 Deployment Budget Tutorial",
                deployment
            );

            // =================================================
            // VR
            // =================================================

            Transform vr =
                GetOrCreate("VR", tutorial);

            CreateNode(
                "TUT-012 VR Tutorial",
                vr
            );

            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                hud.scene
            );

            Selection.activeGameObject =
                tutorial.gameObject;

            Debug.Log(
                "[UI] PHASE 31 - TUTORIAL hierarchy built successfully."
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
