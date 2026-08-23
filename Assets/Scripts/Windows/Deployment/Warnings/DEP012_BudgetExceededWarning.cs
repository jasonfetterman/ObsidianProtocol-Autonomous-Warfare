using UnityEngine;

namespace ObsidianProtocol.UI.Windows.Deployment
{
    /// <summary>
    /// DEP-012 - Budget exceeded warning
    /// </summary>
    public class DEP012_BudgetExceededWarning : MonoBehaviour
    {
        public const string ID = "DEP-012";

        public void Initialize()
        {
            Debug.Log("[Windows.Deployment] DEP-012 initialized.");
        }
    }
}
