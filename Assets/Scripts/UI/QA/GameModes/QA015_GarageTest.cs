using UnityEngine;

namespace ObsidianProtocol.UI.QA
{
    /// <summary>
    /// QA-015 - Garage tested
    /// PHASE 35 - FINAL UI QA
    /// </summary>
    public class QA015_GarageTest : MonoBehaviour
    {
        public const string ID = "QA-015";

        public bool Passed { get; private set; }

        public void MarkPassed()
        {
            Passed = true;
            Debug.Log("[UI QA] QA-015 PASSED.");
        }

        public void ResetTest()
        {
            Passed = false;
        }
    }
}
