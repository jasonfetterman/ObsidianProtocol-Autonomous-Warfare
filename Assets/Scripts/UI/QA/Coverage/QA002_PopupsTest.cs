using UnityEngine;

namespace ObsidianProtocol.UI.QA
{
    /// <summary>
    /// QA-002 - Every popup tested
    /// PHASE 35 - FINAL UI QA
    /// </summary>
    public class QA002_PopupsTest : MonoBehaviour
    {
        public const string ID = "QA-002";

        public bool Passed { get; private set; }

        public void MarkPassed()
        {
            Passed = true;
            Debug.Log("[UI QA] QA-002 PASSED.");
        }

        public void ResetTest()
        {
            Passed = false;
        }
    }
}
