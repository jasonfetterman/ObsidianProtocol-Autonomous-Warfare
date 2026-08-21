using UnityEngine;

namespace Obsidian.VR
{
    /// <summary>
    /// Handles VR head pose anchoring and routing for RTS, FPV, and AR modes.
    /// Works with UnitPOVCamera and any BaseUnitVRController (UGV/UAV/USV/UUV/etc).
    /// </summary>
    public class VRCameraRig : MonoBehaviour
    {
        [Header("Rig Transforms")]
        [SerializeField] private Transform _headAnchor;
        [SerializeField] private Transform _fpvAnchor;
        [SerializeField] private Transform _arAnchor;

        private Camera _currentCamera;

        private void Awake()
        {
            if (_headAnchor == null)
                _headAnchor = this.transform;

            if (_fpvAnchor == null)
                _fpvAnchor = this.transform;

            if (_arAnchor == null)
                _arAnchor = this.transform;
        }

        public void SetActiveCamera(Camera cam)
        {
            _currentCamera = cam;

            if (_currentCamera == null)
                return;

            _currentCamera.transform.SetParent(_headAnchor, false);
        }

        public void AttachToFPV(Camera povCamera)
        {
            if (povCamera == null)
                return;

            _currentCamera = povCamera;
            _currentCamera.transform.SetParent(_fpvAnchor, false);
        }

        public void AttachToAR(Camera arCamera)
        {
            if (arCamera == null)
                return;

            _currentCamera = arCamera;
            _currentCamera.transform.SetParent(_arAnchor, false);
        }

        public void ApplyHeadPose(Pose pose)
        {
            if (_headAnchor == null)
                return;

            _headAnchor.localPosition = pose.position;
            _headAnchor.localRotation = pose.rotation;
        }

        public void DisableAll()
        {
            if (_currentCamera != null)
                _currentCamera.transform.SetParent(null);

            _currentCamera = null;
        }
    }
}
