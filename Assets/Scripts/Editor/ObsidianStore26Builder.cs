#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ObsidianProtocol.Editor
{
    public static class ObsidianStore26Builder
    {
        [MenuItem("Obsidian Protocol/UI/Build PHASE 26 - Store")]
        public static void Build()
        {
            GameObject hud = GameObject.Find("HUD");

            if (hud == null)
            {
                Debug.LogError("[UI] HUD GameObject not found.");
                return;
            }

            Transform store = GetOrCreate("Store", hud.transform);

            // =================================================
            // STORE MAIN
            // =================================================

            Transform main = GetOrCreate("Main", store);

            CreateNode("STORE-001 Store Home", main);
            CreateNode("STORE-002 Featured", main);

            // =================================================
            // CATEGORIES
            // =================================================

            Transform categories = GetOrCreate("Categories", store);

            CreateNode("STORE-003 Units", categories);
            CreateNode("STORE-004 Equipment", categories);
            CreateNode("STORE-005 Materials", categories);
            CreateNode("STORE-006 Cosmetics", categories);
            CreateNode("STORE-007 Garage Slots", categories);
            CreateNode("STORE-008 Convenience Items", categories);
            CreateNode("STORE-009 Campaign Resources", categories);
            CreateNode("STORE-010 Fabrication Materials", categories);
            CreateNode("STORE-011 Credits", categories);

            // =================================================
            // PURCHASING
            // =================================================

            Transform purchasing = GetOrCreate("Purchasing", store);

            CreateNode("STORE-012 Item Details", purchasing);
            CreateNode("STORE-013 Purchase Confirmation", purchasing);
            CreateNode("STORE-014 Purchase Success", purchasing);
            CreateNode("STORE-015 Purchase Failure", purchasing);

            // =================================================
            // OWNERSHIP
            // =================================================

            Transform ownership = GetOrCreate("Ownership", store);

            CreateNode("STORE-016 Ownership Status", ownership);
            CreateNode("STORE-017 Unlock Requirements", ownership);

            // =================================================
            // INVENTORY
            // =================================================

            Transform inventory = GetOrCreate("Inventory", store);

            CreateNode("STORE-018 Inventory", inventory);

            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                hud.scene
            );

            Selection.activeGameObject = store.gameObject;

            Debug.Log(
                "[UI] PHASE 26 - STORE hierarchy built successfully."
            );
        }

        private static Transform GetOrCreate(
            string objectName,
            Transform parent
        )
        {
            Transform existing = parent.Find(objectName);

            if (existing != null)
            {
                return existing;
            }

            GameObject obj = new GameObject(objectName);
            obj.transform.SetParent(parent, false);

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

            GameObject obj = new GameObject(objectName);
            obj.transform.SetParent(parent, false);
        }
    }
}
#endif
