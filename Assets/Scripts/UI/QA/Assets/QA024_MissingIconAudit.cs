using UnityEngine;

namespace ObsidianProtocol.UI.QA
{
    /// <summary>
    /// QA-024 - Missing-icon audit
    /// PHASE 35 - FINAL UI QA
    /// </summary>
    public class QA024_MissingIconAudit : MonoBehaviour
    {
        public const string ID = "QA-024";

        public bool Passed { get; private set; }

        public void MarkPassed()
        {
            Passed = true;
            Debug.Log("[UI QA] QA-024 PASSED.");
        }

        public void ResetTest()
        {
            Passed = false;
        }
    }
}
