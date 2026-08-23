using UnityEngine;

namespace ObsidianProtocol.UI.QA
{
    /// <summary>
    /// QA-007 - Every button tested
    /// PHASE 35 - FINAL UI QA
    /// </summary>
    public class QA007_ButtonsTest : MonoBehaviour
    {
        public const string ID = "QA-007";

        public bool Passed { get; private set; }

        public void MarkPassed()
        {
            Passed = true;
            Debug.Log("[UI QA] QA-007 PASSED.");
        }

        public void ResetTest()
        {
            Passed = false;
        }
    }
}
