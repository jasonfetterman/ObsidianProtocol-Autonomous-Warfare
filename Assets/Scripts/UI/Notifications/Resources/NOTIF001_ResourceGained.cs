using UnityEngine;

namespace ObsidianProtocol.UI.Notifications
{
    /// <summary>
    /// NOTIF-001 - Resource gained
    /// </summary>
    public class NOTIF001_ResourceGained : MonoBehaviour
    {
        public const string ID = "NOTIF-001";

        public void Initialize()
        {
            Debug.Log("[Notification UI] NOTIF-001 initialized.");
        }
    }
}
