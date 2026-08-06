using UnityEngine;

namespace Obsidian.VR
{
    public struct VRRuntimeHeadPose
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public VRRuntimeHeadPose(Vector3 pos, Quaternion rot)
        {
            Position = pos;
            Rotation = rot;
        }
    }
}
