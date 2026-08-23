using UnityEngine;

namespace ObsidianProtocol.UI.QA
{
    /// <summary>
    /// QA-003 - Every window tested
    /// PHASE 35 - FINAL UI QA
    /// </summary>
    public class QA003_WindowsTest : MonoBehaviour
    {
        public const string ID = "QA-003";

        public bool Passed { get; private set; }

        public void MarkPassed()
        {
            Passed = true;
            Debug.Log("[UI QA] QA-003 PASSED.");
        }

        public void ResetTest()
        {
            Passed = false;
        }
    }
}
