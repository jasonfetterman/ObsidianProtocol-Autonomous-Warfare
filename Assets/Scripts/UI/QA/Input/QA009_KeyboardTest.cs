using UnityEngine;

namespace ObsidianProtocol.UI.QA
{
    /// <summary>
    /// QA-009 - Keyboard tested
    /// PHASE 35 - FINAL UI QA
    /// </summary>
    public class QA009_KeyboardTest : MonoBehaviour
    {
        public const string ID = "QA-009";

        public bool Passed { get; private set; }

        public void MarkPassed()
        {
            Passed = true;
            Debug.Log("[UI QA] QA-009 PASSED.");
        }

        public void ResetTest()
        {
            Passed = false;
        }
    }
}
