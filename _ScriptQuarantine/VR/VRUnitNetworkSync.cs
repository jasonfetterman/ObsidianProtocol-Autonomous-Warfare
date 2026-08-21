using Assets.Scripts.VR;
using UnityEngine;

namespace Obsidian.VR
{
    public class VRUnitNetworkSync : MonoBehaviour
    {
        [SerializeField] private VRUnitContextProvider _contextProvider;

        private VRUnitContext _context;

        private void Awake()
        {
            if (_contextProvider == null)
                _contextProvider = Object.FindAnyObjectByType<VRUnitContextProvider>();
        }

        private void Update()
        {
            if (_contextProvider == null)
                return;

            if (_context == null)
                _context = _contextProvider.Context;

            if (_context == null || _context.Runtime == null)
                return;

            var unit = _context.Session?.ActiveUnit;
            if (unit == null)
                return;

            var net = _context.Runtime.Network;
            if (net == null)
                return;

            net.Position = unit.transform.position;
            net.Rotation = unit.transform.rotation;
            net.Speed = unit.GetCurrentSpeed();
            net.Battery = unit.GetBatteryLevel();
        }
    }
}
