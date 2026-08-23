using UnityEngine;

namespace ObsidianProtocol.UI.Notifications
{
    /// <summary>
    /// NOTIF-008 - Objective completed
    /// </summary>
    public class NOTIF008_ObjectiveCompleted : MonoBehaviour
    {
        public const string ID = "NOTIF-008";

        public void Initialize()
        {
            Debug.Log("[Notification UI] NOTIF-008 initialized.");
        }
    }
}
