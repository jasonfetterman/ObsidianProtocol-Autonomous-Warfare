using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class RepairBlackspireStoreMaterials
{
    private const string SourcePath = "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity";

    [MenuItem("Obsidian Protocol/Store/Repair Blackspire Missing Materials")]
    private static void Run()
    {
        Scene targetScene = SceneManager.GetActiveScene();

        if (targetScene.path != "Assets/Scenes/SCN30_ACHERON/Blackspire_Continent.unity")
        {
            Debug.LogError("[STORE REPAIR] Open Blackspire_Continent first. No changes made.");
            return;
        }

        GameObject target = FindInScene(targetScene, "STORE_AAA_REBUILD");
        if (!target)
        {
            Debug.LogError("[STORE REPAIR] STORE_AAA_REBUILD not found. No changes made.");
            return;
        }

        Scene sourceScene = SceneManager.GetSceneByPath(SourcePath);
        bool openedHere = !sourceScene.IsValid() || !sourceScene.isLoaded;

        if (openedHere)
            sourceScene = EditorSceneManager.OpenScene(SourcePath, OpenSceneMode.Additive);

        try
        {
            var sources = new Dictionary<string, List<Renderer>>();
            foreach (GameObject root in sourceScene.GetRootGameObjects())
            {
                foreach (Renderer r in root.GetComponentsInChildren<Renderer>(true))
                {
                    string key = r.name + "|" + MeshKey(r);
                    if (!sources.TryGetValue(key, out var list))
                        sources[key] = list = new List<Renderer>();
                    list.Add(r);
                }
            }

            int repaired = 0, unresolved = 0;
            var changed = new HashSet<Renderer>();

            foreach (Renderer dst in target.GetComponentsInChildren<Renderer>(true))
            {
                Material[] current = dst.sharedMaterials;
                bool dirty = false;
                string key = dst.name + "|" + MeshKey(dst);

                for (int i = 0; i < current.Length; i++)
                {
                    if (current[i]) continue;

                    if (!sources.TryGetValue(key, out var candidates))
                    {
                        unresolved++;
                        continue;
                    }

                    Material replacement = null;
                    foreach (Renderer src in candidates)
                    {
                        Material[] sourceMaterials = src.sharedMaterials;
                        if (i < sourceMaterials.Length && sourceMaterials[i])
                        {
                            if (replacement && replacement != sourceMaterials[i])
                            {
                                replacement = null;
                                break;
                            }
                            replacement = sourceMaterials[i];
                        }
                    }

                    if (!replacement)
                    {
                        unresolved++;
                        continue;
                    }

                    current[i] = replacement;
                    repaired++;
                    dirty = true;
                }

                if (dirty)
                {
                    Undo.RecordObject(dst, "Repair Store Missing Materials");
                    dst.sharedMaterials = current;
                    PrefabUtility.RecordPrefabInstancePropertyModifications(dst);
                    EditorUtility.SetDirty(dst);
                    changed.Add(dst);
                }
            }

            if (repaired > 0)
            {
                EditorSceneManager.MarkSceneDirty(targetScene);
                if (!EditorSceneManager.SaveScene(targetScene))
                    Debug.LogError("[STORE REPAIR] Scene save failed. Check the scene's dirty state.");
            }

            Debug.Log($"[STORE REPAIR] Filled={repaired}; unresolved empty slots={unresolved}; changed renderers={changed.Count}; scene={targetScene.path}");
        }
        finally
        {
            if (openedHere)
                EditorSceneManager.CloseScene(sourceScene, true);
        }
    }

    private static GameObject FindInScene(Scene scene, string name)
    {
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            if (root.name == name) return root;
            foreach (Transform t in root.GetComponentsInChildren<Transform>(true))
                if (t.name == name) return t.gameObject;
        }
        return null;
    }

    private static string MeshKey(Renderer r)
    {
        if (r is SkinnedMeshRenderer skinned)
            return skinned.sharedMesh ? skinned.sharedMesh.name : "(no-mesh)";
        if (r.TryGetComponent<MeshFilter>(out var filter))
            return filter.sharedMesh ? filter.sharedMesh.name : "(no-mesh)";
        return r.GetType().Name;
    }
}
