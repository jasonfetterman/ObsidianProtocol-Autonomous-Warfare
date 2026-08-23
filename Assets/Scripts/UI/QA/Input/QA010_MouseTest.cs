using UnityEngine;

namespace ObsidianProtocol.UI.QA
{
    /// <summary>
    /// QA-010 - Mouse tested
    /// PHASE 35 - FINAL UI QA
    /// </summary>
    public class QA010_MouseTest : MonoBehaviour
    {
        public const string ID = "QA-010";

        public bool Passed { get; private set; }

        public void MarkPassed()
        {
            Passed = true;
            Debug.Log("[UI QA] QA-010 PASSED.");
        }

        public void ResetTest()
        {
            Passed = false;
        }
    }
}
