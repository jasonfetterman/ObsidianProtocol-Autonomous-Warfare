using UnityEngine;

namespace ObsidianProtocol.UI.Notifications
{
    /// <summary>
    /// NOTIF-005 - Unit destroyed
    /// </summary>
    public class NOTIF005_UnitDestroyed : MonoBehaviour
    {
        public const string ID = "NOTIF-005";

        public void Initialize()
        {
            Debug.Log("[Notification UI] NOTIF-005 initialized.");
        }
    }
}
