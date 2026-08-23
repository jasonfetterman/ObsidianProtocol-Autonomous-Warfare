using UnityEngine;

namespace ObsidianProtocol.UI.QA
{
    /// <summary>
    /// QA-006 - Every overlay tested
    /// PHASE 35 - FINAL UI QA
    /// </summary>
    public class QA006_OverlaysTest : MonoBehaviour
    {
        public const string ID = "QA-006";

        public bool Passed { get; private set; }

        public void MarkPassed()
        {
            Passed = true;
            Debug.Log("[UI QA] QA-006 PASSED.");
        }

        public void ResetTest()
        {
            Passed = false;
        }
    }
}
