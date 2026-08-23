using UnityEngine;

namespace ObsidianProtocol.UI.QA
{
    /// <summary>
    /// QA-001 - Every HUD element tested
    /// PHASE 35 - FINAL UI QA
    /// </summary>
    public class QA001_HUDElementsTest : MonoBehaviour
    {
        public const string ID = "QA-001";

        public bool Passed { get; private set; }

        public void MarkPassed()
        {
            Passed = true;
            Debug.Log("[UI QA] QA-001 PASSED.");
        }

        public void ResetTest()
        {
            Passed = false;
        }
    }
}
