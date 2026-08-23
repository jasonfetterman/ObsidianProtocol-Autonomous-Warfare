using UnityEngine;

namespace ObsidianProtocol.UI.Popups.Combat
{
    /// <summary>
    /// POP-003 - Target acquired
    /// </summary>
    public class POP003_TargetAcquired : MonoBehaviour
    {
        public const string ID = "POP-003";

        public void Initialize()
        {
            Debug.Log("[Popups.Combat] POP-003 initialized.");
        }
    }
}
