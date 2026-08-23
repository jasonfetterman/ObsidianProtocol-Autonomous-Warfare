using UnityEngine;

namespace ObsidianProtocol.UI.Popups.System
{
    /// <summary>
    /// POP-027 - Connection lost
    /// </summary>
    public class POP027_ConnectionLost : MonoBehaviour
    {
        public const string ID = "POP-027";

        public void Initialize()
        {
            Debug.Log("[System Popup] POP-027 initialized.");
        }
    }
}
