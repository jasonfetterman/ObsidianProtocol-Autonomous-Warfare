using UnityEngine;
using UnityEngine.UI;

namespace ObsidianProtocol.Game.Intelligence
{
    public sealed class IntelligenceContactFunctionController : MonoBehaviour
    {
        private Text contactSummary;

        private void Awake()
        {
            Transform summary = FindChild(transform, "CONTACT SUMMARY");

            if (summary != null)
                contactSummary = summary.GetComponent<Text>();
        }

        public void ShowUnknown()
        {
            IntelligenceRuntime runtime = IntelligenceRuntime.Instance;

            if (runtime == null || runtime.Intelligence == null)
            {
                Debug.LogWarning("[INTELLIGENCE] UNKNOWN: Intelligence runtime unavailable.");
                return;
            }

            if (!runtime.Intelligence.TryGetRecord(1001, out BattlefieldIntelligenceRecord record))
            {
                Debug.LogWarning("[INTELLIGENCE] UNKNOWN: No unknown contact found.");
                return;
            }

            if (record.Identification != IdentificationStatus.Unknown)
            {
                Debug.LogWarning("[INTELLIGENCE] UNKNOWN: No UNKNOWN contact currently available.");
                return;
            }

            if (contactSummary != null)
            {
                contactSummary.text =
                    "CONTACT SUMMARY\n\n" +
                    "UNKNOWN CONTACT\n\n" +
                    "CONTACT ID       " + record.TargetId + "\n\n" +
                    "DISTANCE         " + record.Distance.ToString("0.0") + " KM\n\n" +
                    "BEARING          " + record.Bearing.ToString("0") + "°\n\n" +
                    "CONFIDENCE       " + (record.Confidence * 100f).ToString("0") + "%\n\n" +
                    "STATUS           UNKNOWN";
            }

            Debug.Log(
                "[INTELLIGENCE] UNKNOWN CONTACT SELECTED: " +
                record.TargetId +
                " | DISTANCE=" + record.Distance.ToString("0.0") +
                " KM | CONFIDENCE=" +
                (record.Confidence * 100f).ToString("0") + "%");
        }

        private static Transform FindChild(Transform parent, string objectName)
        {
            foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
            {
                if (child.name == objectName)
                    return child;
            }

            return null;
        }
    }
}
