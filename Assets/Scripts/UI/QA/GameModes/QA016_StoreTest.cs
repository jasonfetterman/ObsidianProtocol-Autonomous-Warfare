using UnityEngine;

namespace ObsidianProtocol.UI.QA
{
    /// <summary>
    /// QA-016 - Store tested
    /// PHASE 35 - FINAL UI QA
    /// </summary>
    public class QA016_StoreTest : MonoBehaviour
    {
        public const string ID = "QA-016";

        public bool Passed { get; private set; }

        public void MarkPassed()
        {
            Passed = true;
            Debug.Log("[UI QA] QA-016 PASSED.");
        }

        public void ResetTest()
        {
            Passed = false;
        }
    }
}
