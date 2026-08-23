using UnityEngine;

namespace ObsidianProtocol.UI.Windows.Deployment
{
    /// <summary>
    /// DEP-015 - Opponent ready status
    /// </summary>
    public class DEP015_OpponentReadyStatus : MonoBehaviour
    {
        public const string ID = "DEP-015";

        public void Initialize()
        {
            Debug.Log("[Windows.Deployment] DEP-015 initialized.");
        }
    }
}
