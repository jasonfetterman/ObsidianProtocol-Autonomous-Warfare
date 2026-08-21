using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class FleetBootstrap : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField]
        private GarageManager garageManager;

        [SerializeField]
        private FleetManager fleetManager;

        private void Awake()
        {
            InitializeManagers();
        }

        private void InitializeManagers()
        {
            if (garageManager == null)
            {
                garageManager = GarageManager.Instance;
            }

            if (fleetManager == null)
            {
                fleetManager = FleetManager.Instance;
            }

            if (garageManager == null)
            {
                Debug.LogError(
                    "FleetBootstrap could not find a GarageManager."
                );
            }

            if (fleetManager == null)
            {
                Debug.LogError(
                    "FleetBootstrap could not find a FleetManager."
                );
            }
        }
    }
}
