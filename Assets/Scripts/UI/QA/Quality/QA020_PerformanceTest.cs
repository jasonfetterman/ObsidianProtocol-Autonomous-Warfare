using UnityEngine;

namespace ObsidianProtocol.UI.QA
{
    /// <summary>
    /// QA-020 - Performance tested
    /// PHASE 35 - FINAL UI QA
    /// </summary>
    public class QA020_PerformanceTest : MonoBehaviour
    {
        public const string ID = "QA-020";

        public bool Passed { get; private set; }

        public void MarkPassed()
        {
            Passed = true;
            Debug.Log("[UI QA] QA-020 PASSED.");
        }

        public void ResetTest()
        {
            Passed = false;
        }
    }
}
