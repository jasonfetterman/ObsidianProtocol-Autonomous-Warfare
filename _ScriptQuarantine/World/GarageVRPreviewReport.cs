using UnityEngine;

namespace ObsidianProtocol.World
{
    public class GarageVRPreviewReport : MonoBehaviour
    {
        [Header("Preview Systems")]
        [SerializeField]
        private GaragePreviewSetupValidator validator;

        [SerializeField]
        private GarageVRPreviewSession session;

        [Header("Runtime")]
        [SerializeField]
        private bool reportOnStart = true;

        private void Start()
        {
            if (reportOnStart)
                GenerateReport();
        }

        [ContextMenu("Generate Garage VR Report")]
        public void GenerateReport()
        {
            Debug.Log("");
            Debug.Log(
                "============================================");

            Debug.Log(
                " OBSIDIAN PROTOCOL — GARAGE VR PREVIEW");

            Debug.Log(
                "============================================");

            bool valid =
                validator != null &&
                validator.Validate();

            Debug.Log(
                $"SETUP VERIFIED: {valid}");

            if (session != null)
            {
                Debug.Log(
                    $"SESSION READY: {session.IsReady}");

                Debug.Log(
                    $"SESSION ACTIVE: {session.IsActive}");
            }

            Debug.Log(
                "VR MODE: OPTIONAL");

            Debug.Log(
                "RTS MODE: SUPPORTED");

            Debug.Log(
                "DIRECT CONTROL: SUPPORTED");

            Debug.Log(
                "FREE-ROAM: SUPPORTED");

            Debug.Log(
                "============================================");
            Debug.Log("");
        }
    }
}

