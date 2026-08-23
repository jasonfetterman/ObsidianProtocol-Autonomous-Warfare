using UnityEngine;

namespace ObsidianProtocol.UI.Popups.System
{
    /// <summary>
    /// POP-028 - Connection restored
    /// </summary>
    public class POP028_ConnectionRestored : MonoBehaviour
    {
        public const string ID = "POP-028";

        public void Initialize()
        {
            Debug.Log("[System Popup] POP-028 initialized.");
        }
    }
}
