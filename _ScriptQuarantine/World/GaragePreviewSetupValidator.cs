using UnityEngine;

namespace ObsidianProtocol.World
{
    public class GaragePreviewSetupValidator : MonoBehaviour
    {
        [Header("Required Systems")]
        [SerializeField]
        private GarageVRPreviewLocator locator;

        [SerializeField]
        private GarageVRPreviewController controller;

        [SerializeField]
        private GarageVRPreviewSession session;

        [SerializeField]
        private WorldControlManager controlManager;

        [Header("Optional World Systems")]
        [SerializeField]
        private PersistentWorldManager worldManager;

        [SerializeField]
        private WorldEntityManager entityManager;

        public bool Validate()
        {
            bool valid = true;

            Check(
                locator != null,
                "GarageVRPreviewLocator",
                ref valid);

            Check(
                controller != null,
                "GarageVRPreviewController",
                ref valid);

            Check(
                session != null,
                "GarageVRPreviewSession",
                ref valid);

            Check(
                controlManager != null,
                "WorldControlManager",
                ref valid);

            Check(
                locator != null &&
                locator.IsReady,
                "Garage VR preview objects",
                ref valid);

            Check(
                locator != null &&
                locator.SupportsVR(),
                "VR entry point",
                ref valid);

            if (valid)
            {
                Debug.Log(
                    "GARAGE VR PREVIEW SETUP VERIFIED.");

                return true;
            }

            Debug.LogWarning(
                "GARAGE VR PREVIEW SETUP NEEDS CHECKING.");

            return false;
        }

        private void Check(
            bool condition,
            string systemName,
            ref bool valid)
        {
            if (condition)
            {
                Debug.Log(
                    $"[OK] {systemName}");
            }
            else
            {
                Debug.LogWarning(
                    $"[MISSING] {systemName}");

                valid = false;
            }
        }
    }
}
