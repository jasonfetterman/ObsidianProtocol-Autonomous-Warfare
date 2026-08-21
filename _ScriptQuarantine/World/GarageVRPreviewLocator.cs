using UnityEngine;

namespace ObsidianProtocol.World
{
    public class GarageVRPreviewLocator : MonoBehaviour
    {
        [Header("Preview")]
        [SerializeField]
        private GarageVRPreviewAnchor previewAnchor;

        [SerializeField]
        private GarageVRPreviewMarker previewMarker;

        [Header("World Entry")]
        [SerializeField]
        private WorldPlayerEntryPoint playerEntryPoint;

        public GarageVRPreviewAnchor PreviewAnchor =>
            previewAnchor;

        public GarageVRPreviewMarker PreviewMarker =>
            previewMarker;

        public WorldPlayerEntryPoint PlayerEntryPoint =>
            playerEntryPoint;

        public bool IsReady =>
            previewAnchor != null &&
            previewMarker != null &&
            playerEntryPoint != null;

        private void Awake()
        {
            LocatePreviewObjects();
        }

        public bool LocatePreviewObjects()
        {
            if (previewAnchor == null)
            {
                previewAnchor =
                    FindAnyObjectByType<
                        GarageVRPreviewAnchor>();
            }

            if (previewMarker == null)
            {
                previewMarker =
                    FindAnyObjectByType<
                        GarageVRPreviewMarker>();
            }

            if (playerEntryPoint == null)
            {
                WorldPlayerEntryPoint[] entries =
                    FindObjectsByType<
                        WorldPlayerEntryPoint>(FindObjectsInactive.Include);

                foreach (
                    WorldPlayerEntryPoint entry
                    in entries)
                {
                    if (entry == null)
                        continue;

                    if (entry.WorldId ==
                        "WORLD_01")
                    {
                        playerEntryPoint =
                            entry;

                        break;
                    }
                }
            }

            return IsReady;
        }

        public Vector3 GetPreviewPosition()
        {
            if (previewAnchor == null)
                return Vector3.zero;

            return previewAnchor.Position;
        }

        public Quaternion GetPreviewRotation()
        {
            if (previewAnchor == null)
                return Quaternion.identity;

            return previewAnchor.Rotation;
        }

        public bool SupportsVR()
        {
            return playerEntryPoint != null &&
                   playerEntryPoint.Supports(
                       WorldControlMode.VR);
        }
    }
}


