using UnityEngine;

namespace ObsidianProtocol.UI.Notifications
{
    /// <summary>
    /// NOTIF-013 - Mission unlocked
    /// </summary>
    public class NOTIF013_MissionUnlocked : MonoBehaviour
    {
        public const string ID = "NOTIF-013";

        public void Initialize()
        {
            Debug.Log("[Notification UI] NOTIF-013 initialized.");
        }
    }
}
