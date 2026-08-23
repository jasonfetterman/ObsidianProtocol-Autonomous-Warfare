using UnityEngine;

namespace ObsidianProtocol.UI.QA
{
    /// <summary>
    /// QA-011 - Controller tested
    /// PHASE 35 - FINAL UI QA
    /// </summary>
    public class QA011_ControllerTest : MonoBehaviour
    {
        public const string ID = "QA-011";

        public bool Passed { get; private set; }

        public void MarkPassed()
        {
            Passed = true;
            Debug.Log("[UI QA] QA-011 PASSED.");
        }

        public void ResetTest()
        {
            Passed = false;
        }
    }
}
