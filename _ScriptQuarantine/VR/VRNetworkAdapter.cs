using UnityEngine;

namespace Obsidian.VR
{
    public class VRNetworkAdapter : MonoBehaviour
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public float Speed { get; set; }
        public float Battery { get; set; }
    }
}
