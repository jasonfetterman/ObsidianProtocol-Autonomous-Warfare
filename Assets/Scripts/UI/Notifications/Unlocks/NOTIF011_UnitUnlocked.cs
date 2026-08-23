using UnityEngine;

namespace ObsidianProtocol.UI.Notifications
{
    /// <summary>
    /// NOTIF-011 - Unit unlocked
    /// </summary>
    public class NOTIF011_UnitUnlocked : MonoBehaviour
    {
        public const string ID = "NOTIF-011";

        public void Initialize()
        {
            Debug.Log("[Notification UI] NOTIF-011 initialized.");
        }
    }
}
