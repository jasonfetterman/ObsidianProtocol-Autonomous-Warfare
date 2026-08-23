using UnityEngine;

namespace ObsidianProtocol.UI.QA
{
    /// <summary>
    /// QA-013 - Multiplayer tested
    /// PHASE 35 - FINAL UI QA
    /// </summary>
    public class QA013_MultiplayerTest : MonoBehaviour
    {
        public const string ID = "QA-013";

        public bool Passed { get; private set; }

        public void MarkPassed()
        {
            Passed = true;
            Debug.Log("[UI QA] QA-013 PASSED.");
        }

        public void ResetTest()
        {
            Passed = false;
        }
    }
}
