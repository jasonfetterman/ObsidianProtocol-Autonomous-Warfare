using UnityEngine;

namespace ObsidianProtocol.UI.Notifications
{
    /// <summary>
    /// NOTIF-007 - Squad destroyed
    /// </summary>
    public class NOTIF007_SquadDestroyed : MonoBehaviour
    {
        public const string ID = "NOTIF-007";

        public void Initialize()
        {
            Debug.Log("[Notification UI] NOTIF-007 initialized.");
        }
    }
}
