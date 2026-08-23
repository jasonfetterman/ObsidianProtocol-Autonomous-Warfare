using UnityEngine;

namespace ObsidianProtocol.UI.Popups.Combat
{
    /// <summary>
    /// POP-002 - Enemy lost
    /// </summary>
    public class POP002_EnemyLost : MonoBehaviour
    {
        public const string ID = "POP-002";

        public void Initialize()
        {
            Debug.Log("[Popups.Combat] POP-002 initialized.");
        }
    }
}
