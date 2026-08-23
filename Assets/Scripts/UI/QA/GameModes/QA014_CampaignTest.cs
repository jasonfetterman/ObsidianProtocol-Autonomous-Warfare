using UnityEngine;

namespace ObsidianProtocol.UI.QA
{
    /// <summary>
    /// QA-014 - Campaign tested
    /// PHASE 35 - FINAL UI QA
    /// </summary>
    public class QA014_CampaignTest : MonoBehaviour
    {
        public const string ID = "QA-014";

        public bool Passed { get; private set; }

        public void MarkPassed()
        {
            Passed = true;
            Debug.Log("[UI QA] QA-014 PASSED.");
        }

        public void ResetTest()
        {
            Passed = false;
        }
    }
}
