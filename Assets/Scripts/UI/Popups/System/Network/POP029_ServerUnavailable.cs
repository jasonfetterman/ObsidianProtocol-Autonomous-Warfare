using UnityEngine;

namespace ObsidianProtocol.UI.Popups.System
{
    /// <summary>
    /// POP-029 - Server unavailable
    /// </summary>
    public class POP029_ServerUnavailable : MonoBehaviour
    {
        public const string ID = "POP-029";

        public void Initialize()
        {
            Debug.Log("[System Popup] POP-029 initialized.");
        }
    }
}
