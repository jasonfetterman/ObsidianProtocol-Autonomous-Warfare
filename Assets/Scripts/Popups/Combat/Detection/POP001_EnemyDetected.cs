using UnityEngine;

namespace ObsidianProtocol.UI.Popups.Combat
{
    /// <summary>
    /// POP-001 - Enemy detected
    /// </summary>
    public class POP001_EnemyDetected : MonoBehaviour
    {
        public const string ID = "POP-001";

        public void Initialize()
        {
            Debug.Log("[Popups.Combat] POP-001 initialized.");
        }
    }
}
