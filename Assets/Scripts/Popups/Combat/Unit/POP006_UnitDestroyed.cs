using UnityEngine;

namespace ObsidianProtocol.UI.Popups.Combat
{
    /// <summary>
    /// POP-006 - Unit destroyed
    /// </summary>
    public class POP006_UnitDestroyed : MonoBehaviour
    {
        public const string ID = "POP-006";

        public void Initialize()
        {
            Debug.Log("[Popups.Combat] POP-006 initialized.");
        }
    }
}
