using UnityEngine;

namespace ObsidianProtocol.UI.QA
{
    /// <summary>
    /// QA-023 - Missing-image audit
    /// PHASE 35 - FINAL UI QA
    /// </summary>
    public class QA023_MissingImageAudit : MonoBehaviour
    {
        public const string ID = "QA-023";

        public bool Passed { get; private set; }

        public void MarkPassed()
        {
            Passed = true;
            Debug.Log("[UI QA] QA-023 PASSED.");
        }

        public void ResetTest()
        {
            Passed = false;
        }
    }
}
