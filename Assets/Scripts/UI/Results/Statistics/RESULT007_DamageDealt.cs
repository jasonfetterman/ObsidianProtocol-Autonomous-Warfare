using UnityEngine;

namespace ObsidianProtocol.UI.Results
{
    /// <summary>
    /// RESULT-007 - Damage dealt
    /// </summary>
    public class RESULT007_DamageDealt : MonoBehaviour
    {
        public const string ID = "RESULT-007";

        public void Initialize()
        {
            Debug.Log("[Results UI] RESULT-007 initialized.");
        }
    }
}
