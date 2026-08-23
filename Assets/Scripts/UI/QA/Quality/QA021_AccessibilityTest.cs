using UnityEngine;

namespace ObsidianProtocol.UI.QA
{
    /// <summary>
    /// QA-021 - Accessibility tested
    /// PHASE 35 - FINAL UI QA
    /// </summary>
    public class QA021_AccessibilityTest : MonoBehaviour
    {
        public const string ID = "QA-021";

        public bool Passed { get; private set; }

        public void MarkPassed()
        {
            Passed = true;
            Debug.Log("[UI QA] QA-021 PASSED.");
        }

        public void ResetTest()
        {
            Passed = false;
        }
    }
}
