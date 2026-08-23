using UnityEngine;

namespace ObsidianProtocol.UI.Notifications
{
    /// <summary>
    /// NOTIF-012 - Equipment unlocked
    /// </summary>
    public class NOTIF012_EquipmentUnlocked : MonoBehaviour
    {
        public const string ID = "NOTIF-012";

        public void Initialize()
        {
            Debug.Log("[Notification UI] NOTIF-012 initialized.");
        }
    }
}
