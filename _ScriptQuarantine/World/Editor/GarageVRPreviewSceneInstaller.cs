#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace ObsidianProtocol.World.Editor
{
    public static class GarageVRPreviewSceneInstaller
    {
        [MenuItem(
            "Obsidian Protocol/World/Install Garage VR Preview")]
        public static void Install()
        {
            GameObject worldRoot =
                FindOrCreate(
                    "OBSIDIAN_WORLD");

            GameObject garageRoot =
                FindOrCreateChild(
                    worldRoot.transform,
                    "GARAGE_WORLD");

            GameObject previewRoot =
                FindOrCreateChild(
                    garageRoot.transform,
                    "GARAGE_VR_PREVIEW");

            GameObject entryObject =
                FindOrCreateChild(
                    previewRoot.transform,
                    "GARAGE_ENTRY");

            GameObject anchorObject =
                FindOrCreateChild(
                    previewRoot.transform,
                    "VR_PREVIEW_ANCHOR");

            GameObject playerAnchorObject =
                FindOrCreateChild(
                    anchorObject.transform,
                    "PLAYER_ANCHOR");

            GameObject lookTargetObject =
                FindOrCreateChild(
                    anchorObject.transform,
                    "LOOK_TARGET");

            GameObject markerObject =
                FindOrCreateChild(
                    previewRoot.transform,
                    "VR_PREVIEW_MARKER");

            if (entryObject.GetComponent<
                    WorldPlayerEntryPoint>() == null)
            {
                entryObject.AddComponent<
                    WorldPlayerEntryPoint>();
            }

            GarageVRPreviewAnchor anchor =
                anchorObject.GetComponent<
                    GarageVRPreviewAnchor>();

            if (anchor == null)
            {
                anchor =
                    anchorObject.AddComponent<
                        GarageVRPreviewAnchor>();
            }

            GarageVRPreviewMarker marker =
                markerObject.GetComponent<
                    GarageVRPreviewMarker>();

            if (marker == null)
            {
                marker =
                    markerObject.AddComponent<
                        GarageVRPreviewMarker>();
            }

            SetTransform(
                playerAnchorObject.transform,
                Vector3.zero,
                Quaternion.identity);

            SetTransform(
                lookTargetObject.transform,
                new Vector3(0f, 1.5f, 4f),
                Quaternion.identity);

            SetTransform(
                anchorObject.transform,
                Vector3.zero,
                Quaternion.identity);

            SetTransform(
                markerObject.transform,
                Vector3.zero,
                Quaternion.identity);

            anchorObject.transform.SetParent(
                previewRoot.transform,
                true);

            playerAnchorObject.transform.SetParent(
                anchorObject.transform,
                false);

            lookTargetObject.transform.SetParent(
                anchorObject.transform,
                false);

            EditorUtility.SetDirty(
                entryObject);

            EditorUtility.SetDirty(
                anchorObject);

            EditorUtility.SetDirty(
                markerObject);

            Selection.activeGameObject =
                previewRoot;

            Debug.Log(
                "GARAGE VR PREVIEW SCENE HIERARCHY CREATED.");

            Debug.Log(
                "NEXT: ASSIGN THE ACTUAL VR PLAYER ROOT IN THE INSPECTOR.");
        }

        private static GameObject FindOrCreate(
            string objectName)
        {
            GameObject existing =
                GameObject.Find(objectName);

            if (existing != null)
                return existing;

            return new GameObject(objectName);
        }

        private static GameObject FindOrCreateChild(
            Transform parent,
            string objectName)
        {
            Transform existing =
                parent.Find(objectName);

            if (existing != null)
                return existing.gameObject;

            GameObject child =
                new GameObject(objectName);

            child.transform.SetParent(
                parent,
                false);

            return child;
        }

        private static void SetTransform(
            Transform target,
            Vector3 position,
            Quaternion rotation)
        {
            target.localPosition = position;
            target.localRotation = rotation;
            target.localScale = Vector3.one;
        }
    }
}

#endif
