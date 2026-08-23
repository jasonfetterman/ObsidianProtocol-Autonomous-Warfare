using UnityEngine;

namespace ObsidianProtocol.UI.Notifications
{
    /// <summary>
    /// NOTIF-004 - Unit deployed
    /// </summary>
    public class NOTIF004_UnitDeployed : MonoBehaviour
    {
        public const string ID = "NOTIF-004";

        public void Initialize()
        {
            Debug.Log("[Notification UI] NOTIF-004 initialized.");
        }
    }
}
