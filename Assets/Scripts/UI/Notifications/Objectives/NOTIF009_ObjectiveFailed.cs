using UnityEngine;

namespace ObsidianProtocol.UI.Notifications
{
    /// <summary>
    /// NOTIF-009 - Objective failed
    /// </summary>
    public class NOTIF009_ObjectiveFailed : MonoBehaviour
    {
        public const string ID = "NOTIF-009";

        public void Initialize()
        {
            Debug.Log("[Notification UI] NOTIF-009 initialized.");
        }
    }
}
