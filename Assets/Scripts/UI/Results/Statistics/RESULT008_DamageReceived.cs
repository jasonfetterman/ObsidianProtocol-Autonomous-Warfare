using UnityEngine;

namespace ObsidianProtocol.UI.Results
{
    /// <summary>
    /// RESULT-008 - Damage received
    /// </summary>
    public class RESULT008_DamageReceived : MonoBehaviour
    {
        public const string ID = "RESULT-008";

        public void Initialize()
        {
            Debug.Log("[Results UI] RESULT-008 initialized.");
        }
    }
}
