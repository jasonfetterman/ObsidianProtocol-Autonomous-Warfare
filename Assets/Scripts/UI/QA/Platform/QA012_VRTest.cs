using UnityEngine;

namespace ObsidianProtocol.UI.QA
{
    /// <summary>
    /// QA-012 - VR tested
    /// PHASE 35 - FINAL UI QA
    /// </summary>
    public class QA012_VRTest : MonoBehaviour
    {
        public const string ID = "QA-012";

        public bool Passed { get; private set; }

        public void MarkPassed()
        {
            Passed = true;
            Debug.Log("[UI QA] QA-012 PASSED.");
        }

        public void ResetTest()
        {
            Passed = false;
        }
    }
}
