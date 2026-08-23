using UnityEngine;

namespace ObsidianProtocol.UI.Notifications
{
    /// <summary>
    /// NOTIF-002 - Resource depleted
    /// </summary>
    public class NOTIF002_ResourceDepleted : MonoBehaviour
    {
        public const string ID = "NOTIF-002";

        public void Initialize()
        {
            Debug.Log("[Notification UI] NOTIF-002 initialized.");
        }
    }
}
