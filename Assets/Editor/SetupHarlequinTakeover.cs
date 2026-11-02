using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using ObsidianProtocol.Game.Units;

public static class SetupHarlequinTakeover
{
    [MenuItem("Obsidian Protocol/Unit Takeover/Setup Harlequin Mech")]
    private static void Run()
    {
        const string path = "Assets/Scenes/SCN30_ACHERON/Blackspire_Continent.unity";
        var scene = SceneManager.GetSceneByPath(path);
        bool openedHere = !scene.IsValid() || !scene.isLoaded;

        if (openedHere)
            scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);

        try
        {
            Transform mech = null;
            SkinnedMeshRenderer renderer = null;

            foreach (var root in scene.GetRootGameObjects())
            {
                foreach (var smr in root.GetComponentsInChildren<SkinnedMeshRenderer>(true))
                {
                    if (smr.sharedMesh == null ||
                        !AssetDatabase.GetAssetPath(smr.sharedMesh).Contains("autorig-7e7ecd87-v4-glb"))
                        continue;

                    mech = smr.transform;
                    while (mech.parent != null && mech.parent != root.transform)
                        mech = mech.parent;

                    renderer = smr;
                    break;
                }

                if (mech != null)
                    break;
            }

            if (mech == null || renderer == null)
            {
                Debug.LogError("[HARLEQUIN SETUP] Existing mech instance not found.");
                return;
            }

            Undo.RegisterFullObjectHierarchyUndo(mech.gameObject, "Setup Harlequin Takeover");

            const string animationAssetPath = "Assets/Art/Units/Clown Syndicate/Clown_Assault_Mech/textures/autorig-7e7ecd87-v4-glb.glb";
            var animation = mech.GetComponent<Animation>();
            if (!animation)
                animation = Undo.AddComponent<Animation>(mech.gameObject);

            AnimationClip idleClip = null;
            foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(animationAssetPath))
            {
                if (!(asset is AnimationClip clip))
                    continue;

                if (animation.GetClip(clip.name) != null)
                    animation.RemoveClip(clip.name);

                animation.AddClip(clip, clip.name);

                if (clip.name == "Idle")
                    idleClip = clip;
            }

            if (idleClip != null)
            {
                animation.clip = idleClip;
                animation.playAutomatically = true;
                animation.wrapMode = WrapMode.Loop;
                animation.Play("Idle");
                Debug.Log("[HARLEQUIN SETUP] Registered animation clips; Idle selected.");
            }
            else
            {
                Debug.LogError("[HARLEQUIN SETUP] Idle clip was not found.");
            }


            if (!mech.GetComponent<GroundUnitManualDriver>())
                Undo.AddComponent<GroundUnitManualDriver>(mech.gameObject);
Transform cameraTransform = mech.Find("UNIT CAMERA - ADJUST IN SCENE VIEW");
            if (!cameraTransform)
            {
                var cameraGo = new GameObject("UNIT CAMERA - ADJUST IN SCENE VIEW");
                Undo.RegisterCreatedObjectUndo(cameraGo, "Create Harlequin Camera");
                cameraTransform = cameraGo.transform;
                cameraTransform.SetParent(mech, false);
            }

            Transform head = null;
            foreach (Transform child in mech.GetComponentsInChildren<Transform>(true))
            {
                if (child.name == "Head")
                {
                    head = child;
                    break;
                }
            }

            cameraTransform.localPosition = new Vector3(0f, 24f, -36f);
            cameraTransform.localRotation = Quaternion.Euler(15f, 0f, 0f);

            var cam = cameraTransform.GetComponent<Camera>();
            if (!cam)
                cam = Undo.AddComponent<Camera>(cameraTransform.gameObject);

            cam.enabled = false;
            cameraTransform.gameObject.SetActive(true);

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);

            Debug.Log("[HARLEQUIN SETUP] Configured existing mech: " +
                      mech.name + " at " + mech.position, mech.gameObject);
        }
        finally
        {
            if (openedHere)
                EditorSceneManager.CloseScene(scene, true);
        }
    }
}




