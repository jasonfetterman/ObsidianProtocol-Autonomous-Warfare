using UnityEngine;

namespace ObsidianProtocol.UI.QA
{
    /// <summary>
    /// QA-004 - Every menu tested
    /// PHASE 35 - FINAL UI QA
    /// </summary>
    public class QA004_MenusTest : MonoBehaviour
    {
        public const string ID = "QA-004";

        public bool Passed { get; private set; }

        public void MarkPassed()
        {
            Passed = true;
            Debug.Log("[UI QA] QA-004 PASSED.");
        }

        public void ResetTest()
        {
            Passed = false;
        }
    }
}
