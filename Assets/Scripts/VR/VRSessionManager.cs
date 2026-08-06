using UnityEngine;

namespace Obsidian.VR
{
    public class VRSessionManager : MonoBehaviour
    {
        public static VRSessionManager Instance { get; private set; }

        public enum VRState
        {
            Inactive,
            Active,
            Switching,
            Paused
        }

        public VRState State { get; private set; } = VRState.Inactive;

        [System.Serializable]
        public struct VRInfo
        {
            public VRMode Mode;
            public int ActiveUnitId;
        }

        public VRInfo Info;

        public class VRStateData
        {
            public int ActiveUnitId;
        }

        public VRStateData StateData { get; private set; } = new VRStateData();

        public bool SessionActive { get; private set; }
        public BaseUnitVRController ActiveUnit { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void SetActiveUnit(BaseUnitVRController unit)
        {
            ActiveUnit = unit;
            Info.ActiveUnitId = unit != null ? unit.UnitId : -1;
            StateData.ActiveUnitId = Info.ActiveUnitId;
        }

        public void SetActiveUnit(int unitId, string source)
        {
            Info.ActiveUnitId = unitId;
            StateData.ActiveUnitId = unitId;

            if (unitId < 0)
            {
                ActiveUnit = null;
                return;
            }

            ActiveUnit = FindUnitById(unitId);
        }

        private BaseUnitVRController FindUnitById(int id)
        {
            var units = Object.FindObjectsByType<BaseUnitVRController>();
            foreach (var u in units)
            {
                if (u.UnitId == id)
                    return u;
            }
            return null;
        }
    }
}
