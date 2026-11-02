using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class GarageNavigation : MonoBehaviour
    {
        public enum Destination
        {
            Fleet,
            Store,
            Repair,
            Customize,
            Equipment,
            Upgrade,
            AICore,
            Fabrication,
            Storage,
            Salvage,
            Deployment,
            CommandCenter
        }

        public void Navigate(Destination destination)
        {
            Debug.Log("[Garage] Navigate: " + destination);
        }
    }
}
