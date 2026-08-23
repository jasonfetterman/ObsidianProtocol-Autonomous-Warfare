using UnityEngine;

namespace ObsidianProtocol.UI.QA
{
    /// <summary>
    /// QA-008 - Every input tested
    /// PHASE 35 - FINAL UI QA
    /// </summary>
    public class QA008_InputTest : MonoBehaviour
    {
        public const string ID = "QA-008";

        public bool Passed { get; private set; }

        public void MarkPassed()
        {
            Passed = true;
            Debug.Log("[UI QA] QA-008 PASSED.");
        }

        public void ResetTest()
        {
            Passed = false;
        }
    }
}
