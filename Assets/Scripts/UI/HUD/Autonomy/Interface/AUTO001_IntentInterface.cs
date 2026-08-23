using UnityEngine;

namespace ObsidianProtocol.UI.HUD.Autonomy
{
    /// <summary>
    /// AUTO-001 - Intent interface
    /// </summary>
    public class AUTO001_IntentInterface : MonoBehaviour
    {
        public const string ID = "AUTO-001";

        public void Initialize()
        {
            Debug.Log("[Autonomy HUD] AUTO-001 initialized.");
        }
    }
}
