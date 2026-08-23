using UnityEngine;

namespace ObsidianProtocol.UI.QA
{
    /// <summary>
    /// QA-018 - Save/load tested
    /// PHASE 35 - FINAL UI QA
    /// </summary>
    public class QA018_SaveLoadTest : MonoBehaviour
    {
        public const string ID = "QA-018";

        public bool Passed { get; private set; }

        public void MarkPassed()
        {
            Passed = true;
            Debug.Log("[UI QA] QA-018 PASSED.");
        }

        public void ResetTest()
        {
            Passed = false;
        }
    }
}
