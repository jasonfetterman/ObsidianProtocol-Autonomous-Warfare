using Assets.Scripts.AI;   // RTSBrain
using Assets.Scripts.VR;   // BaseUnitVRController
using UnityEngine;

namespace Obsidian.VR
{
    public class UnitBrainRouter : MonoBehaviour
    {
        public RTSBrain rtsBrain;
        public BaseUnitVRController vrBrain;

        private bool inVR;

        void Awake()
        {
            if (rtsBrain == null)
                rtsBrain = GetComponent<RTSBrain>();

            if (vrBrain == null)
                vrBrain = GetComponent<BaseUnitVRController>();
        }

        void Update()
        {
            if (inVR)
            {
                rtsBrain.enabled = false;
                vrBrain.enabled = true;
            }
            else
            {
                rtsBrain.enabled = true;
                vrBrain.enabled = false;
            }
        }

        public void EnterVR()
        {
            inVR = true;
            vrBrain.enabled = true;
        }

        public void ExitVR()
        {
            inVR = false;
            vrBrain.enabled = false;
        }
    }
}
