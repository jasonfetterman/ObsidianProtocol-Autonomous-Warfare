using UnityEngine;

namespace ObsidianProtocol.UI.Windows.Deployment
{
    /// <summary>
    /// DEP-011 - Invalid deployment warning
    /// </summary>
    public class DEP011_InvalidDeploymentWarning : MonoBehaviour
    {
        public const string ID = "DEP-011";

        public void Initialize()
        {
            Debug.Log("[Windows.Deployment] DEP-011 initialized.");
        }
    }
}
