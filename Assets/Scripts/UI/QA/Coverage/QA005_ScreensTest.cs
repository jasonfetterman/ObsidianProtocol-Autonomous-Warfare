using UnityEngine;

namespace ObsidianProtocol.UI.QA
{
    /// <summary>
    /// QA-005 - Every screen tested
    /// PHASE 35 - FINAL UI QA
    /// </summary>
    public class QA005_ScreensTest : MonoBehaviour
    {
        public const string ID = "QA-005";

        public bool Passed { get; private set; }

        public void MarkPassed()
        {
            Passed = true;
            Debug.Log("[UI QA] QA-005 PASSED.");
        }

        public void ResetTest()
        {
            Passed = false;
        }
    }
}
